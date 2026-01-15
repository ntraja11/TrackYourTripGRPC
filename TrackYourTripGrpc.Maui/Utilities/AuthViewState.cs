
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TrackYourTripGrpc.Maui.Utilities;

public static class AuthViewState
{
    public static string? UserName { get; private set; }
    public static string? UserEmail { get; private set; }
    public static int? GroupId { get; private set; }
    public static string? UserId { get; private set; }

    public static async Task DecodeAndStoreClaimsAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
            return;

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        UserName = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        UserEmail = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        GroupId = Convert.ToInt32(jwt.Claims.FirstOrDefault(c => c.Type == "groupid")?.Value);
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