using DigiFyy.DataService;
using DigiFyy.Helpers;
using DigiFyy.Models;

using DigiFyy.Services;
using DigiFyy.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    public class ScanViewModel : ViewModelBase
    {
        string nfcImage = string.Empty;
        public string NfcImage
        {
            get { return nfcImage; }
            set { SetProperty(ref nfcImage, value); }
        }

        private string message = string.Empty;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        private string serialNumber = string.Empty;
        public string SerialNumber
        {
            get { return serialNumber; }
            set { SetProperty(ref serialNumber, value); }
        }


        private string nFCModel = string.Empty;
        public string NFCModel
        {
            get { return nFCModel; }
            set { SetProperty(ref nFCModel, value); }
        }
        

        private bool useScanEnabled = false;
        public bool UseScanEnabled
        {
            get { return useScanEnabled; }
            set { SetProperty(ref useScanEnabled, value); }
        }
        private bool scanAgainEnabled = false;
        public bool ScanAgainEnabled
        {
            get { return scanAgainEnabled; }
            set { SetProperty(ref scanAgainEnabled, value); }
        }

        private bool waitingForScanVisible = false;
        public bool WaitingForScanVisible
        {
            get { return waitingForScanVisible; }
            set { SetProperty(ref waitingForScanVisible, value); }
        }

        


        public ScanViewModel()
        {
            this.LoginCommand = new Helpers.Command(this.ToLoginPage);
            this.UseCommand = new Helpers.Command(this.ToDetailsPage);
            this.ScanAgainCommand = new Helpers.Command(this.ScanAgain);
            this.OnAppearingCommand = new Helpers.Command(this.OnAppearing);
            this.OnDisappearingCommand = new Helpers.Command(this.OnDisappearing);
            //   NfcImage = "NFC.png";
            NfcImage = "ScanBike.png";
        }

        public string TagId { get; set; }

        private List<string> _arg;
        public string ReceivedAt { get; set; }



        private  void OnAppearing()
        {
            
        }

        private  void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<App, List<string>>(this, "Tag");
        }

        public override async Task InitializeAsync(object navigationData)
        {
            WaitingForScanVisible = true;
            UseScanEnabled = false;
            ScanAgainEnabled = false;
            Message = "";
            NFCModel = "";
            SerialNumber = "";

            if (Preferences.Get("UseFakeUUID", "0") == "1")
            {
                UseScanEnabled = true;
                Message = Constants.FakeUUID;
            }

            MessagingCenter.Unsubscribe<App, Models.NFC.TagRead>(this, "Tag");

            MessagingCenter.Subscribe<App, Models.NFC.TagRead>(this, "Tag", (sender, arg) =>
            {             
                OnScanReceived(arg);
            });

            // IOS only - star session here - it will pop-up iOS build-in scan dialog
            if (Device.RuntimePlatform == Device.iOS)
                DependencyService.Get<INfc>().StartSession();
            
        }

        public Helpers.Command OnAppearingCommand { get; protected set; }
        public Helpers.Command OnDisappearingCommand { get; protected set; }
        public Helpers.Command LoginCommand { get; set; }
        public Helpers.Command UseCommand { get; set; }
        public Helpers.Command ScanAgainCommand { get; set; }
        

        private async void ToLoginPage()
        {
            MessagingCenter.Unsubscribe<App, Models.NFC.TagRead>(this, "Tag");

            await NavigationService.NavigateToAsync<LoginViewModel>(new LogoutParameter { Logout = true });
            await NavigationService.RemoveBackStackAsync();
        }

        private async void ScanAgain()
        {
            Message = "";
            NFCModel = "";
            SerialNumber = "";

            WaitingForScanVisible = true;
            ScanAgainEnabled = false;
            UseScanEnabled = false;

            // IOS only - star session here - it will pop-up iOS build-in scan dialog
            if (Device.RuntimePlatform == Device.iOS)
                DependencyService.Get<INfc>().StartSession();
        }

        private async void ToDetailsPage()
        {
            MessagingCenter.Unsubscribe<App, Models.NFC.TagRead>(this, "Tag");
            if (Message != "" && Message != "Empty" && Message != "(Empty)")
                Preferences.Set("UUID", Message);
            else
                Preferences.Set("UUID", SerialNumber);

            if (Preferences.Get("UseFakeUUID", "0") == "1" && DataService.Constants.FakeUUID != "")
                Preferences.Set("UUID", DataService.Constants.FakeUUID);

            // Send to details page regardless of registreation/login state..
//            TabParameter tabParameter = new TabParameter() { TabIndex = 2 };
  //          await NavigationService.NavigateToAsync<MainViewModel>(tabParameter);

            var mainViewModel = ViewModelLocator.Resolve<MainViewModel>();
            if (mainViewModel != null)
                MessagingCenter.Send(mainViewModel, MessageKeys.ChangeTab, 2);

        }


        private void OnScanReceived(Models.NFC.TagRead tagRead)
        {
            if (tagRead == null)
                return;

            if (string.IsNullOrEmpty(tagRead.ErrorMessage) == false)
            {
                DialogService.Show("NFC error", tagRead.ErrorMessage, "Ok");
                return;
            }

            SerialNumber = string.IsNullOrEmpty(tagRead.SerialNumber) ? "(Unknown)" : tagRead.SerialNumber;
            NFCModel = string.IsNullOrEmpty(tagRead.Model) ? "(Unknown)" : tagRead.Model;
            Message = string.IsNullOrEmpty(tagRead.Data) ? "(Empty)" : tagRead.Data.Trim();

            UseScanEnabled = true;
            ScanAgainEnabled = true;
            WaitingForScanVisible = false;



        }
    }
}
