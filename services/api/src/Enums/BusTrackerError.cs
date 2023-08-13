namespace TixFactory.BusTracker.Api;

/// <summary>
/// Errors that can occur with the bus tracker.
/// </summary>
public enum BusTrackerError
{
    /// <summary>
    /// Unknown error code (reserved for default)
    /// </summary>
    Invalid = 0,

    /// <summary>
    /// The bus operator is invalid.
    /// </summary>
    InvalidBusOperator = 1,
}
