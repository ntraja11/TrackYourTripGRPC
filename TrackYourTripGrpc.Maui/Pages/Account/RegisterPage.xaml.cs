using TrackYourTripGrpc.Maui.Utilities;
using TrackYourTripGrpc.Maui.ViewModels;

namespace TrackYourTripGrpc.Maui.Pages.Account;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        AuthViewState.ToggleLogoutButton(false);
    }


    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        AuthViewState.ToggleLogoutButton(true);
    }
}