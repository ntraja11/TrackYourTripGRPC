using CommunityToolkit.Mvvm.ComponentModel;
using TrackYourTripGRPC.SharedProtos.Protos;

namespace TrackYourTripGrpc.Maui.ViewModels;

public partial class MemberDetailViewModel : ObservableObject
{
    public MemberDetail MemberDetail { get; }

    public MemberDetailViewModel(MemberDetail memberDetail)
    {
        MemberDetail = memberDetail;
    }

    public int Id => MemberDetail.Id;
    public string Name => MemberDetail.Name;

    private double _totalTripExpense;
    public double TotalTripExpense
    {
        get => _totalTripExpense;
        set
        {
            if (SetProperty(ref _totalTripExpense, value))
                RaiseComputedProperties();
        }
    }

    private double _singleMemberShare;
    public double SingleMemberShare
    {
        get => _singleMemberShare;
        set
        {
            if (SetProperty(ref _singleMemberShare, value))
                RaiseComputedProperties();
        }
    }

    public double ShareDifference => TotalTripExpense - SingleMemberShare;
    public bool IsPositive => ShareDifference > 0;
    public bool IsNegative => ShareDifference < 0;    

    private void RaiseComputedProperties()
    {
        OnPropertyChanged(nameof(ShareDifference));
        OnPropertyChanged(nameof(IsPositive));
        OnPropertyChanged(nameof(IsNegative));
    }

}
