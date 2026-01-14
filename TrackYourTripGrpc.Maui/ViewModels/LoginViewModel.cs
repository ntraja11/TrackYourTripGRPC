using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TrackYourTripGrpc.Maui.Pages;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Maui.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowErrorMessage))]
    private string errorMessage = string.Empty;

    private readonly IAuthGrpcService _authService;

    public bool ShowErrorMessage => !string.IsNullOrEmpty(ErrorMessage);


    public LoginViewModel(IAuthGrpcService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task LoginAsync(CancellationToken cancellationToken = default)
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Email))
        {
            ErrorMessage = "Email is required.";
            return;
        }
        if (string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Password is required.";
            return;
        }

        LoginResponse response = await _authService.LoginAsync(Email, Password, cancellationToken);

        if (response.Status)
        {
            await SecureStorage.SetAsync(AppConstants.AuthTokenKey, response.Token);
            await Shell.Current.GoToAsync($"//{nameof(TripsPage)}");
        }
        else
        {
            ErrorMessage = response.ErrorMessage;
            return;
        }


    }
}
