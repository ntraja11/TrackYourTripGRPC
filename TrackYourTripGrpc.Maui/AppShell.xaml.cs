using TrackYourTripGrpc.Maui.Pages.Trip;
using TrackYourTripGrpc.Maui.Pages.Account;
using TrackYourTripGrpc.Maui.Pages.Member;
using TrackYourTripGrpc.Maui.Utilities;

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
            Routing.RegisterRoute(nameof(MembersPage), typeof(MembersPage));
        }

        public Command LogoutCommand => new Command(async () =>
        {
            SecureStorage.Remove(AppConstants.AuthTokenKey);

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        });

    }
}
