using Android.App;
using TrackYourTripGrpc.Maui.ViewModels;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Maui.Pages;

public partial class TripsPage : ContentPage
{
	private readonly TripsViewModel _viewModel;
    private readonly TripDetailViewModel _tripDetailViewModel;
    
    public TripsPage(TripsViewModel viewModel, TripDetailViewModel vm)
	{
		_viewModel = viewModel;		
        _tripDetailViewModel = vm;
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if(!_viewModel.IsBusy)
            await _viewModel.LoadTripsAsync();
    }

    private async void OnTripSelected(Object sender, SelectionChangedEventArgs e)
    {
        if(e.CurrentSelection.FirstOrDefault() is TripDetail selectedTrip)
        {
            await Navigation.PushAsync(new TripDetailPage(selectedTrip.Id, _tripDetailViewModel));            
        }
        ((CollectionView)sender).SelectedItem = null;
    }
}