using TrackYourTripGRPC.SharedProtos.Protos;

namespace TrackYourTripGrpc.Sdk.Interfaces;

public interface IAuthGrpcService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
}
