using DigiFyy.Helpers;
using DigiFyy.Services;
using DigiFyy.ViewModels;
using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DigiFyy.ViewModels
{
    public class ScanViewModel : ViewModelBase
    {


        string message = string.Empty;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        public ScanViewModel(INavigationService navigationService, IAnalyticsService analyticsService) :  base(navigationService, analyticsService)
        {
            this.LoginCommand = new Command(this.ToLoginPage);
            this.UseCommand = new Command(this.ToDetailsPage);
        }

       
        public ICommand LoginCommand { get; set; }
        public ICommand UseCommand { get; set; }

        private void ToLoginPage()
        {
            NavigationService.NavigateTo(typeof(LoginPageViewModel), string.Empty, string.Empty);
        }

        private void ToDetailsPage()
        {
            NavigationService.NavigateTo(typeof(BikeDetailViewModel), Message, string.Empty);
        }
    }
}
