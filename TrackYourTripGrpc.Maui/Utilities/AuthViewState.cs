namespace TrackYourTripGrpc.Maui.Utilities;

public static class AuthViewState
{
    public static async Task<string> GetTokenAsync()
        => await SecureStorage.GetAsync(AppConstants.AuthTokenKey);

    public static async Task<bool> IsLoggedInAsync()
        => !string.IsNullOrEmpty(await GetTokenAsync());

    public static void ToggleLogoutButton(bool isVisible)
    {
        var shell = Shell.Current as AppShell;
        if (shell == null)
            return;

        var logoutButton = shell.FindByName<Button>("LogoutButton");
        if (logoutButton != null)
            logoutButton.IsVisible = isVisible;
    }


}