using DigiFyy.Models;
using DigiFyy.Resources;
using DigiFyy.Services;
using Plugin.Multilingual;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DigiFyy.ViewModels
{
    /// <summary>
    /// Viewmodel of settings page
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SettingsViewModel : ViewModelBase
    {
        public ObservableCollection<Language> Languages;


        private Language _SelectedLanguage;

        public Language SelectedLanguage
        {
            get { return _SelectedLanguage; }
            set { SetProperty(ref _SelectedLanguage, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DigiFyy.ViewModels.SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel(INavigationService navigationService, IAnalyticsService analyticsService) :  base(navigationService, analyticsService)
        {
            Languages = new ObservableCollection<Language>()
            {
                new Language { DisplayName =  "English", ShortName = "en" },
                new Language { DisplayName =  "Danish", ShortName = "da" } 
            };

            Language language = Languages.FirstOrDefault(p => p.ShortName == Preferences.Get("Language", "en"));
            SelectedLanguage = language ?? Languages.FirstOrDefault(p => p.ShortName == "en");
            this.SaveCommand = new Command(this.SaveSettings);
            this.ShowFilesCommand = new Command(this.ShowHiddenFilesTapped);
            this.PolicyCommand = new Command(this.PrivacyPolicyTapped);
        }

        public ICommand SaveCommand { get; set; }

        private void SaveSettings()
        {
            CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(element => element.EnglishName.Contains(SelectedLanguage.DisplayName));
            AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
            Preferences.Set("Language", SelectedLanguage.ShortName);
        }


        
        /// <summary>
        /// Gets or sets the value of command used for show hidden files click.
        /// </summary>
        public Command ShowFilesCommand { get; set; }

        /// <summary>
        /// Gets or sets the value of command used for privacy policy click.
        /// </summary>
        public Command PolicyCommand { get; set; }

        /// <summary>
        /// Invoked when download quality tapped.
        /// </summary>
        /// <param name="obj">The Object.</param>
        private void DownloadQualityTapped(object obj)
        {
        }

        /// <summary>
        /// Invoked when Show hidden files tapped.
        /// </summary>
        /// <param name="obj">The Object.</param>
        private void ShowHiddenFilesTapped(object obj)
        {
        }

        /// <summary>
        /// Invoked when Privacy policy tapped.
        /// </summary>
        /// <param name="obj">The Object.</param>
        private void PrivacyPolicyTapped(object obj)
        {
        }
    }
}
