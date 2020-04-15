using DigiFyy.Helpers;
using DigiFyy.Models.AWS;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    public class DeregisterViewModel : ViewModelBase
    {
        private string _deregisterToken;
        public string DeregisterToken
        {
            get
            {
                return _deregisterToken;
            }
            set
            {
                SetProperty(ref _deregisterToken, value);
            }
        }

        private bool _stage1 = true;
        public bool Stage1
        {
            get
            {
                return _stage1;
            }
            set
            {
                SetProperty(ref _stage1, value);
            }
        }

        private bool _stage2 = false;
        public bool Stage2
        {
            get
            {
                return _stage2;
            }
            set
            {
                SetProperty(ref _stage2, value);
            }
        }

        private string _sellText;
        public string SellText
        {
            get
            {
                return _sellText;
            }
            set
            {
                SetProperty(ref _sellText, value);
            }
        }

        public Helpers.Command DeregisterBikeCommand { get; set; }
        
        public Helpers.Command BackButtonCommand { get; set; }

        public DeregisterViewModel()
        {
            Stage1 = true;
            Stage2 = false;
            

            this.DeregisterBikeCommand = new Helpers.Command(this.DeregisterBikeClicked);
            this.BackButtonCommand = new Helpers.Command(this.BackButtonClicked);
        }

        private async void BackButtonClicked(object obj)
        {
            // Update state
            var bikeDetailViewModel = ViewModelLocator.Resolve<BikeDetailViewModel>();

            if (bikeDetailViewModel != null)
                MessagingCenter.Send(bikeDetailViewModel, MessageKeys.UpdateState, 1);
            await NavigationService.GoBackAsync();
        }



        public override async Task InitializeAsync(object navigationData)
        {           
            var resmgr = new ResourceManager("DigiFyy.Resources.AppResources", typeof(TranslateExtension).GetTypeInfo().Assembly);
            var ci = CrossMultilingual.Current.CurrentCultureInfo;

            if (Stage1)
                SellText = resmgr.GetString("ToSellYourBike", ci);
            else
                SellText = resmgr.GetString("YourBikeHasBeenDeregistered", ci);
        }



        private async void DeregisterBikeClicked(object obj)
        {

            FrameNumberStatus fns = new FrameNumberStatus()
            {
                LastUpdateTime = DateTime.Now,
                Latitude = 0,
                Longitude = 0,
                LastUpdateClientID = DeviceInfo.Name,
                Status = (int)FrameNumberStatusType.NotRegistered
            };
            IsBusy = true;
            string uuid = Preferences.Get("UUID", "");
            string user = Preferences.Get("Email", "");
            string token = Preferences.Get("Token", "");


            if (await DataStore.UpdateStatus(user, token, uuid, fns) == true)
            {

                DeregisterToken = "ANH123YX";
                Helpers.EmailSender emailSender = new EmailSender();
                await emailSender.SendEmail("DIGIFYY Unregistration token", "Token = " + DeregisterToken, new List<string> { user });
                Stage1 = false;
                Stage2 = true;
                IsBusy = false;
                var resmgr = new ResourceManager("DigiFyy.Resources.AppResources", typeof(TranslateExtension).GetTypeInfo().Assembly);
                var ci = CrossMultilingual.Current.CurrentCultureInfo;
                SellText = resmgr.GetString("YourBikeHasBeenDeregistered", ci);
            }
        }


    }
}