using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Maui.ViewModels;

public partial class TripDetailViewModel : ObservableObject
{
    private readonly ITripGrpcService _tripService;
    private readonly IMemberGrpcService _memberService;
    [ObservableProperty]
    private TripDetail? trip;

    [ObservableProperty]
    private ObservableCollection<MemberDetail> tripMembers = new();
    
    public TripDetailViewModel(ITripGrpcService tripService, IMemberGrpcService memberService)
    {
        _tripService = tripService;
        _memberService = memberService;
    }

    public async Task InitializeAsync(int tripId, CancellationToken cancellationToken)
    {
        Trip = null;
        Trip = await _tripService.GetTripAsync(tripId, cancellationToken);
        var request = new GetAllMembersByTripRequest { TripId = tripId };
        var response = await _memberService.GetAllMembersByTripAsync(request, cancellationToken);

        TripMembers.Clear();
        foreach (var member in response.Members)
            TripMembers.Add(member);
    }

    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        if (Trip is not null)
        {
            await _tripService.DeleteTripAsync(Trip.Id, cancellationToken);
            Trip = null;
        }
    }
}

