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

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var http = new HttpClient();
                await http.GetAsync(AppConstants.ApiUrl + "/api/status");
            }
            catch
            {
                // swallow errors silently — it's just a warm-up
            }
        }


        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            SecureStorage.Remove(AppConstants.AuthTokenKey);

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void ToggleTheme_Clicked(object sender, EventArgs e)
        {
            Application.Current.UserAppTheme =
                    Application.Current.UserAppTheme == AppTheme.Dark
                        ? AppTheme.Light
                        : AppTheme.Dark;

            Preferences.Set("AppTheme", Application.Current.UserAppTheme.ToString());
        }
    }
}
