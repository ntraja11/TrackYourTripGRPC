using TrackYourTripGrpc.Maui.ViewModels;

namespace TrackYourTripGrpc.Maui.Pages;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var shell = Shell.Current as AppShell;
        var logoutButton = shell?.FindByName<Button>("LogoutButton");

        if (logoutButton != null)
            logoutButton.IsVisible = false;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        var shell = Shell.Current as AppShell;
        var logoutButton = shell?.FindByName<Button>("LogoutButton");

        if (logoutButton != null)
            logoutButton.IsVisible = true;
    }
}