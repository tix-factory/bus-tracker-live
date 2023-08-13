using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using TixFactory.BusTracker.Api.Entities;
using TixFactory.MongoDB;
using TixFactory.Operations;

namespace TixFactory.BusTracker.Api;

/// <summary>
/// Refreshes the bus schedule for a given operator.
/// </summary>
public class RefreshBusScheduleOperation : IAsyncAction<RefreshBusScheduleRequest>
{
    private readonly IMongoCollection<BusOperatorEntity> _BusOperators;
    private readonly IMongoCollection<BusStopEntity> _BusStops;
    private readonly ITransitClient _TransitClient;

    /// <summary>
    /// Initializes a new <seealso cref="RefreshBusScheduleOperation"/>.
    /// </summary>
    /// <param name="transitClient">The <seealso cref="ITransitClient"/>.</param>
    /// <param name="busOperators">The <seealso cref="IMongoCollection{TDocument}"/> for <seealso cref="BusOperatorEntity"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="transitClient"/>
    /// - <paramref name="busOperators"/>
    /// </exception>
    public RefreshBusScheduleOperation(
        ITransitClient transitClient,
        IMongoCollection<BusOperatorEntity> busOperators,
        IMongoCollection<BusStopEntity> busStops,
        IMongoCollection<BusLineEntity> busLines,
        IMongoCollection<BusTripEntity> busTrips)
    {
        _TransitClient = transitClient ?? throw new ArgumentNullException(nameof(transitClient));
        _BusOperators = busOperators ?? throw new ArgumentNullException(nameof(busOperators));
        _BusStops = busStops ?? throw new ArgumentNullException(nameof(busStops));
    }

    /// <inheritdoc cref="IAsyncAction{TInput}.ExecuteAsync"/>
    public async Task<OperationError> ExecuteAsync(RefreshBusScheduleRequest request, CancellationToken cancellationToken)
    {
        var busOperator = await _BusOperators.FindOneAsync(e => e.Region == request.Region && e.OperatorId == request.OperatorId, cancellationToken);
        if (busOperator == null)
        {
            return new OperationError(BusTrackerError.InvalidBusOperator);
        }

        var existingBusStops = await _BusStops.FindAsync(e => e.OperatorId == busOperator.Id, new FindOptions<BusStopEntity>
        {
            Limit = 10000
        }, cancellationToken);
        var busStopsList = await existingBusStops.ToListAsync(cancellationToken);
        var busStopsById = busStopsList.ToDictionary(kvp => kvp.StopId, kvp => kvp);
        var busStops = await _TransitClient.LoadBusStopsAsync(busOperator.OperatorId, cancellationToken);

        var now = DateTime.UtcNow;
        await Task.WhenAll(busStops.Select(async busStop =>
        {
            if (busStopsById.TryGetValue(busStop.Id, out var existingBusStop))
            {
                if (existingBusStop.Name == busStop.Name
                    && existingBusStop.Latitude.Equals(busStop.Location.Latitude)
                    && existingBusStop.Longitude.Equals(busStop.Location.Longitude))
                {
                    return;
                }

                await _BusStops.UpdateOneAsync(e => e.Id == existingBusStop.Id, e =>
                {
                    e.Name = busStop.Name;
                    e.Longitude = busStop.Location.Longitude;
                    e.Latitude = busStop.Location.Latitude;
                    e.Updated = now;
                    return true;
                }, cancellationToken);
            }
            else
            {
                await _BusStops.InsertOneAsync(new BusStopEntity
                {
                    Name = busStop.Name,
                    OperatorId = busOperator.Id,
                    StopId = busStop.Id,
                    Latitude = busStop.Location.Latitude,
                    Longitude = busStop.Location.Longitude,
                    Created = now,
                    Updated = now
                }, new InsertOneOptions
                {
                    Comment = CollectionExtensions.WriteComment
                }, cancellationToken);
            }
        }));

        return null;
    }
}
