// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Authorization;
using Econolite.Ode.Services.EventLogger.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Econolite.Ode.Services.EventLogger;

/// <summary>
/// EventLoggerController
/// </summary>
[ApiController]
[Route("system-metrics")]
[AuthorizeOde(MoundRoadRole.ReadOnly)]
public class MetricsController : ControllerBase
{
    private readonly ILogger<MetricsController> _logger;

    private readonly IMetricsRepository _metricsRepository;

    /// <summary>
    /// EventLoggerController
    /// </summary>
    public MetricsController(ILogger<MetricsController> logger, IMetricsRepository metricsRepository)
    {
        _logger = logger;
        _metricsRepository = metricsRepository;
    }

    /// <summary>
    /// GetSummaryAsync
    /// </summary>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MetricSummaryDto>))]
    public async Task<IActionResult> GetSummaryAsync()
    {
        try
        {
            var eventLogs = await _metricsRepository.GetSummaryAsync();
            return Ok(eventLogs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get metrics");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get metrics");
        }
    }

    /// <summary>
    /// RemoveItemFromSummaryAsynca
    /// </summary>
    [HttpPost("remove/{source}")]
    [AuthorizeOde(MoundRoadRole.Administrator)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveItemFromSummaryAsync(string source)
    {
        try
        {
            await _metricsRepository.RemoveItemFromSummaryAsync(source);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove metrics with source: {source}", source);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to remove metrics with source: {source}");
        }
    }
}
