using Grpc.Core;
using System.Diagnostics;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPC.SharedProtos.Protos;

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
            Debug.WriteLine($"gRPC ERROR: {ex.StatusCode} - {ex.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<TripDetail>> GetAllTripsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var request = new Google.Protobuf.WellKnownTypes.Empty();
            var response = await _tripClient.GetAllTripsAsync(request, cancellationToken: cancellationToken);
            return response.Trips;
        }
        catch (RpcException ex)
        {
            Debug.WriteLine($"gRPC ERROR: {ex.StatusCode} - {ex.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<TripDetail>> GetAllTripsByGroupAsync(int groupId, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetAllByGroupRequest{GroupId = groupId};
            var response = await _tripClient.GetAllTripsByGroupAsync(request, cancellationToken: cancellationToken);
            return response.Trips;
        }
        catch (RpcException ex)
        {
            Debug.WriteLine($"gRPC ERROR: {ex.StatusCode} - {ex.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            throw;
        }
    }

    public async Task<TripDetail> CreateTripAsync(TripDetail tripDetail, CancellationToken cancellationToken)
    {
        try
        {
            var request = new CreateTripRequest
            {
                Title = tripDetail.Title,
                Description = tripDetail.Description,
                CreatedByUserEmail = tripDetail.CreatedByUserEmail,
                StartDate = tripDetail.StartDate,
                EndDate = tripDetail.EndDate,
                From = tripDetail.From,
                To = tripDetail.To,
                Notes = tripDetail.Notes,
                GroupId = tripDetail.GroupId
            };
            var response = await _tripClient.CreateTripAsync(request, cancellationToken: cancellationToken);
            return response.Trip;
        }
        catch (RpcException ex)
        {
            Debug.WriteLine($"gRPC ERROR: {ex.StatusCode} - {ex.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            throw;
        }
    }

    public async Task<TripDetail> UpdateTripAsync(TripDetail tripDetail, CancellationToken cancellationToken)
    {
        try
        {
            var request = new UpdateTripRequest
            {
                Id = tripDetail.Id,
                Title = tripDetail.Title,
                Description = tripDetail.Description,
                StartDate = tripDetail.StartDate,
                From = tripDetail.From,
                To = tripDetail.To,
                Notes = tripDetail.Notes,
                EndDate = tripDetail.EndDate,
            };
            var response = await _tripClient.UpdateTripAsync(request, cancellationToken: cancellationToken);
            return response.Trip;
        }
        catch (RpcException ex)
        {
            Debug.WriteLine($"gRPC ERROR: {ex.StatusCode} - {ex.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteTripAsync(int tripId, CancellationToken cancellationToken)
    {
        try
        {
            var request = new DeleteTripRequest { Id = tripId };
            var response = await _tripClient.DeleteTripAsync(request, cancellationToken: cancellationToken);
            return response.Success;
        }
        catch (RpcException ex)
        {
            Debug.WriteLine($"gRPC ERROR: {ex.StatusCode} - {ex.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            throw;
        }
    }
}
