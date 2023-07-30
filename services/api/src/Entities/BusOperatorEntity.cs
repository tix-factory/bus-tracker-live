using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TixFactory.BusTracker.Api.Entities;

/// <summary>
/// This table keeps track of all the different bus operators.
/// </summary>
[DataContract(Name = "bus_operators", Namespace = "bus_tracker")]
public class BusOperatorEntity
{
    /// <summary>
    /// An internal identifier for the bus operator.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// The bus operator ID, as registered with 511.
    /// </summary>
    [BsonElement("operator_id")]
    public string OperatorId { get; set; }

    /// <summary>
    /// The operator name.
    /// </summary>
    /// <example>
    /// Bay Area Rapid Transit
    /// </example>
    [BsonElement("name")]
    public string Name { get; set; }

    /// <summary>
    /// The operator short name.
    /// </summary>
    /// <example>
    /// BART
    /// </example>
    [BsonElement("short_name")]
    public string ShortName { get; set; }

    /// <summary>
    /// The 511 region in charge of monitoring this bus operator.
    /// </summary>
    [BsonElement("region")]
    public string Region { get; set; }

    /// <summary>
    /// Whether or not the vehicles for this operator are monitored.
    /// </summary>
    [BsonElement("monitored")]
    public bool Monitored { get; set; }

    /// <summary>
    /// Whether or not the operator is active/enabled.
    /// </summary>
    [BsonElement("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// When this bus operator was added.
    /// </summary>
    [BsonElement("created"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Created { get; set; }

    /// <summary>
    /// When the information for this bus operator was last updated.
    /// </summary>
    [BsonElement("updated"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Updated { get; set; }
}
