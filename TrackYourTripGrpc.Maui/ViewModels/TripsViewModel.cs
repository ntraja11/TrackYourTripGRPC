using CommunityToolkit.Mvvm.ComponentModel;
using Google.Protobuf.WellKnownTypes;
using System.Collections.ObjectModel;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Maui.ViewModels;

public partial class TripsViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TripDetail> trips = new ();

    [ObservableProperty]
    private bool isBusy;

    private readonly ITripGrpcService _tripService;


    public TripsViewModel(ITripGrpcService tripService)
    {
        _tripService = tripService;
    }

    public async Task LoadTripsAsync(CancellationToken cancellationToken = default)
    {
        if (IsBusy)
             return;

        try
        {
            IsBusy = true;
            var tripsList = await _tripService.GetAllTripsAsync(cancellationToken);
            Trips = new ObservableCollection<TripDetail>(tripsList);

        }
        finally
        {
            IsBusy = false;
        }        
    }
}
