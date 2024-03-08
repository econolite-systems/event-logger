// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using MongoDB.Bson.Serialization.Attributes;

namespace Econolite.Ode.Services.EventLogger.Models;

public class Metric
{
    public Metric() { }

    public Metric(Monitoring.Metrics.Messaging.Metric metric)
    {
        Name = metric.Name;
        Value = metric.Value;
        Units = metric.Units;
    }

    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("Value")]
    public long Value { get; set; }

    [BsonElement("Units")]
    public string Units { get; set; } = string.Empty;

    public MetricDto ToDto()
    {
        return new MetricDto
        {
            Name = Name,
            Value = Value,
            Units = Units,
        };
    }
}