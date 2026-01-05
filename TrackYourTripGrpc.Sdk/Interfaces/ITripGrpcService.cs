using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Sdk.Interfaces;

public interface ITripGrpcService
{
    Task<TripDetail> GetTripAsync(int tripId, CancellationToken cancellationToken);
}
