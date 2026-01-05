using Microsoft.Extensions.DependencyInjection;
using TrackYourTripGrpc.Maui.Pages;

namespace TrackYourTripGrpc.Maui
{
    public partial class App : Application
    {
        public App(TripDetailPage tripDetailPage)
        {
            InitializeComponent();
            MainPage = new NavigationPage(tripDetailPage);
        }

        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        //    return new Window(new AppShell());
        //}
    }
}