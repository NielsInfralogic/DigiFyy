using DigiFyy.Models.AWS;
using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    public class ShowPositionViewModel : ViewModelBase
    {


        FrameNumberStatus _frameNumberStatus = null;
        public FrameNumberStatus FrameNumberStatus
        {
            get => _frameNumberStatus;
            set
            {
                SetProperty(ref _frameNumberStatus, value);
            }
        }


        public ShowPositionViewModel()
        {
            this.UpdatePostionCommand = new Helpers.Command(this.UpdatePostionClicked);  
            this.BackButtonCommand = new Helpers.Command(this.BackButtonClicked);
        }

        /* public override void Init(object param)
         {
             if (param == null)
                 return;
             if (param is Models.AWS.FrameNumberStatus)
                 FrameNumberStatus = param as Models.AWS.FrameNumberStatus;
         }*/

        public override async Task InitializeAsync(object navigationData)
        {

            if (navigationData == null)
                return;
            if (navigationData is Models.AWS.FrameNumberStatus)
            {
                FrameNumberStatus = navigationData as Models.AWS.FrameNumberStatus;
            }

            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    if (FrameNumberStatus != null)
                    {
                        FrameNumberStatus.Latitude = location.Latitude;
                        FrameNumberStatus.Longitude = location.Longitude;
                    }
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DialogService.Show("Feature not supported", "Positioning is not supported", "Ok");
                Console.WriteLine($"Exception in UpdatePostionClicked() - {fnsEx.Message}");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DialogService.Show("Feature not enabled", "Positioning is not enabled", "Ok");
                Console.WriteLine($"Exception in UpdatePostionClicked() - {fneEx.Message}");
            }
            catch (PermissionException pEx)
            {
                await DialogService.Show("Feature not permitted", "Positioning is not currently permitted", "Ok");
                Console.WriteLine($"Exception in UpdatePostionClicked() - {pEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in UpdatePostionClicked() - {ex.Message}");
            }
        }



        public Helpers.Command UpdatePostionCommand { get; set; }
        public Helpers.Command BackButtonCommand { get; set; }

        private async void UpdatePostionClicked(object obj)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    FrameNumberStatus updFrameNumberStatus = new FrameNumberStatus()
                    {
                        LastUpdateClientID = FrameNumberStatus.LastUpdateClientID,
                        Status = FrameNumberStatus.Status,
                        LastUpdateTime = DateTime.Now,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    };
                    string uuid = Preferences.Get("UUID", "");
                    string user = Preferences.Get("Email", "");
                    string token = Preferences.Get("Token", "");
                    if (uuid != "")
                    {
                        bool res = await DataStore.UpdateStatus(user, token, uuid, updFrameNumberStatus);
                        if (res)
                        {
                            FrameNumberStatus = updFrameNumberStatus;

                            // Notify about new state.
                            var bikeDetailViewModel = ViewModelLocator.Resolve<BikeDetailViewModel>();
                            if (bikeDetailViewModel != null)
                                MessagingCenter.Send(bikeDetailViewModel, MessageKeys.UpdateState, 1);
                            await NavigationService.GoBackAsync();
                        }
                    }
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DialogService.Show("Feature not supported", "Positioning is not supported", "Ok");
                Console.WriteLine($"Exception in UpdatePostionClicked() - {fnsEx.Message}");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DialogService.Show("Feature not enabled", "Positioning is not enabled", "Ok");
                Console.WriteLine($"Exception in UpdatePostionClicked() - {fneEx.Message}");
            }
            catch (PermissionException pEx)
            {
                await DialogService.Show("Feature not permitted", "Positioning is not currently permitted", "Ok");
                Console.WriteLine($"Exception in UpdatePostionClicked() - {pEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in UpdatePostionClicked() - {ex.Message}");
            }

        }


        private async void BackButtonClicked(object obj)
        {
            var bikeDetailViewModel = ViewModelLocator.Resolve<BikeDetailViewModel>();
            if (bikeDetailViewModel != null)
                MessagingCenter.Send(bikeDetailViewModel, MessageKeys.UpdateState, 1);
            await NavigationService.GoBackAsync();
        }


    }
}
