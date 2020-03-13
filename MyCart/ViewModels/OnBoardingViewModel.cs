using DigiFyy.Helpers;
using DigiFyy.Services;
using DigiFyy.ViewModels;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Input;

namespace DigiFyy.ViewModels
{
    public class OnBoardingViewModel : ViewModelBase
    {
        #region Fields

        readonly INavigationService navigationService;

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

            Translate();

            this.ScanCommand = new Command(this.ScanNFC);
            this.LoginCommand = new Command(this.ToLoginPage);
        }

        private void Translate()
        {
            var resmgr = new ResourceManager("DigiFyy.Resources.AppResources", typeof(TranslateExtension).GetTypeInfo().Assembly);
            var ci = CrossMultilingual.Current.CurrentCultureInfo;
            Content = resmgr.GetString("ContentHeader", ci);
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
