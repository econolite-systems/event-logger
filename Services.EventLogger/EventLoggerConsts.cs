// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

namespace Econolite.Ode.Services.EventLogger;

public static class EventLoggerConsts
{
    public static readonly string RedisConnection = "ConnectionStrings:Redis";
    public static readonly string CacheMetricsTTL = "Redis:MetricsTTL";
    public static readonly string CacheMetricsPrefix = "Metrics.";
    public static readonly string CacheMetricsSummary = "Metrics.Summary";
}