using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPC.SharedProtos.Protos;

namespace TrackYourTripGrpc.Maui.ViewModels;

public partial class TripDetailViewModel : ObservableObject
{
    private readonly ITripGrpcService _tripService;
    private readonly IMemberGrpcService _memberService;
    private readonly IExpenseGrpcService _expenseService;
    [ObservableProperty]
    private TripDetail? trip;

    [ObservableProperty]
    private ObservableCollection<MemberDetail> tripMembers = new();

    [ObservableProperty]
    private ObservableCollection<ExpenseDetail> tripExpenses = new();

    [ObservableProperty]
    public bool hasExpenses;
    [ObservableProperty]
    public bool hasMembers;

    public TripDetailViewModel(ITripGrpcService tripService, IMemberGrpcService memberService,
        IExpenseGrpcService expenseService)
    {
        _tripService = tripService;
        _memberService = memberService;
        _expenseService = expenseService;
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

        var expenses = await _expenseService.GetAllExpensesByTripAsync(tripId, cancellationToken);

        TripExpenses.Clear();
        foreach (var expense in expenses)
            TripExpenses.Add(expense);

        HasMembers = TripMembers.Any() == true;
        HasExpenses = TripExpenses.Any() == true;
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

