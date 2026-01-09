using Microsoft.Extensions.DependencyInjection;
using TrackYourTripGrpc.Maui.Pages;

namespace TrackYourTripGrpc.Maui
{
    public partial class App : Application
    {
        public App(TripsPage tripsPage)
        {
            InitializeComponent();
            MainPage = new NavigationPage(tripsPage);
        }

        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        //    return new Window(new AppShell());
        //}
    }
}