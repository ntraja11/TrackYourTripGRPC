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

    public void InitializeForEdit(TripDetail trip)
    {
        _viewModel.Trip = trip;
        _viewModel.StartDate = trip.StartDate.ToDateTime();
    }

    private async void SaveTrip_ClickedEvent(object sender, EventArgs e)
    {
        await _viewModel.SaveTripAsync();
        await Task.Delay(2000);
        await Navigation.PopAsync();
    }
}