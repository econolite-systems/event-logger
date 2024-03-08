// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Common.Scheduler.Base.Timers;
using Econolite.Ode.Monitoring.Events;
using Econolite.Ode.Monitoring.Events.Extensions;
using Econolite.Ode.Monitoring.Metrics;
using Econolite.Ode.Services.EventLogger;

namespace Econolite.Ode.EventLogger;

public class MetricAggregator : BackgroundService
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly IPeriodicTimerFactory _periodicTimerFactory;
    private readonly IMetricsRepository _metricsRepository;
    private readonly UserEventFactory _userEventFactory;
    private readonly ILogger _logger;
    private readonly IMetricsCounter _loopCounter;

    public MetricAggregator(IPeriodicTimerFactory periodicTimerFactory, IMetricsRepository metricsRepository, IMetricsFactory metricsFactory, UserEventFactory userEventFactory, ILogger<MetricAggregator> logger)
    {
        _periodicTimerFactory = periodicTimerFactory;
        _metricsRepository = metricsRepository;
        _userEventFactory = userEventFactory;
        _logger = logger;

        _loopCounter = metricsFactory.GetMetricsCounter("Metrics Aggregator");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = _periodicTimerFactory.CreateTopOfMinuteTimer();

        try
        {
            _logger.LogInformation("Starting Timer");
            timer.Start(() => ExecuteCronStepAsync(stoppingToken));

            // wait for the stoppingToken to trigger stopping
            await Task.Delay(-1, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogDebug("CancellationToken signaled");
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "CancellationToken signaled");
        }

        _logger.LogInformation("Stopping timer");
        await timer.StopAsync();
        _logger.LogInformation("Timer Stopped");
    }

    private async Task ExecuteCronStepAsync(CancellationToken stoppingToken)
    {
        if (await _semaphore.WaitAsync(0, stoppingToken))
        {
            try
            {
                _logger.LogInformation("Beginning aggregate task");

                await _metricsRepository.AggregateAsync(stoppingToken);

                _logger.LogInformation("Completed aggregate task");

                _loopCounter.Increment();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aggregate failed");
                _logger.ExposeUserEvent(_userEventFactory.BuildUserEvent(EventLevel.Error, "Metrics aggregation failed."));
            }
            finally
            {
                _semaphore.Release();
            }
        }
        else
        {
            _logger.LogWarning("Did not complete previous aggregate step");
        }
    }
}