// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Monitoring.Events;
using Econolite.Ode.Monitoring.Events.Messaging;
using Econolite.Ode.Monitoring.Metrics;
using Econolite.Ode.Services.EventLogger;

namespace Econolite.Ode.EventLogger.Consumers;

/// <summary>
/// EventLogConsumer
/// </summary>
public class EventLogConsumer : BackgroundService
{
    private readonly IEventSource _eventSource;
    private readonly IEventLoggerRepository _eventLoggerRepository;
    private readonly ILogger<EventLogConsumer> _logger;
    private readonly IMetricsCounter _loopCounter;

    /// <summary>
    /// EventLogConsumer
    /// </summary>
    public EventLogConsumer(IEventSource eventSource, IEventLoggerRepository eventLoggerRepository, IMetricsFactory metricsFactory, ILogger<EventLogConsumer> logger)
    {
        _eventSource = eventSource;
        _eventLoggerRepository = eventLoggerRepository;
        _logger = logger;
        
        _loopCounter = metricsFactory.GetMetricsCounter("Events");
    }

    /// <summary>
    /// ExecuteAsync
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = Task.Run(async () =>
        {
            _logger.LogInformation("Starting the main loop");
            try
            {
                await _eventSource.ConsumeOnAsync(ProcessEventAsync, stoppingToken);
                _loopCounter.Increment();
            }
            finally
            {
                _logger.LogInformation("Ending the main loop");
            }
        }, stoppingToken);

        return Task.CompletedTask;
    }

    private async Task ProcessEventAsync(UserEvent arg)
    {
        try
        {
            await _eventLoggerRepository.AddAsync(arg, CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to save user event: {@event}", arg);
            throw;
        }
    }
}
