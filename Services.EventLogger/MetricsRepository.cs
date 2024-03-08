// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Monitoring.Metrics.Messaging;
using Econolite.Ode.Services.EventLogger.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Econolite.Ode.Services.EventLogger;

public class MetricsRepository : IMetricsRepository
{
    private readonly ILogger<MetricsRepository> _logger;
    private readonly Lazy<ConnectionMultiplexer> _lazyConnectionMultiplexer;
    private ConnectionMultiplexer Redis => _lazyConnectionMultiplexer.Value;

    public MetricsRepository(IConfiguration configuration, ILogger<MetricsRepository> logger)
    {
        _logger = logger;
        
        var redisConnectionString = configuration[EventLoggerConsts.RedisConnection] ?? throw new NullReferenceException($"{EventLoggerConsts.RedisConnection} missing from config.");
        _lazyConnectionMultiplexer = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisConnectionString));
    }

    public async Task AddAsync(MetricMessage argValue, CancellationToken cancellationToken)
    {
        var metric = new ServiceMetric(argValue);
        var serialized = System.Text.Json.JsonSerializer.Serialize(metric);
        var db = Redis.GetDatabase();
        await db.StringSetAsync(metric.Id, serialized, TimeSpan.FromMinutes(30));
    }

    /// <summary>
    /// Combine entries together if stored in redis to have a single summary doc to pull for UI?
    /// </summary>
    public async Task AggregateAsync(CancellationToken cancellationToken)
    {
        var db = Redis.GetDatabase();
        var endpoint = Redis.GetEndPoints().First();
        var redisServer = Redis.GetServer(endpoint);
        var serviceStatuses = new List<ServiceMetric>();
        await foreach (var key in redisServer.KeysAsync(pattern: $"{EventLoggerConsts.CacheMetricsPrefix}*").WithCancellation(cancellationToken))
        {
            try
            {
                if (string.Compare(key.ToString(), EventLoggerConsts.CacheMetricsSummary, StringComparison.InvariantCulture) != 0)
                {
                    var metric = System.Text.Json.JsonSerializer.Deserialize<ServiceMetric>(await db.StringGetAsync(key));
                    if (metric != null)
                        serviceStatuses.Add(metric);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deserializing stored service metric for key: {key}", key);
            }
        }

        var metricSummary = new List<MetricSummary>();
        foreach (var metrics in serviceStatuses.GroupBy(m => m.Source))
        {
            metricSummary.Add(new MetricSummary
            {
                LastUpdated = DateTime.UtcNow,
                Source = metrics.Key,
                Metrics = metrics.OrderBy(m => m.Computer).ThenBy(m => m.InstanceHash).ToList(),
            });
        }

        await db.StringSetAsync(EventLoggerConsts.CacheMetricsSummary, System.Text.Json.JsonSerializer.Serialize(metricSummary.OrderBy(m => m.Source).ToArray()), TimeSpan.FromMinutes(30));
    }

    public async Task<IEnumerable<MetricSummaryDto>> GetSummaryAsync()
    {
        var db = Redis.GetDatabase();
        var results = System.Text.Json.JsonSerializer.Deserialize<MetricSummary[]>(await db.StringGetAsync(EventLoggerConsts.CacheMetricsSummary));
        return results?.Select(ms => ms.ToDto()).OrderBy(ms => ms.Source).ToArray() ?? Array.Empty<MetricSummaryDto>();
    }

    /// <summary>
    /// Remove item from combined set if stored in redis
    /// </summary>
    public async Task RemoveItemFromSummaryAsync(string source)
    {
        var db = Redis.GetDatabase();
        var summary = System.Text.Json.JsonSerializer.Deserialize<ServiceMetric[]>((await db.StringGetAsync(EventLoggerConsts.CacheMetricsSummary)).ToString());

        if (summary != null)
        {
            summary = summary.Where(s => string.Compare(s.Source, source, StringComparison.InvariantCulture) != 0).ToArray();
            await db.StringSetAsync(EventLoggerConsts.CacheMetricsSummary, System.Text.Json.JsonSerializer.Serialize(summary), TimeSpan.FromMinutes(30));
        }
    }
}