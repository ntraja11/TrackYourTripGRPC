using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Sdk.Interfaces;

public interface IMemberGrpcService
{
    Task<MemberDetail> GetMemberAsync(int memberId, CancellationToken cancellationToken);

    Task<CreateMembersResponse> CreateMembersAsync(CreateMembersRequest request, CancellationToken cancellationToken);

    Task<DeleteMembersResponse> DeleteMembersAsync(DeleteMembersRequest request, CancellationToken cancellationToken);

    Task<GetAllMembersByTripResponse> GetAllMembersByTripAsync(GetAllMembersByTripRequest request, CancellationToken cancellationToken);

    Task<GetAllMembersByGroupResponse> GetAllMembersByGroupAsync(GetAllMembersByGroupRequest request, CancellationToken cancellationToken);
}
