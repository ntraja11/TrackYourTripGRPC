using TrackYourTripGrpc.Maui.ViewModels;

namespace TrackYourTripGrpc.Maui.Pages.Member;

public partial class MembersPage : ContentPage
{
    private readonly MembersViewModel _viewModel;
    private int _tripId;

    public MembersPage(MembersViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        InitializeComponent();
    }

    public async Task Initialize(int tripId)
    {
        _tripId = tripId;
        await _viewModel.InitializeAsync(_tripId);

    }

    private void ApplyInitialSelection()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            MembersList.SelectedItems = _viewModel.SelectedMembers
                .Cast<object>()
                .ToList();
        });
    }

    protected override async void OnAppearing()
    {
        await _viewModel.LoadMembersAsync();
        MembersList.SelectedItems = _viewModel.SelectedMembers.Cast<object>().ToList();
    }
}