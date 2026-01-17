using TrackYourTripGrpc.Maui.Pages.Member;
using TrackYourTripGrpc.Maui.ViewModels;
using TrackYourTripGrpc.Sdk.Services;

namespace TrackYourTripGrpc.Maui.Pages.Trip;

public partial class TripDetailPage : ContentPage
{
    private readonly TripDetailViewModel _viewModel;
    private readonly IServiceProvider _services;
    private int _tripId;

    private CancellationTokenSource? _cts;

    public TripDetailPage(TripDetailViewModel viewModel, IServiceProvider services)
    {        
        _viewModel = viewModel;
        _services = services;
        BindingContext = _viewModel;
        InitializeComponent();
    }

    public void Initialize(int tripId)
    {
        _tripId = tripId;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _cts = new CancellationTokenSource();

        await _viewModel.InitializeAsync(_tripId, _cts.Token);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _cts?.Cancel();
        _cts?.Dispose();
    }

    private async void UpdateTrip_ClickedEvent(Object sender, EventArgs e)
    {
        var tripUpsertPage = _services.GetRequiredService<TripUpsertPage>();
        tripUpsertPage.InitializeForEdit(_viewModel.Trip!);

        await Navigation.PushAsync(tripUpsertPage);
    }

    private async void DeleteTrip_ClickedEvent(object sender, EventArgs e)
    {
        var delete = await DisplayAlertAsync("Delete Trip", "Do you really want to delete this trip?", "Yes", "No");

        if (delete)
        {
            await _viewModel.DeleteAsync();
            await Navigation.PopAsync();
        }
    }

    private async void ManageMembers_ClickedEvent(object sender, EventArgs e)
    {
        var membersPage = _services.GetRequiredService<MembersPage>();
        await membersPage.Initialize(_tripId);
        await Navigation.PushAsync(membersPage);
        
    }
}