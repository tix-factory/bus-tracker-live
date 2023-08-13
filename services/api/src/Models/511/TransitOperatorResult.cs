using System.Runtime.Serialization;

namespace TixFactory.BusTracker.Api;

/// <summary>
/// Response model for <see href="https://511.org/open-data/transit">511 transit operators</see>.
/// </summary>
/// <seealso href="http://api.511.org/transit/operators"/>
[DataContract]
public class TransitOperatorResult
{
    /// <summary>
    /// The operator ID.
    /// </summary>
    [DataMember(Name = "Id")]
    public string Id { get; set; }

    /// <summary>
    /// The operator name.
    /// </summary>
    /// <example>
    /// Bay Area Rapid Transit
    /// </example>
    [DataMember(Name = "Name")]
    public string Name { get; set; }

    /// <summary>
    /// The operator short name.
    /// </summary>
    /// <example>
    /// BART
    /// </example>
    [DataMember(Name = "ShortName")]
    public string ShortName { get; set; }

    /// <summary>
    /// The primary mode of transportation for this operator.
    /// </summary>
    /// <example>
    /// bus, cableway, metro, rail, ferry
    /// </example>
    [DataMember(Name = "PrimaryMode")]
    public string PrimaryMode { get; set; }

    /// <summary>
    /// Whether or not the vehicles for this operator are monitored.
    /// </summary>
    [DataMember(Name = "Monitored")]
    public bool Monitored { get; set; }

    /// <summary>
    /// The region the transit operator belongs to.
    /// </summary>
    [IgnoreDataMember]
    internal string Region { get; set; }
}
