using TrackYourTripGrpc.Maui.Pages;

namespace TrackYourTripGrpc.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(TripDetailPage), typeof(TripDetailPage));
            Routing.RegisterRoute(nameof(TripUpsertPage), typeof(TripUpsertPage));
        }
    }
}
