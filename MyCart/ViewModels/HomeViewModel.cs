using DigiFyy.Helpers;
using DigiFyy.Models;
using DigiFyy.Models.AWS;
using DigiFyy.Services;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        //    public ICommand SettingsCommand => new Command(async () => await SettingsAsync());



        private string _userStateText = string.Empty;
        public string UserStateText
        {
            get { return _userStateText; }
            set { SetProperty(ref _userStateText, value); }
        }

        private bool _registered;
        public bool Registered
        {
            get { return _registered; }
            set { SetProperty(ref _registered, value); }
        }

        public HomeViewModel()
        {

           

            this.ScanCommand = new Helpers.Command(this.ScanNFC);
            this.SeeBikesCommand = new Helpers.Command(this.ToBikeDetailsPage);

            Registered = Preferences.Get("RegisteredToBike", "0") == "1";

        }

        // Data from navigation
        public override async Task InitializeAsync(object navigationData)
        {
            
           
            if (IsBusy)
                return;

            IsBusy = true;

            // Renew token
            if (Preferences.Get("Email","") != "" && Preferences.Get("Password", "") != "" && Preferences.Get("Token", "") == "")
            {

                User result = await DataStore.LoginUser(Preferences.Get("Email", ""), Preferences.Get("Password", ""));
                if (result.Token != "")
                {
                    Preferences.Set("Token", result.Token);
                    Preferences.Set("IsLoggedIn", "1");
                    Preferences.Set("RegisteredToBike", result.UIDs.Count > 0 ? "1" : "0");
                    if (result.UIDs.Count > 0)
                        Preferences.Set("UUID", result.UIDs[0]);
                }
            }

            Registered = Preferences.Get("RegisteredToBike", "0") == "1";
            Translate();

            IsBusy = false;

        }

        private void Translate()
        {
            var resmgr = new ResourceManager("DigiFyy.Resources.AppResources", typeof(TranslateExtension).GetTypeInfo().Assembly);
            var ci = CrossMultilingual.Current.CurrentCultureInfo;
            string state = "";

            if (Preferences.Get("RegisteredToBike", "0") != "1")
                state = resmgr.GetString("Unregistred", ci);

            if (Preferences.Get("IsLoggedIn", "0") != "1")
            {
                if (state != "")
                    state += ", ";
                state += resmgr.GetString("NotLoggedIn", ci);
            }
            
            UserStateText = state;
        }

        #region Commands

        /// <summary>
        /// Gets or sets the command that is executed when the Skip button is clicked.
        /// </summary>
        public ICommand ScanCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Done button is clicked.
        /// </summary>
        public ICommand SeeBikesCommand { get; set; }

        #endregion

        private async void ToBikeDetailsPage()
        {
            //   TabParameter tabParameter = new TabParameter() { TabIndex = 2 };
            // await NavigationService.NavigateToAsync<MainViewModel>(tabParameter);

            var mainViewModel = ViewModelLocator.Resolve<MainViewModel>();
            if (mainViewModel != null)
                MessagingCenter.Send(mainViewModel, MessageKeys.ChangeTab, 2);
        }

        private async void ScanNFC()
        {
            // TabParameter tabParameter = new TabParameter() { TabIndex = 1 };
            // await NavigationService.NavigateToAsync<MainViewModel>(tabParameter);      

            var mainViewModel = ViewModelLocator.Resolve<MainViewModel>();
            if (mainViewModel != null)
                MessagingCenter.Send(mainViewModel, MessageKeys.ChangeTab, 1);
        }
    }
}
