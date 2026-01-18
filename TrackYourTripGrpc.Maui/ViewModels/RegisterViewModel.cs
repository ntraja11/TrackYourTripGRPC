using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TrackYourTripGrpc.Maui.Pages.Account;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPC.SharedProtos.Protos;

namespace TrackYourTripGrpc.Maui.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string groupName = string.Empty;

    [ObservableProperty]
    private bool isNewGroup = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowErrorMessage))]
    private string errorMessage = string.Empty;

    private readonly IAuthGrpcService _authService;

    public bool ShowErrorMessage => !string.IsNullOrEmpty(ErrorMessage);


    public RegisterViewModel(IAuthGrpcService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task RegisterAsync(CancellationToken cancellationToken = default)
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Name))
        {
            ErrorMessage = "Name is required.";
            return;
        }
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
        if (string.IsNullOrWhiteSpace(GroupName))
        {
            ErrorMessage = "Group name is required.";
            return;
        }

        RegisterRequest request = new RegisterRequest
        {
            Name = Name,
            Email = Email,
            Password = Password,
            GroupName = GroupName,
            IsNewGroup = IsNewGroup
        };

        RegisterResponse response = await _authService.RegisterAsync(request, cancellationToken);

        if (response.IsSuccess)
        {
            await Shell.Current.DisplayAlertAsync("Success", "Registration successful!", "OK");
            ResetDataInput();
            await Task.Delay(300);
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        else
        {
            ErrorMessage = response.ErrorMessage;
            return;
        }
    }

    private void ResetDataInput()
    {
        Name = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        GroupName = string.Empty;
        IsNewGroup = false;
    }
}
