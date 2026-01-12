using CommunityToolkit.Mvvm.ComponentModel;
using Google.Protobuf.WellKnownTypes;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Maui.ViewModels;

public partial class TripUpsertViewModel : ObservableObject
{
    private readonly ITripGrpcService _tripService;

    [ObservableProperty]
    private TripDetail? trip;

    [ObservableProperty]
    private DateTime startDate = DateTime.Today;

    public TripUpsertViewModel(ITripGrpcService tripService)
    {
        _tripService = tripService;
        trip = new();
    }

    public async Task InitializeAsync(int tripId, CancellationToken cancellationToken)
    {
        Trip = await _tripService.GetTripAsync(tripId, cancellationToken);
    }

    
    public async Task SaveTripAsync(CancellationToken cancellationToken = default)
    {
        var normalizedDate = DateTime.SpecifyKind(StartDate.Date, DateTimeKind.Utc);
        Trip!.StartDate = normalizedDate.ToTimestamp();

        if(Trip.Id == 0)
        {
            Trip.CreatedByUserEmail = "test@mauiapp.com";
            await _tripService.CreateTripAsync(Trip!, cancellationToken);
        }
        else
        {
            await _tripService.UpdateTripAsync(Trip, cancellationToken);
        }        

        Trip = new();
    }
}
