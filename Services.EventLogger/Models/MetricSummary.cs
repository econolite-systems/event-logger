// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

namespace Econolite.Ode.Services.EventLogger.Models;

public class MetricSummary
{
    public string Source { get; set; } = string.Empty;

    public DateTime LastUpdated { get; set; }

    public List<ServiceMetric> Metrics { get; set; } = new();

    public MetricSummaryDto ToDto()
    {
        return new MetricSummaryDto
        {
            Source = Source,
            LastUpdated = LastUpdated,
            ServiceMetrics = Metrics.Average(m => m.Metrics.Count),
            Services = Metrics.Count,
            Metrics = Metrics.Select(m => m.ToDto()).ToList(),
        };
    }
}