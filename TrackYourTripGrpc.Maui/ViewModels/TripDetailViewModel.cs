using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Maui.ViewModels;

public class TripDetailViewModel : INotifyPropertyChanged
{
    private readonly ITripGrpcService _tripService;

    private TripDetail? _trip;
    public TripDetail? Trip
    {
        get => _trip;
        set
        {
            _trip = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public TripDetailViewModel(ITripGrpcService tripService)
    {
        _tripService = tripService;
    }

    public async Task LoadTripAsync(int tripId, CancellationToken cancellationToken = default)
    {
        Trip = await _tripService.GetTripAsync(tripId, cancellationToken);
        Console.WriteLine("----------");
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

