using TrackYourTripGrpc.Maui.ViewModels;

namespace TrackYourTripGrpc.Maui.Pages;

public partial class TripDetailPage : ContentPage
{
    private readonly TripDetailViewModel _viewModel;

    public TripDetailPage(TripDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // For testing, hardcode a tripId that exists
        int testTripId = 14;

        await _viewModel.LoadTripAsync(testTripId);
    }

}