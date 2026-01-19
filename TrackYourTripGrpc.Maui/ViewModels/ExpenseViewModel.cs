using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Google.Protobuf.WellKnownTypes;
using System.Collections.ObjectModel;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPC.SharedProtos.Protos;

namespace TrackYourTripGrpc.Maui.ViewModels;

public partial class ExpenseViewModel : ObservableObject
{
    private readonly IExpenseGrpcService _expenseService;
    private readonly IMemberGrpcService _memberService;

    public ExpenseViewModel(IExpenseGrpcService expenseService, IMemberGrpcService memberService)
    {
        _expenseService = expenseService;
        _memberService = memberService;
    }

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private decimal amount = 0;

    [ObservableProperty]
    private int memberId = 0;

    [ObservableProperty]
    private ObservableCollection<MemberDetail> tripMembers = new();

    [ObservableProperty]
    private MemberDetail? selectedMember;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowErrorMessage))]
    private string errorMessage = string.Empty;

    public bool ShowErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

    public ExpenseDetail? Expense { get; private set; }


    partial void OnSelectedMemberChanged(MemberDetail? value)
    {
        MemberId = value?.Id ?? 0;
    }

    public async Task InitializeAsync(ExpenseDetail ExpenseToUpdate, CancellationToken cancellationToken = default)
    {
        Expense = ExpenseToUpdate;

        Title = Expense.Title;
        Description = Expense.Description;
        Amount = Convert.ToDecimal(Expense.Amount);
        MemberId = Expense.MemberId;        

        var request = new GetAllMembersByTripRequest { TripId = Expense.TripId };
        var response = await _memberService.GetAllMembersByTripAsync(request, cancellationToken);

        TripMembers.Clear();
        foreach (var member in response.Members)
        {
            TripMembers.Add(member);
        }

        if (Expense.MemberId != 0)
        {
            SelectedMember = TripMembers.FirstOrDefault(m => m.Id == Expense.MemberId);
        }

    }

    [RelayCommand]
    public async Task SaveExpenseAsync(CancellationToken cancellationToken = default)
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Title))
        {
            ErrorMessage = "Title is required.";
            return;
        }

        if (Amount < 0 || Amount > 999999)
        {
            ErrorMessage = "Please enter an amount between '1' and '100000'.";
            return;
        }

        if (SelectedMember == null)
        {
            ErrorMessage = "Please selecte a member.";
            return;
        }


        var normalizedDate = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);

        var expenseDetail = new ExpenseDetail
        {
            Title = Title,
            Description = Description,
            Amount = Convert.ToDouble(Amount),
            TripId = Expense!.TripId,
            MemberId = MemberId,
            ExpenseDate = normalizedDate.ToTimestamp(),
        };


        if (Expense.Id == 0)
        {
            await _expenseService!.CreateExpenseAsync(expenseDetail!, cancellationToken);
            //await _expenseService!.CreateExpenseAsync(Expense, cancellationToken);
        }
        else
        {
            expenseDetail.Id = Expense.Id;
            await _expenseService!.UpdateExpenseAsync(expenseDetail, cancellationToken);
        }

        Expense = new();


        await Shell.Current.GoToAsync("..");
    }
}
