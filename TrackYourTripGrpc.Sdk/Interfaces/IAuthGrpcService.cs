using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Sdk.Interfaces;

public interface IAuthGrpcService
{
    Task<LoginResponse> LoginAsync(string email, string password, CancellationToken cancellationToken);
    Task<bool> RegisterAsync(string email, string password, string Name, string groupName, bool newGroup, CancellationToken cancellationToken);
}
