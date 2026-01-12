using Microsoft.Extensions.DependencyInjection;
using TrackYourTripGrpc.Maui.Pages;

namespace TrackYourTripGrpc.Maui
{
    public partial class App : Application
    {
        private readonly IServiceProvider _services;
        public App(IServiceProvider services)
        {
            InitializeComponent();
            _services = services;
        }

        //public App(TripsPage tripsPage)
        //{
        //    InitializeComponent();
        //    MainPage = new NavigationPage(tripsPage);
        //}

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var rootPage = _services.GetRequiredService<TripsPage>();
            var navigationPage = new NavigationPage(rootPage);

            return new Window(navigationPage);
        }

    }
}