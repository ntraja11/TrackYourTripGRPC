using TrackYourTripGrpc.Maui.Utilities;
using TrackYourTripGrpc.Maui.ViewModels;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Maui.Pages;

public partial class TripsPage : ContentPage
{
	private readonly TripsViewModel _viewModel;
    private readonly IServiceProvider _services;

    public TripsPage(TripsViewModel viewModel, IServiceProvider services)
    {
        _viewModel = viewModel;
        InitializeComponent();
        BindingContext = _viewModel;
        _services = services;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_viewModel.IsBusy)
            await _viewModel.LoadTripsAsync();
    }

    private async void OnTripSelected(Object sender, SelectionChangedEventArgs e)
    {
        if(e.CurrentSelection.FirstOrDefault() is TripDetail selectedTrip)
        {
            var detailPage = _services.GetService<TripDetailPage>();
            detailPage!.Initialize(selectedTrip.Id);

            await Navigation.PushAsync(detailPage);
        }
        ((CollectionView)sender).SelectedItem = null;
    }

    private async void CreateTrip_ClickedEvent(object sender, EventArgs e)
    {
        var createTripPage = _services.GetService<TripUpsertPage>();
        await Navigation.PushAsync(createTripPage);
    }
}