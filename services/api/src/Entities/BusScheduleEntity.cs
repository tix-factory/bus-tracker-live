using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TixFactory.BusTracker.Api.Entities;

/// <summary>
/// This table keeps track of all the different bus schedules, for each bus line.
/// </summary>
[DataContract(Name = "bus_schedules", Namespace = "bus_tracker")]
public class BusScheduleEntity
{
    /// <summary>
    /// An internal identifier for the bus schedule.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// The <see cref="BusLineEntity.Id"/>.
    /// </summary>
    [BsonElement("line_id")]
    public ObjectId LineId { get; set; }

    /// <summary>
    /// The day of the week the bus schedule is for.
    /// </summary>
    [BsonElement("day")]
    public string Day { get; set; }

    /// <summary>
    /// When this bus schedule was created.
    /// </summary>
    [BsonElement("created"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Created { get; set; }

    /// <summary>
    /// When the information for this bus schedule was last updated.
    /// </summary>
    [BsonElement("updated"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Updated { get; set; }
}
