using TrackYourTripGrpc.Maui.Pages.Trip;
using TrackYourTripGrpc.Maui.Utilities;

namespace TrackYourTripGrpc.Maui.Pages.Account;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        
    }

    private async void CheckAuthenticationState()
    {
        if (await AuthViewState.IsLoggedInAsync())
        {
            await AuthViewState.DecodeAndStoreClaimsAsync();
            await Shell.Current.GoToAsync($"//{nameof(TripsPage)}");
        }
        else
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        AuthViewState.ToggleLogoutButton(false);

        CheckAuthenticationState();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        AuthViewState.ToggleLogoutButton(true);
    }

}
