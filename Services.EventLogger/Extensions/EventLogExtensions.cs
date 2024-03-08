// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Monitoring.Events;
using Econolite.Ode.Services.EventLogger.Models;

namespace Econolite.Ode.Services.EventLogger.Extensions;

/// <summary>
/// EventLogExtensions
/// </summary>
public static class EventLogExtensions
{
    /// <summary>
    /// ToDto
    /// </summary>
    /// <param name="eventLogDoc"></param>
    /// <returns></returns>
    public static EventLogDto ToDto(this EventLog eventLogDoc)
    {
        return new EventLogDto
        {
            Id = eventLogDoc.Id,
            Timestamp = eventLogDoc.Timestamp,
            Name = eventLogDoc.Name,
            Source = eventLogDoc.Source,
            TenantId = eventLogDoc.TenantId,
            Level = eventLogDoc.Level,
            Category = eventLogDoc.Category,
            Computer = eventLogDoc.Computer,
            Details = eventLogDoc.Details,
        };
    }

    /// <summary>
    /// ToDoc
    /// </summary>
    /// <param name="eventLogDto"></param>
    /// <returns></returns>
    public static EventLog ToDoc(this EventLogDto eventLogDto)
    {
        return new EventLog
        {
            Id = eventLogDto.Id,
            Timestamp = eventLogDto.Timestamp,
            Name = eventLogDto.Name,
            Source = eventLogDto.Source,
            TenantId = eventLogDto.TenantId,
            Level = eventLogDto.Level,
            Category = eventLogDto.Category,
            Computer = eventLogDto.Computer,
            Details = eventLogDto.Details,
        };
    }

    public static EventLog ToDoc(this UserEvent eventLog)
    {
        return new EventLog
        {
            Id = Guid.NewGuid(),
            Timestamp = eventLog.Logged ?? DateTime.UtcNow,
            Name = eventLog.LogName,
            Source = eventLog.Source,
            TenantId = eventLog.TenantId,
            Level = eventLog.Level,
            Category = eventLog.Category,
            Computer = eventLog.Computer,
            Details = eventLog.Details,
        };
    }
}
