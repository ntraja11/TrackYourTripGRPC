using CommunityToolkit.Mvvm.ComponentModel;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Maui.ViewModels;

public partial class TripDetailViewModel : ObservableObject
{
    private readonly ITripGrpcService _tripService;

    [ObservableProperty]
    private TripDetail? trip;
    
    public TripDetailViewModel(ITripGrpcService tripService)
    {
        _tripService = tripService;
    }

    public async Task InitializeAsync(int tripId, CancellationToken cancellationToken)
    {
        var tripFromServer = await _tripService.GetTripAsync(tripId, cancellationToken);

        Trip = new TripDetail
        {
            Id = tripFromServer.Id,
            Title = tripFromServer.Title,
            Description = tripFromServer.Description,
            StartDate = tripFromServer.StartDate,
            EndDate = tripFromServer.EndDate,
            Notes = tripFromServer.Notes,
            CreatedByUserEmail = tripFromServer.CreatedByUserEmail,
            From = tripFromServer.From,
            To = tripFromServer.To
        };

    }
}

