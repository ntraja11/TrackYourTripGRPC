
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TrackYourTripGrpc.Maui.Utilities;

public static class AuthViewState
{
    public static string? UserName { get; private set; }
    public static string? Email { get; private set; }
    public static string? GroupId { get; private set; }
    public static string? UserId { get; private set; }

    public static async Task DecodeAndStoreClaimsAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
            return;

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        UserName = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        Email = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        GroupId = jwt.Claims.FirstOrDefault(c => c.Type == "groupid")?.Value;
        UserId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }



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