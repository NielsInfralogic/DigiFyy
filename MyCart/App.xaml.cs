
using DigiFyy.Resources;
using Plugin.Multilingual;
using System;
using System.Globalization;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigiFyy
{
    public partial class App : Application
    {
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
        public static string CurrentLanguage = "en";
        public App()
        {
           

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjAzMTIzQDMxMzcyZTM0MmUzMFBqOXYxWmNWc1VNVHBFUjZGQ1VMZGdkRGFKR3ZxUWNFbnFaTUFHVUtYenM9");

            InitializeComponent();

            // Adjust culture from setting
            Preferences.Set("Language", "da");

            CultureInfo cultureFromPreference = CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(p => p.TwoLetterISOLanguageName == Preferences.Get("Language", "en"));
            AppResources.Culture = cultureFromPreference ?? CrossMultilingual.Current.DeviceCultureInfo;
            CrossMultilingual.Current.CurrentCultureInfo = AppResources.Culture;

            MainPage = new NavigationPage(AppShell.Init());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
