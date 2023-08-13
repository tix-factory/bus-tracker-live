using System.Runtime.Serialization;

namespace TixFactory.BusTracker.Api;

/// <summary>
/// Response model for <see href="https://511.org/open-data/transit">511 transit operators</see>.
/// </summary>
/// <seealso href="http://api.511.org/transit/stops"/>
[DataContract]
public class ScheduledStopPointResult
{
    /// <summary>
    /// The bus stop ID.
    /// </summary>
    [DataMember(Name = "id")]
    public string Id { get; set; }

    /// <summary>
    /// The name of the bus stop.
    /// </summary>
    /// <remarks>
    /// Humans would refer to the bus stop by this name.
    /// </remarks>
    [DataMember(Name = "Name")]
    public string Name { get; set; }

    /// <summary>
    /// The physical location of the bus stop.
    /// </summary>
    [DataMember(Name = "Location")]
    public PhysicalLocation Location { get; set; }

    /// <summary>
    /// The type of stop this bus stop is.
    /// </summary>
    /// <example>
    /// onstreetBus
    /// </example>
    [DataMember(Name = "StopType")]
    public string StopType { get; set; }
}

[DataContract]
internal class ExternalContentResult
{
    [DataMember(Name = "Contents")]
    public BusStopsContentWrapperResult Data { get; set; }
}

[DataContract]
internal class BusStopsContentWrapperResult
{
    [DataMember(Name = "dataObjects")]
    public BusStopsDataWrapperResult Data { get; set; }
}

[DataContract]
internal class BusStopsDataWrapperResult
{
    [DataMember(Name = "ScheduledStopPoint")]
    public ScheduledStopPointResult[] Data { get; set; }
}
