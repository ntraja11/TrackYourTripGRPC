using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Sdk.Interfaces;

public interface ITripGrpcService
{
    Task<TripDetail> GetTripAsync(int tripId, CancellationToken cancellationToken);
    Task<IEnumerable<TripDetail>> GetAllTripsAsync(CancellationToken cancellationToken);
    Task<TripDetail> CreateTripAsync(TripDetail tripDetail, CancellationToken cancellationToken);

    Task<TripDetail> UpdateTripAsync(TripDetail tripDetail, CancellationToken cancellationToken);

    Task<bool> DeleteTripAsync(int tripId, CancellationToken cancellationToken);
}
