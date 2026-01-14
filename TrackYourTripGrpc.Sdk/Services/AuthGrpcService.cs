using Grpc.Core;
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
       
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        try
        {            
            var response = await _authClient.LoginAsync(loginRequest, cancellationToken: cancellationToken);
            return response;

        }        
        catch (Exception ex)
        {
            LoginResponse response = new LoginResponse
            {
                IsSuccess = false,
                Token = string.Empty,
                StatusCode = (int)StatusCode.Unknown,
                ErrorMessage = $"UNEXPECTED ERROR: {ex.Message}"
            };
            return response;
        }
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        try
        {            
            var response = await _authClient.RegisterAsync(registerRequest, cancellationToken: cancellationToken);
            return response;

        }        
        catch (Exception ex)
        {
            RegisterResponse response = new RegisterResponse
            {
                IsSuccess = false,
                ErrorMessage = $"UNEXPECTED ERROR: {ex.Message}"
            };
            return response;
        }
    }
}
