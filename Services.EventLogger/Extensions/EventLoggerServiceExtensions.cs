// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Microsoft.Extensions.DependencyInjection;

namespace Econolite.Ode.Services.EventLogger.Extensions;

/// <summary>
/// EventLoggerServiceExtensions
/// </summary>
public static class EventLoggerServiceExtensions
{
    /// <summary>
    /// Add the event logger database services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventLoggerService(this IServiceCollection services)
    {
        services.AddScoped<IEventLoggerRepository, EventLoggerRepository>();
        services.AddScoped<IMetricsRepository, MetricsRepository>();
        return services;
    }
}
