// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

namespace Econolite.Ode.Services.EventLogger.Models;

/// <summary>
/// Tracking for an individual component within a service.
/// </summary>
public class MetricDto
{
    /// <summary>
    /// Name of an individual metric within a service such as DM.Status
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Count of how much this metric has been used.
    /// </summary>
    public long Value { get; set; }

    /// <summary>
    /// Total, Count/Second, etc, may need formatting
    /// </summary>
    public string Units { get; set; } = string.Empty;
}