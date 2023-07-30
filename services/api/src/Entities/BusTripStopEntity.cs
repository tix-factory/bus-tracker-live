using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TixFactory.BusTracker.Api.Entities;

/// <summary>
/// The stops a bus will make along its trip.
/// </summary>
[DataContract(Name = "bus_trip_stops", Namespace = "bus_tracker")]
public class BusTripStopEntity
{
    /// <summary>
    /// An internal identifier for the bus trip stop.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// The <see cref="BusTripEntity.Id"/>.
    /// </summary>
    [BsonElement("trip_id")]
    public ObjectId TripId { get; set; }

    /// <summary>
    /// The ID of the bus stop.
    /// </summary>
    [BsonElement("stop_id")]
    public string StopId { get; set; }

    /// <summary>
    /// The time the bus is scheduled to arrive at this stop.
    /// </summary>
    [BsonElement("scheduled_time")]
    public string ScheduledTime { get; set; }

    /// <summary>
    /// The estimated time the bus will arrive at this stop.
    /// </summary>
    [BsonElement("estimated_time")]
    public string EstimatedTime { get; set; }

    /// <summary>
    /// When record was created.
    /// </summary>
    [BsonElement("created"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Created { get; set; }

    /// <summary>
    /// When this record was last updated.
    /// </summary>
    [BsonElement("updated"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Updated { get; set; }
}
