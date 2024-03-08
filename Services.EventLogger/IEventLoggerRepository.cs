// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Monitoring.Events;
using Econolite.Ode.Services.EventLogger.Models;

namespace Econolite.Ode.Services.EventLogger;

/// <summary>
/// IEventLoggerService
/// </summary>
public interface IEventLoggerRepository
{
    /// <summary>
    /// FindAsync
    /// </summary>
    Task<IEnumerable<EventLogDto>> FindAsync(DateTime beginDate, DateTime? endDate, params EventLevel[] eventLevels);
    
    Task AddAsync(UserEvent userEvent, CancellationToken cancellationToken);
}
