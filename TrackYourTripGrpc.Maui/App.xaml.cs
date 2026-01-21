namespace TrackYourTripGrpc.Maui;

public partial class App : Application
{
    private readonly IServiceProvider _services;
    public App(IServiceProvider services)
    {
        InitializeComponent();

        var theme = Preferences.Get("AppTheme", "Light");
        UserAppTheme = Enum.Parse<AppTheme>(theme);

        _services = services;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_services.GetRequiredService<AppShell>());
    }

}