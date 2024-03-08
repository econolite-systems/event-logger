// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Authorization;
using Econolite.Ode.Monitoring.Events;
using Econolite.Ode.Services.EventLogger.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Econolite.Ode.Services.EventLogger;

/// <summary>
/// EventLoggerController
/// </summary>
[ApiController]
[Route("event-logger")]
[AuthorizeOde(MoundRoadRole.ReadOnly)]
public class EventLoggerController : ControllerBase
{
    private readonly ILogger<EventLoggerController> _logger;

    private readonly IEventLoggerRepository _eventLoggerService;

    /// <summary>
    /// EventLoggerController
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="eventLoggerService"></param>
    public EventLoggerController(ILogger<EventLoggerController> logger, IEventLoggerRepository eventLoggerService)
    {
        _logger = logger;
        _eventLoggerService = eventLoggerService;
    }

    /// <summary>
    /// Find
    /// </summary>
    /// <param name="beginDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    [HttpGet("find")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EventLogDto>))]
    public async Task<IActionResult> FindAsync([FromQuery] DateTime beginDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var eventLogs = await _eventLoggerService.FindAsync(beginDate, endDate, EventLevel.Information); // Debug too?
            return Ok(eventLogs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to find Event Logs");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to find Event Logs");
        }
    }

    /// <summary>
    /// Errors
    /// </summary>
    /// <param name="beginDate"></param>
    /// <param name="endDate"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    [HttpGet("errors")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EventLogDto>))]
    public async Task<IActionResult> ErrorsAsync([FromQuery] DateTime beginDate, [FromQuery] DateTime? endDate, [FromQuery] int? limit)
    {
        try
        {
            var eventLogs = await _eventLoggerService.FindAsync(beginDate, endDate, EventLevel.Warning, EventLevel.Error, EventLevel.Critical);
            if (limit.HasValue && limit.Value > 0)
                eventLogs = eventLogs.Take(limit.Value);
            return Ok(eventLogs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to find error logs");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to find error logs");
        }
    }
}
