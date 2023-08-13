using System.Runtime.Serialization;

namespace TixFactory.BusTracker.Api;

/// <summary>
/// Physical location data.
/// </summary>
public class PhysicalLocation
{
    /// <summary>
    /// The latitude.
    /// </summary>
    [DataMember(Name = "latitude")]
    public double Latitude { get; set; }

    /// <summary>
    /// The longitude.
    /// </summary>
    [DataMember(Name = "longitude")]
    public double Longitude { get; set; }
}
