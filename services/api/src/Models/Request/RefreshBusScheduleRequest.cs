using System.Runtime.Serialization;

namespace TixFactory.BusTracker.Api;

/// <summary>
/// Request data, required to refresh the bus schedule.
/// </summary>
[DataContract]
public class RefreshBusScheduleRequest
{
    /// <summary>
    /// The bus operator 511 region.
    /// </summary>
    /// <example>
    /// api.511.org
    /// </example>
    [DataMember(Name = "region")]
    public string Region { get; set; }

    /// <summary>
    /// The ID of the bus operator in the region.
    /// </summary>
    /// <example>
    /// SM (SamTrans)
    /// </example>
    [DataMember(Name = "operatorId")]
    public string OperatorId { get; set; }
}
