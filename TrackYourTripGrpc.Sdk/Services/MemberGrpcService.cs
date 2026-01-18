using Grpc.Core;
using System.Diagnostics;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPC.SharedProtos.Protos;

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

    public async Task<CreateMembersResponse> CreateMembersAsync(CreateMembersRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _memberClient.CreateMembersAsync(request, cancellationToken: cancellationToken);
            return response;
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

    public async Task<DeleteMembersResponse> DeleteMembersAsync(DeleteMembersRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _memberClient.DeleteMembersAsync(request, cancellationToken: cancellationToken);
            return response;
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

    public async Task<GetAllMembersByTripResponse> GetAllMembersByTripAsync(GetAllMembersByTripRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _memberClient.GetAllMembersByTripAsync(request, cancellationToken: cancellationToken);
            return response;

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

    public async Task<GetAllMembersByGroupResponse> GetAllMembersByGroupAsync(GetAllMembersByGroupRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _memberClient.GetAllMembersByGroupAsync(request, cancellationToken: cancellationToken);
            return response;

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
