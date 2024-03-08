// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

namespace Econolite.Ode.Services.EventLogger.Models;

public class ServiceMetricDto
{
    public string Id { get; set; } = string.Empty;

    public DateTime Logged { get; set; }

    public Guid TenantId { get; set; }

    public string Source { get; set; } = string.Empty;

    public string Computer { get; set; } = string.Empty;

    public string InstanceHash { get; set; } = string.Empty;

    public List<MetricDto> Metrics { get; set; } = new();
}