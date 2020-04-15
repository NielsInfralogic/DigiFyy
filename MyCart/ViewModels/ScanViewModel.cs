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

        string message = string.Empty;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        string serialNumber = string.Empty;
        public string SerialNumber
        {
            get { return serialNumber; }
            set { SetProperty(ref serialNumber, value); }
        }

        

        public ScanViewModel()
        {
            this.LoginCommand = new Helpers.Command(this.ToLoginPage);
            this.UseCommand = new Helpers.Command(this.ToDetailsPage);
            //   NfcImage = "NFC.png";
            NfcImage = "ScanBike.png";

        }

        public override async Task InitializeAsync(object navigationData)
        {
            
        }


        public ICommand LoginCommand { get; set; }
        public ICommand UseCommand { get; set; }

        private async void ToLoginPage()
        {
            await NavigationService.NavigateToAsync<LoginViewModel>(new LogoutParameter { Logout = true });
            await NavigationService.RemoveBackStackAsync();
        }

        private async void ToDetailsPage()
        {
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
    }
}
