using DigiFyy.Helpers;
using DigiFyy.Services;
using DigiFyy.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DigiFyy.ViewModels
{
    public class OnBoardingViewModel : ViewModelBase
    {
        #region Fields

        readonly INavigationService navigationService;

        string header = string.Empty;
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }

        string content = string.Empty;
        public string Content
        {
            get { return content; }
            set { SetProperty(ref content, value); }
        }


        public string ImagePath { get; set; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/ChooseGradient.svg";
        #endregion

        /// <summary>
        /// Initializes a new instance for the <see cref="OnBoardingGradientViewModel" /> class.
        /// </summary>
        public OnBoardingViewModel(INavigationService _navigationService, IAnalyticsService analyticsService) : base(_navigationService, analyticsService)
        {
            navigationService = _navigationService;
            
            Header = "Digital Bike Frame Number";
            Content = "";
            ImagePath = "Digifyy.png";

            this.ScanCommand = new Command(this.ScanNFC);
            this.LoginCommand = new Command(this.ToLoginPage);
        }

        

        #region Commands

        /// <summary>
        /// Gets or sets the command that is executed when the Skip button is clicked.
        /// </summary>
        public ICommand ScanCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Done button is clicked.
        /// </summary>
        public ICommand LoginCommand { get; set; }

        #endregion

        private void ToLoginPage()
        {
            navigationService.NavigateTo(typeof(LoginPageViewModel), string.Empty, string.Empty);
        }

        private void ScanNFC()
        {
            navigationService.NavigateTo(typeof(ScanViewModel), string.Empty, string.Empty);
        }
    }
}
