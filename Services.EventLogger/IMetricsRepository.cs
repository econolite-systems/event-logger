// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Monitoring.Metrics.Messaging;
using Econolite.Ode.Services.EventLogger.Models;

namespace Econolite.Ode.Services.EventLogger;

public interface IMetricsRepository
{
    Task AddAsync(MetricMessage argValue, CancellationToken cancellationToken);
    
    Task<IEnumerable<MetricSummaryDto>> GetSummaryAsync();

    Task AggregateAsync(CancellationToken cancellationToken);

    Task RemoveItemFromSummaryAsync(string source);
}