using TrackYourTripGrpc.Maui.Pages.Account;
using TrackYourTripGrpc.Maui.Pages.Expense;
using TrackYourTripGrpc.Maui.Pages.Member;
using TrackYourTripGrpc.Maui.Pages.Trip;
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
            Routing.RegisterRoute(nameof(ExpenseUpsertPage), typeof(ExpenseUpsertPage));
        }

        public Command LogoutCommand => new Command(async () =>
        {
            SecureStorage.Remove(AppConstants.AuthTokenKey);

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        });

    }
}
