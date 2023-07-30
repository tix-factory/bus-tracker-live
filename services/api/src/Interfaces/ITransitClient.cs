using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TixFactory.BusTracker.Api;

/// <summary>
/// A client used to access transit data.
/// </summary>
public interface ITransitClient
{
    /// <summary>
    /// Loads all known transit operators.
    /// </summary>
    /// <param name="cancellationToken">The <seealso cref="CancellationToken"/>.</param>
    /// <returns>The transit operators.</returns>
    Task<IReadOnlyCollection<TransitOperatorResult>> LoadTransitOperatorsAsync(CancellationToken cancellationToken);
}
