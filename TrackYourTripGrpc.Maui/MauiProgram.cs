using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TrackYourTripGrpc.Maui.Pages;
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
        
        builder.Services.AddTransient<TripDetailViewModel>();
        builder.Services.AddTransient<TripsViewModel>();
        
        builder.Services.AddTransient<TripDetailPage>();
        builder.Services.AddTransient<TripsPage>();

        builder.Services.AddSingleton<App>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
