using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TrackYourTripGrpc.Maui.Utilities;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPC.SharedProtos.Protos;

namespace TrackYourTripGrpc.Maui.ViewModels;

public partial class MembersViewModel : ObservableObject
{

    [ObservableProperty]
    private ObservableCollection<MemberDetail> availableMembers = new();

    [ObservableProperty]
    private ObservableCollection<MemberDetail> selectedMembers = new();

    private IEnumerable<MemberDetail> ExistingMembers = new List<MemberDetail>();
    private bool _initializingSelection;

    private int TripId;


    private readonly IMemberGrpcService _memberService;

    public MembersViewModel(IMemberGrpcService memberService)
    {
        _memberService = memberService;
    }

    public async Task InitializeAsync(int tripId)
    {
        TripId = tripId;
    }

    public async Task LoadMembersAsync(CancellationToken cancellationToken = default)
    {
        var request = new GetAllMembersByGroupRequest
        {
            GroupId = AuthViewState.GroupId ?? 0
        };
        var response = await _memberService.GetAllMembersByGroupAsync(request, cancellationToken);

        ExistingMembers = await GetExistingMembers(cancellationToken);

        AvailableMembers.Clear();
        foreach (var member in response.Members)
            AvailableMembers.Add(member);


        _initializingSelection = true;

        SelectedMembers.Clear();

        foreach (var existing in ExistingMembers)
        {
            var match = AvailableMembers.FirstOrDefault(m =>
                m.Name == existing.Name &&
                m.Email == existing.Email);

            if (match != null)
                SelectedMembers.Add(match);
        }

        _initializingSelection = false;

    }

    [RelayCommand]
    private void MembersSelectionChanged(IList<object> selectedItems)
    {
        if (_initializingSelection)
            return;

        SelectedMembers.Clear();

        if (selectedItems != null)
        {
            foreach (var item in selectedItems)
                SelectedMembers.Add((MemberDetail)item);
        }

    }

    [RelayCommand]
    public async Task SubmitMemberSelectionAsync(CancellationToken cancellationToken = default)
    {
        var selectedLookup = SelectedMembers
            .Select(m => (m.Name?.Trim().ToLower(), m.Email?.Trim().ToLower()))
            .ToHashSet();

        var existingLookup = ExistingMembers
            .Select(m => (m.Name?.Trim().ToLower(), m.Email?.Trim().ToLower()))
            .ToHashSet();

        var membersToAdd = SelectedMembers
            .Where(sel => !existingLookup.Contains((sel.Name.Trim().ToLower(), sel.Email.Trim().ToLower())))
            .ToList();

        var membersToRemove = ExistingMembers
            .Where(ex => !selectedLookup.Contains((ex.Name.Trim().ToLower(), ex.Email.Trim().ToLower())))
            .ToList();

        if (membersToAdd.Count > 0)
        {
            var createRequest = new CreateMembersRequest();

            foreach (var m in membersToAdd)
            {
                var memberDetail = new MemberDetail
                {
                    Name = m.Name,
                    Email = m.Email,
                    TripId = TripId,
                };
                createRequest.Members.Add(memberDetail);
            }

            await _memberService.CreateMembersAsync(createRequest, cancellationToken);
        }

        if (membersToRemove.Count > 0)
        {
            var deleteRequest = new DeleteMembersRequest();

            deleteRequest.MemberIds.AddRange(membersToRemove.Select(m => m.Id));

            await _memberService.DeleteMembersAsync(deleteRequest, cancellationToken);
        }

        await Shell.Current.GoToAsync("..");
    }

    private async Task<IEnumerable<MemberDetail>> GetExistingMembers(CancellationToken cancellationToken)
    {
        var getAllByTripRequest = new GetAllMembersByTripRequest { TripId = TripId };
        var existingResponse = await _memberService
            .GetAllMembersByTripAsync(getAllByTripRequest, cancellationToken);
        var existingMembers = existingResponse.Members;
        return existingMembers;
    }
}

