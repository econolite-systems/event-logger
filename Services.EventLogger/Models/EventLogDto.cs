// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Monitoring.Events;

namespace Econolite.Ode.Services.EventLogger.Models;

/// <summary>
/// EventLogDto
/// </summary>
public class EventLogDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public LogName Name { get; set; }

    /// <summary>
    /// Source
    /// </summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// TenantId
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// Level
    /// </summary>
    public EventLevel Level { get; set; }

    /// <summary>
    /// Category
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// Computer
    /// </summary>
    public string Computer { get; set; } = string.Empty;

    /// <summary>
    /// Details
    /// </summary>
    public string Details { get; set; } = string.Empty;
}
