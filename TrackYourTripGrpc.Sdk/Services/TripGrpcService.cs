using Grpc.Core;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Sdk.Services;

public class TripGrpcService : ITripGrpcService
{
    private readonly Trip.TripClient _tripClient;
    public TripGrpcService(Trip.TripClient tripClient)
    {
        _tripClient = tripClient;
    }
    public async Task<TripDetail> GetTripAsync(int tripId, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetTripRequest { Id = tripId };
            var response = await _tripClient.GetTripAsync(request, cancellationToken: cancellationToken);
            return response.Trip;
        }
        catch (RpcException ex)
        {
            System.Diagnostics.Debug.WriteLine($"gRPC ERROR: {ex.StatusCode} - {ex.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            throw;
        }

    }
}
