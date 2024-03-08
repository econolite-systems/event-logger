// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Monitoring.Events;
using Econolite.Ode.Persistence.Common.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Econolite.Ode.Services.EventLogger.Models;

/// <summary>
/// EventLogDoc
/// </summary>
public class EventLog : IndexedEntityBase<Guid>
{
    /// <summary>
    /// Timestamp
    /// </summary>
    [BsonElement("Timestamp")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    [BsonElement("Name")]
    public LogName Name { get; set; }

    /// <summary>
    /// Source
    /// </summary>
    [BsonElement("Source")]
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// TenantId
    /// </summary>
    [BsonElement("TenantId")]
    public Guid? TenantId { get; set; }

    /// <summary>
    /// Level
    /// </summary>
    [BsonElement("Level")]
    public EventLevel Level { get; set; }
    
    /// <summary>
    /// Category
    /// </summary>
    [BsonElement("Category")]
    public Category Category { get; set; }

    /// <summary>
    /// Computer
    /// </summary>
    [BsonElement("Computer")]
    public string Computer { get; set; } = string.Empty;

    /// <summary>
    /// Details
    /// </summary>
    [BsonElement("Details")]
    public string Details { get; set; } = string.Empty;
}
