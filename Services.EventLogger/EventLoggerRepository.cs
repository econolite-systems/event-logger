// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Monitoring.Events;
using Econolite.Ode.Persistence.Mongo.Context;
using Econolite.Ode.Persistence.Mongo.Repository;
using Econolite.Ode.Services.EventLogger.Extensions;
using Econolite.Ode.Services.EventLogger.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Econolite.Ode.Services.EventLogger;

/// <summary>
/// EventLoggerService
/// </summary>
public class EventLoggerRepository : DocumentRepositoryBase<EventLog, Guid>, IEventLoggerRepository
{
    /// <summary>
    /// EventLoggerService
    /// </summary>
    public EventLoggerRepository(IMongoContext mongoContext, ILogger<EventLoggerRepository> logger) : base(mongoContext, logger)
    {
    }
    
    public async Task<IEnumerable<EventLogDto>> FindAsync(DateTime beginDate, DateTime? endDate, params EventLevel[] eventLevels)
    {
        try
        {
            var filter = Builders<EventLog>.Filter.Where(x => x.Timestamp >= beginDate && eventLevels.Contains(x.Level));
            if (endDate.HasValue)
                filter &= Builders<EventLog>.Filter.Where(x => x.Timestamp < endDate.Value);
            var cursor = await ExecuteDbSetFunc(x => x.Find(filter).ToListAsync());
            return cursor.Select(x => x.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to find Event Logs");
            throw;
        }
    }

    public async Task AddAsync(UserEvent userEvent, CancellationToken cancellationToken)
    {
        var dbModel = userEvent.ToDoc();
        Add(dbModel);

        var (success, error) = await DbContext.SaveChangesAsync(cancellationToken);
        if (!success)
        {
            _logger.LogError("Did not save user event: {@event}, error: {error}", dbModel, error);
        }
    }
}
