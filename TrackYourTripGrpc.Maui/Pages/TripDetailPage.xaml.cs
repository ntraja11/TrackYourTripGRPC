using TrackYourTripGrpc.Maui.ViewModels;

namespace TrackYourTripGrpc.Maui.Pages;

public partial class TripDetailPage : ContentPage
{
    private readonly TripDetailViewModel _viewModel;
    private readonly int _tripid;
    private CancellationTokenSource? _cts;
    public TripDetailPage(int tripId, TripDetailViewModel viewModel)
    {
        InitializeComponent();
        _tripid = tripId;
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _cts = new CancellationTokenSource();

        await _viewModel.InitializeAsync(_tripid, _cts.Token);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _cts?.Cancel();
        _cts?.Dispose();
    }
}