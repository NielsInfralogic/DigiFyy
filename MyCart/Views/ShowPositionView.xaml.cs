using DigiFyy.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace DigiFyy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowPositionView : ContentPage
    {
        ShowPositionViewModel ViewModel => BindingContext as ShowPositionViewModel;

        public ShowPositionView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel != null)
            {
                UpdateMap();
                ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (ViewModel != null)
            {
                ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }
        }

        void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(ShowPositionViewModel.FrameNumberStatus))
            {
               UpdateMap();
            }
        }

        /*      async Task UpdateMap()
              {
                  if (ViewModel.Status != null)
                  {

                      var location = new Location(ViewModel.Status.Latitude, ViewModel.Status.Longitude);
                      var options = new MapLaunchOptions { Name = "My bike!" };
                      await Map.OpenAsync(location, options);
                  }

              }*/

        void UpdateMap()
        {
            if (ViewModel.FrameNumberStatus == null)
                return;

           

            // Center the map around the log entry's location
            map.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(ViewModel.FrameNumberStatus.Latitude, ViewModel.FrameNumberStatus.Longitude),
                Distance.FromMiles(.5)));

            // Place a pin on the map for the log entry's location

            map.Pins.Add(new Pin
            {
                Type = PinType.Place,
                Label = "My bike!", //ViewModel.Status..Title,                
                Position = new Position(ViewModel.FrameNumberStatus.Latitude, ViewModel.FrameNumberStatus.Longitude), 
            });
        }

    }
}