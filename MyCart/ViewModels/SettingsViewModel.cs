using DigiFyy.DataService;
using DigiFyy.Models;
using DigiFyy.Models.AWS;
using DigiFyy.Resources;
using DigiFyy.Services;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using SelectionChangedEventArgs = Syncfusion.XForms.ComboBox.SelectionChangedEventArgs;

namespace DigiFyy.ViewModels
{
    /// <summary>
    /// Viewmodel of settings page
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SettingsViewModel : ViewModelBase
    {
        public ObservableCollection<Language> _languages;
        public ObservableCollection<Language> Languages
        {
            get { return _languages; }
            set { SetProperty(ref _languages, value); }
        }
        
        private string _selectedLanguage;
        private bool _useFakeUUID;
        private bool _userIsLoggedIn;
        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string uuid;
        public string UUID
        {
            get { return uuid; }
            set { SetProperty(ref uuid, value); }
        }

        public bool UseFakeUUID
        {
            get { return _useFakeUUID; }
            set { SetProperty(ref _useFakeUUID, value); }
        }

        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { SetProperty(ref _selectedLanguage, value); }
        }

        public bool UserIsLoggedIn
        {
            get { return _userIsLoggedIn; }
            set { SetProperty(ref _userIsLoggedIn, value); }
        }

        private string _registrationText;
        public string RegistrationText
        {
            get { return _registrationText; }
            set { SetProperty(ref _registrationText, value); }
        }


        private string _loggedInText;
        public string LoggedInText
        {
            get { return _loggedInText; }
            set { SetProperty(ref _loggedInText, value); }
        }

       
 

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DigiFyy.ViewModels.SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel()
        {
            UUID = Constants.FakeUUID;
            _languages = new ObservableCollection<Language>()
            {
                new Language { DisplayName =  "English", ShortName = "en" },
                new Language { DisplayName =  "Danish", ShortName = "da" }
            };
            Languages = _languages;

            Language language = Languages.FirstOrDefault(p => p.ShortName == Preferences.Get("Language", "en"));
            SelectedLanguage = language.DisplayName ?? Languages.FirstOrDefault(p => p.ShortName == "en").DisplayName;

            
           
            Email = Preferences.Get("Email", "");
            this.SaveCommand = new Helpers.Command(this.SaveSettingsClicked);
            this.LogoutCommand = new Helpers.Command(this.LogoutClicked);
            this.UnregisterCommand = new Helpers.Command(this.UnregisterClicked);
            this.SelectionChangedCommand = new Helpers.Command(ComboBoxSelectionChanged);

            UserIsLoggedIn = Preferences.Get("IsLoggedIn", "0") == "1";
            LoggedInText = UserIsLoggedIn ? "Logged in" : "Not logged in";

            RegistrationText = Preferences.Get("RegisteredToBike", "0")  == "1" ? "Registered to bike" : "Not registered";

            UseFakeUUID = Preferences.Get("UseFakeUUID", "0") == "1";
        }

        public override async Task InitializeAsync(object navigationData)
        {
            UserIsLoggedIn = Preferences.Get("IsLoggedIn", "0") == "1";
            LoggedInText = UserIsLoggedIn ? "Logged in" : "Not logged in";
            RegistrationText = Preferences.Get("RegisteredToBike", "0") == "1" ? "Registered to bike" : "Not registered";
        }


        private void ComboBoxSelectionChanged(object obj)
        {
            var selectionChangedArgs = obj as SelectionChangedEventArgs;
            SelectedLanguage = (selectionChangedArgs.Value as Language).DisplayName;
        }

        public Helpers.Command SaveCommand { get; set; }
        public Helpers.Command LogoutCommand { get; set; }
        public Helpers.Command UnregisterCommand { get; set; }

        public Helpers.Command SelectionChangedCommand { get; set; }

        private void SaveSettingsClicked()
        {
            CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(element => element.EnglishName.Contains(SelectedLanguage));
            AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
            Preferences.Set("Language", Languages.FirstOrDefault(p => p.DisplayName == SelectedLanguage).ShortName);
            if (Email != "")
                Preferences.Set("Email", Email);

            Preferences.Set("UseFakeUUID", UseFakeUUID ? "1" : "0");
            
        }

        private async void LogoutClicked(object obj)
        {
            Preferences.Set("IsLoggedIn", "0");
            UserIsLoggedIn = false;
            LoggedInText = "Not logged in";
            Preferences.Set("Token", "");
            Preferences.Set("Password", "");
            


        }

        private async void UnregisterClicked(object obj)
        {
            Preferences.Set("RegisteredToBike", "0");
            RegistrationText = "Not registered";

            FrameNumberStatus fns = new FrameNumberStatus()
            {
                LastUpdateTime = DateTime.Now,
                Latitude = 0,
                Longitude = 0,
                LastUpdateClientID = Xamarin.Essentials.DeviceInfo.Name,
                Status = (int)FrameNumberStatusType.NotRegistered
            };
            IsBusy = true;
            string uuid = Preferences.Get("UUID", "");
            string user = Preferences.Get("Email", "");
            string token = Preferences.Get("Token", "");


            await DataStore.UpdateStatus(user, token, uuid, fns);
            var bikeDetailViewModel = ViewModelLocator.Resolve<BikeDetailViewModel>();
            if (bikeDetailViewModel != null)
                MessagingCenter.Send(bikeDetailViewModel, MessageKeys.UpdateState, 1);
            IsBusy = false;
        }


    }
}
