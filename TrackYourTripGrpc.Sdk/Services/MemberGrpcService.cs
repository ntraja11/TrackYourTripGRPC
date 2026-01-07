using Grpc.Core;
using System.Diagnostics;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Sdk.Services;

public class MemberGrpcService : IMemberGrpcService
{
    private readonly Member.MemberClient _memberClient;
    public MemberGrpcService(Member.MemberClient memberClient)
    {
        _memberClient = memberClient;
    } 

    public async Task<MemberDetail> GetMemberAsync(int memberId, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetMemberRequest { Id = memberId };
            var response = await _memberClient.GetMemberAsync(request, cancellationToken: cancellationToken);
            return response.Member;
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

    public async Task<MemberDetail> CreateMemberAsync(MemberDetail memberDetail, CancellationToken cancellationToken)
    {
        try
        {
            var request = new CreateMemberRequest { 
                Email = memberDetail.Email,
                Name = memberDetail.Name,
                TripId = memberDetail.TripId
            };
            var response =  await _memberClient.CreateMemberAsync(request, cancellationToken: cancellationToken);
            return response.Member;
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

    public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken cancellationToken)
    {
        try
        {
            var request = new DeleteMemberRequest { Id = memberId };
            var response = await _memberClient.DeleteMemberAsync(request, cancellationToken: cancellationToken);
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

    public async Task<IEnumerable<MemberDetail>> GetAllMembersByTripIdAsync(int tripId, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetAllMembersByTripIdRequest { TripId = tripId };
            var response = await _memberClient.GetAllMembersByTripIdAsync(request, cancellationToken: cancellationToken);
            return response.Members;

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
