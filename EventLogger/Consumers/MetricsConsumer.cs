// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Messaging.Elements;
using Econolite.Ode.Monitoring.Metrics;
using Econolite.Ode.Monitoring.Metrics.Messaging;
using Econolite.Ode.Services.EventLogger;

namespace Econolite.Ode.EventLogger.Consumers;

public class MetricsConsumer : BackgroundService
{
    private readonly IMetricSource _metricSource;
    private readonly IMetricsRepository _metricsRepository;
    private readonly ILogger<MetricsConsumer> _logger;
    private readonly IMetricsCounter _loopCounter;

    /// <summary>
    /// MetricsConsumer
    /// </summary>
    public MetricsConsumer(IMetricSource metricSource, IMetricsRepository metricsRepository, IMetricsFactory metricsFactory, ILogger<MetricsConsumer> logger)
    {
        _metricSource = metricSource;
        _metricsRepository = metricsRepository;
        _logger = logger;

        _loopCounter = metricsFactory.GetMetricsCounter("Metrics");
    }

    /// <summary>
    /// ExecuteAsync
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = Task.Run(async () =>
        {
            _logger.LogInformation("Starting the main loop");
            try
            {
                await _metricSource.ConsumeOnAsync(ProcessMetricsAsync, stoppingToken);
                _loopCounter.Increment();
            }
            finally
            {
                _logger.LogInformation("Ending the main loop");
            }
        }, stoppingToken);

        return Task.CompletedTask;
    }

    private async Task ProcessMetricsAsync(ConsumeResult<Guid, MetricMessage> arg)
    {
        try
        {
            await _metricsRepository.AddAsync(arg.Value, CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to save metrics: {@event}", arg);
            throw;
        }
    }
}