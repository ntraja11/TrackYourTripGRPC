using TrackYourTripGrpc.Maui.ViewModels;
using TrackYourTripGRPC.SharedProtos.Protos;

namespace TrackYourTripGrpc.Maui.Pages.Expense;

public partial class ExpenseUpsertPage : ContentPage
{
    private readonly ExpenseViewModel _viewModel;

    public ExpenseUpsertPage(ExpenseViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
        BindingContext = _viewModel;
    }

    public async void InitializeForEdit(ExpenseDetail expense)
    {
        await _viewModel.InitializeAsync(expense);
    }
}