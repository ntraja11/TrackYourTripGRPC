using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TrackYourTripGrpc.Maui.Pages.Account;
using TrackYourTripGrpc.Maui.Pages.Expense;
using TrackYourTripGrpc.Maui.Pages.Member;
using TrackYourTripGrpc.Maui.Pages.Trip;
using TrackYourTripGrpc.Maui.ViewModels;
using TrackYourTripGrpc.Sdk;

namespace TrackYourTripGrpc.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiCommunityToolkit();

        builder.Services.AddTrackYourTripGrpcSdk("https://10.0.2.2:7089");

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();

        builder.Services.AddTransient<TripDetailViewModel>();
        builder.Services.AddTransient<TripsViewModel>();
        builder.Services.AddTransient<TripUpsertViewModel>();

        builder.Services.AddTransient<TripDetailPage>();
        builder.Services.AddTransient<TripsPage>();
        builder.Services.AddTransient<TripUpsertPage>();

        builder.Services.AddTransient<MembersPage>();
        builder.Services.AddSingleton<MembersViewModel>();

        builder.Services.AddTransient<ExpenseUpsertPage>();
        builder.Services.AddTransient<ExpenseViewModel>();

        builder.Services.AddTransient<AppShell>();
        //builder.Services.AddSingleton<App>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
