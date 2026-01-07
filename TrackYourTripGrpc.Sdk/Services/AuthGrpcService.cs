using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Sdk.Services;

public class AuthGrpcService : IAuthGrpcService
{
    private readonly Auth.AuthClient _authClient;

    public AuthGrpcService(Auth.AuthClient authClient)
    {
        _authClient = authClient;
    }
       
    public async Task<LoginResponse> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        try
        {
            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };
            var response = await _authClient.LoginAsync(request, cancellationToken: cancellationToken);
            return response;

        }
        catch (Grpc.Core.RpcException ex)
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

    public async Task<bool> RegisterAsync(string email, string password, string Name, string groupName, bool newGroup, CancellationToken cancellationToken)
    {
        try
        {
            var request = new RegisterRequest
            {
                Email = email,
                Password = password,
                Name = Name,
                GroupName = groupName,
                NewGroup = newGroup
            };
            var response = await _authClient.RegisterAsync(request, cancellationToken: cancellationToken);
            return response.Status;

        }
        catch (Grpc.Core.RpcException ex)
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
