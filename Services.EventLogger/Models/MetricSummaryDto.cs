// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

namespace Econolite.Ode.Services.EventLogger.Models;

public class MetricSummaryDto
{
    public string Source { get; set; } = string.Empty;

    public DateTime LastUpdated { get; set; }

    public int Services { get; set; }
    
    public double ServiceMetrics { get; set; }

    public List<ServiceMetricDto> Metrics { get; set; } = new();
}