using TrackYourTripGrpc.Maui.ViewModels;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Maui.Pages;

public partial class TripUpsertPage : ContentPage
{
	private readonly TripUpsertViewModel _viewModel;
    public TripUpsertPage(TripUpsertViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public async void InitializeForEdit(TripDetail trip)
    {
        await _viewModel.InitializeAsync(trip);
    }

    private async void SaveTrip_ClickedEvent(object sender, EventArgs e)
    {
        await _viewModel.SaveTripAsync();
        await Navigation.PopAsync();
    }
}