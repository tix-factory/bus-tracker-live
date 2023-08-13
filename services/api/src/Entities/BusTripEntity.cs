using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TixFactory.BusTracker.Api.Entities;

/// <summary>
/// A bus trip is an individual dispatch of a scheduled bus.
/// </summary>
[DataContract(Name = "bus_trips", Namespace = "bus_tracker")]
public class BusTripEntity
{
    /// <summary>
    /// An internal identifier for the bus trip.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// The <see cref="BusScheduleEntity.Id"/>.
    /// </summary>
    [BsonElement("schedule_id")]
    public ObjectId ScheduleId { get; set; }

    /// <summary>
    /// The time the bus is scheduled to leave for this trip.
    /// </summary>
    [BsonElement("start_time")]
    public string StartTime { get; set; }

    /// <summary>
    /// The first stop the bus will depart from.
    /// </summary>
    [BsonElement("departure_stop_id")]
    public string DepartureStopId { get; set; }

    /// <summary>
    /// The final stop the bus will arrive at.
    /// </summary>
    [BsonElement("destination_stop_id")]
    public string DestinationStopId { get; set; }

    /// <summary>
    /// When this bus trip was created.
    /// </summary>
    [BsonElement("created"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Created { get; set; }

    /// <summary>
    /// When the information for this bus trip was last updated.
    /// </summary>
    [BsonElement("updated"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Updated { get; set; }
}
