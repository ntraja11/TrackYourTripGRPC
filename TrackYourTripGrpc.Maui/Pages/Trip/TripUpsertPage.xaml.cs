using TrackYourTripGrpc.Maui.ViewModels;
using TrackYourTripGRPC.SharedProtos.Protos;

namespace TrackYourTripGrpc.Maui.Pages.Trip;

public partial class TripUpsertPage : ContentPage
{
    private readonly TripUpsertViewModel _viewModel;
    public TripUpsertPage(TripUpsertViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        InitializeComponent();
    }

    public async void InitializeForEdit(TripDetail trip)
    {
        await _viewModel.InitializeAsync(trip);
    }

}