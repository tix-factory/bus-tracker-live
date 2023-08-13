using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TixFactory.BusTracker.Api.Entities;

/// <summary>
/// This table keeps track of all the different bus lines, across all operators.
/// </summary>
[DataContract(Name = "bus_lines", Namespace = "bus_tracker")]
public class BusLineEntity
{
    /// <summary>
    /// An internal identifier for the bus line.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// The <see cref="BusOperatorEntity.Id"/>.
    /// </summary>
    [BsonElement("operator_id")]
    public ObjectId OperatorId { get; set; }

    /// <summary>
    /// The publicly recognizable name for the bus line.
    /// </summary>
    [BsonElement("name")]
    public string Name { get; set; }

    /// <summary>
    /// The bus number.
    /// </summary>
    [BsonElement("number")]
    public string Number { get; set; }

    /// <summary>
    /// Whether or not the bus line is enabled/active.
    /// </summary>
    [BsonElement("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// When this bus line was added.
    /// </summary>
    [BsonElement("created"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Created { get; set; }

    /// <summary>
    /// When the information for this bus line was last updated.
    /// </summary>
    [BsonElement("updated"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Updated { get; set; }
}
