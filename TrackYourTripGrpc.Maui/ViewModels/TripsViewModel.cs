using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPC.SharedProtos.Protos;

namespace TrackYourTripGrpc.Maui.ViewModels;

public partial class TripsViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TripDetail> trips = new();

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private bool isBusy;

    private readonly ITripGrpcService _tripService;


    public TripsViewModel(ITripGrpcService tripService)
    {
        _tripService = tripService;
        RefreshCommand = new AsyncRelayCommand(RefreshAsync);
    }

    public IAsyncRelayCommand RefreshCommand { get; }

    private async Task RefreshAsync()
    {
        try
        {
            IsRefreshing = true;
            await LoadTripsAsync();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    public async Task LoadTripsAsync(CancellationToken cancellationToken = default)
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var tripList = await _tripService.GetAllTripsAsync(cancellationToken);

            Trips.Clear();
            foreach (var trip in tripList)
                Trips.Add(trip);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
