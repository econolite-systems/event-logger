// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

namespace Econolite.Ode.Services.EventLogger.Models;

public class ServiceMetric
{
    public ServiceMetric() { }

    public ServiceMetric(Monitoring.Metrics.Messaging.MetricMessage metric)
    {
        Logged = metric.Logged ?? DateTime.UtcNow;
        if (Guid.TryParse(metric.TenantId?.Value?.ToString() ?? string.Empty, out var tenantId))
            TenantId = tenantId;
        Source = metric.Source;
        Computer = metric.Computer;
        InstanceHash = metric.InstanceHash;
        Id = $"{EventLoggerConsts.CacheMetricsPrefix}{TenantId}.{Source}";
        Metrics = metric.Metrics.Select(m => new Metric(m)).OrderBy(m => m.Name).ToList();
    }

    public string Id { get; set; } = string.Empty;

    public DateTime Logged { get; set; }

    public Guid TenantId { get; set; }

    public string Source { get; set; } = string.Empty;

    public string Computer { get; set; } = string.Empty;

    public string InstanceHash { get; set; } = string.Empty;

    public List<Metric> Metrics { get; set; } = new();

    public ServiceMetricDto ToDto()
    {
        return new ServiceMetricDto
        {
            Id = Id,
            Logged = Logged,
            TenantId = TenantId,
            Source = Source,
            Computer = Computer,
            InstanceHash = InstanceHash,
            Metrics = Metrics.Select(m => m.ToDto()).ToList(),
        };
    }
}