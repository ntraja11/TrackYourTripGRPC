using TrackYourTripGrpc.Maui.ViewModels;
using TrackYourTripGrpc.Sdk.Services;

namespace TrackYourTripGrpc.Maui.Pages;

public partial class TripDetailPage : ContentPage
{
    private readonly TripDetailViewModel _viewModel;
    private readonly IServiceProvider _services;
    private int _tripid;

    private CancellationTokenSource? _cts;
    public TripDetailPage(TripDetailViewModel viewModel, IServiceProvider services)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        _services = services;
    }

    public void Initialize(int tripId)
    {
        _tripid =  tripId;
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

    private async void UpdateTrip_ClickedEvent(Object sender, EventArgs e)
    {
        //var tripUpsertPage = _services.GetRequiredService<TripUpsertPage>();
        var tripUpsertPage = ActivatorUtilities.CreateInstance<TripUpsertPage>(_services);
        tripUpsertPage.InitializeForEdit(_viewModel.Trip!);

        await Navigation.PushAsync(tripUpsertPage);
    }
}