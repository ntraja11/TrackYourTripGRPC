using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Google.Protobuf.WellKnownTypes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrackYourTripGrpc.Maui.Utilities;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPC.SharedProtos.Protos;

namespace TrackYourTripGrpc.Maui.ViewModels;

public partial class TripUpsertViewModel : ObservableValidator
{
    private readonly ITripGrpcService? _tripService;

    public TripUpsertViewModel(ITripGrpcService tripService)
    {
        _tripService = tripService;
        ValidateAllProperties();
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    private string title = "";

    [ObservableProperty]
    private string description = "";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Please provide your starting place.")]
    private string from = "";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Please provide your destination.")]
    private string to = "";

    [ObservableProperty]
    private DateTime startDate = DateTime.Today;

    [ObservableProperty]
    private DateTime endDate;

    public TripDetail? Trip { get; private set; }

    public bool IsValid => !HasErrors;


    public async Task InitializeAsync(TripDetail tripToUpdate, CancellationToken cancellationToken = default)
    {
        Trip = tripToUpdate;

        Title = Trip.Title;
        Description = Trip.Description;
        From = Trip.From;
        To = Trip.To;
        StartDate = Trip.StartDate.ToDateTime();
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName != nameof(IsValid))
            OnPropertyChanged(nameof(IsValid));
    }

    [RelayCommand]
    public async Task SaveTripAsync(CancellationToken cancellationToken = default)
    {
        if (HasErrors)
            return;


        var normalizedStartDate = DateTime.SpecifyKind(StartDate.Date, DateTimeKind.Utc);

        var tripDetail = new TripDetail
        {
            Title = Title,
            Description = Description,
            From = From,
            To = To,
            StartDate = normalizedStartDate.ToTimestamp(),
            EndDate = normalizedStartDate.AddDays(7).ToTimestamp(),
        };


        if (Trip is null)
        {
            tripDetail.CreatedByUserEmail = AuthViewState.UserEmail;
            tripDetail.GroupId = AuthViewState.GroupId ?? 0;
            await _tripService!.CreateTripAsync(tripDetail!, cancellationToken);
        }
        else
        {
            tripDetail.Id = Trip.Id;
            await _tripService!.UpdateTripAsync(tripDetail, cancellationToken);
        }

        Trip = new();

        await Shell.Current.GoToAsync("..");
    }
}
