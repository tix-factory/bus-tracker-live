using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TixFactory.BusTracker.Api.Entities;

/// <summary>
/// All the bus stops in existence.
/// </summary>
[DataContract(Name = "bus_stops", Namespace = "bus_tracker")]
public class BusStopEntity
{
    /// <summary>
    /// An internal identifier for the bus stop.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// The bus stop ID, as it known with the regional bus operator.
    /// </summary>
    [BsonElement("stop_id")]
    public string StopId { get; set; }

    /// <summary>
    /// The <see cref="BusOperatorEntity.Id"/> that operates this bus stop.
    /// </summary>
    [BsonElement("operator_id")]
    public ObjectId OperatorId { get; set; }

    /// <summary>
    /// The name of the bus stop.
    /// </summary>
    [BsonElement("name")]
    public string Name { get; set; }

    /// <summary>
    /// The physical latitude position of this bus stop.
    /// </summary>
    [BsonElement("latitude")]
    public float Latitude { get; set; }

    /// <summary>
    /// The physical longitude position of this bus stop.
    /// </summary>
    [BsonElement("longitude")]
    public float Longitude { get; set; }

    /// <summary>
    /// When this bus stop was created.
    /// </summary>
    [BsonElement("created"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Created { get; set; }

    /// <summary>
    /// When this bus stop was last updated.
    /// </summary>
    [BsonElement("updated"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Updated { get; set; }
}
