using TrackYourTripGrpc.Maui.Pages;

namespace TrackYourTripGrpc.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = this;

            Routing.RegisterRoute(nameof(TripDetailPage), typeof(TripDetailPage));
            Routing.RegisterRoute(nameof(TripUpsertPage), typeof(TripUpsertPage));
        }

        public Command LogoutCommand => new Command(async () =>
        {
            SecureStorage.Remove(AppConstants.AuthTokenKey);

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        });

    }
}
