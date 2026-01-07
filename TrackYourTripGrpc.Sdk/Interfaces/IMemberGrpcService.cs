using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Sdk.Interfaces;

public interface IMemberGrpcService
{
    Task<MemberDetail> GetMemberAsync(int memberId, CancellationToken cancellationToken);

    Task<MemberDetail> CreateMemberAsync(MemberDetail memberDetail, CancellationToken cancellationToken);

    Task<bool> DeleteMemberAsync(int memberId, CancellationToken cancellationToken);

    Task<IEnumerable<MemberDetail>> GetAllMembersByTripIdAsync(int tripId, CancellationToken cancellationToken);
}
