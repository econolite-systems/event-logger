// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Common.Scheduler.Base.Extensions;
using Econolite.Ode.EventLogger;
using Econolite.Ode.EventLogger.Consumers;
using Econolite.Ode.Extensions.AspNet;
using Econolite.Ode.Monitoring.Events.Messaging.Extensions;
using Econolite.Ode.Monitoring.HealthChecks.Mongo.Extensions;
using Econolite.Ode.Monitoring.Metrics.Messaging.Extensions;
using Econolite.Ode.Persistence.Mongo;
using Econolite.Ode.Services.EventLogger;

await AppBuilder.BuildAndRunWebHostAsync(args, options => { options.Source = "Event Logger"; }, (builder, services) =>
{
    builder.Services.AddMongo();

    builder.Services.AddEventSourceServices(builder.Configuration);
    builder.Services.AddSingleton<IEventLoggerRepository, EventLoggerRepository>();
    builder.Services.AddHostedService<EventLogConsumer>();

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration[EventLoggerConsts.RedisConnection];
    });

    builder.Services.AddSingleton<IMetricsRepository, MetricsRepository>();
    builder.Services.AddHostedService<MetricsConsumer>();
    builder.Services.AddTimerFactory();
    builder.Services.AddHostedService<MetricAggregator>();
    builder.Services.AddMetricMonitoringSources();
}, (_, checksBuilder) => checksBuilder.AddMongoDbHealthCheck());
