
using DigiFyy.DataService;
using DigiFyy.Resources;
using DigiFyy.Services;
using DigiFyy.ViewModels;
using DigiFyy.Views;
using Plugin.Multilingual;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigiFyy
{
    public partial class App : Application
    {

//        readonly NavigationService navigationService = (NavigationService)TypeLocator.Instance.Resolve(typeof(INavigationService));


        public App()
        {
           
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjMyNTM4QDMxMzgyZTMxMmUzMGk5SnZMR3JhaWRBSzY0cEFCaDhQVGI0WDQ2ZDcrbTVYQVJWMmRyR0dybjA9");

            // Adjust culture from setting
            if (Preferences.Get("Language", "") == "")
            {
                string ss = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                Preferences.Set("Language", (ss == "en" || ss == "da") ? ss : "da");
            }

            CultureInfo cultureFromPreference = CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(p => p.TwoLetterISOLanguageName == Preferences.Get("Language", "en"));
            AppResources.Culture = cultureFromPreference ?? CrossMultilingual.Current.DeviceCultureInfo;
            CrossMultilingual.Current.CurrentCultureInfo = AppResources.Culture;

            InitializeComponent();

            InitApp();

            if (Device.RuntimePlatform == Device.UWP)
            {
                InitNavigation();
            }
        }

        private void InitApp()
        {
            ListenNetworkChanges();
           
            // Ensure login at app startup..
            Preferences.Set("Token", "");
           
            // Sanitize..
            Preferences.Set("Email", Preferences.Get("Email", "").Trim());
            Preferences.Set("Password", Preferences.Get("Password", "").Trim());
            
            // Demo mode..
            if (Preferences.Get("UseFakeUUID", "0") == "1")
                Preferences.Set("UUID", Constants.FakeUUID);
            Preferences.Set("UUID", Preferences.Get("UUID", "").Trim());

        }


        protected override async void OnStart()
        {
            base.OnStart();

            if (Device.RuntimePlatform != Device.UWP)
            {
                await InitNavigation();
            }

            base.OnResume();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        private Task InitNavigation()
        {
            var navigationService = ViewModelLocator.Resolve<INavigationService>();
            return navigationService.InitializeAsync();
        }

        private static void ListenNetworkChanges()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private static void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckInternet();
        }

        static bool onErrorPage;
        private static void CheckInternet()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                onErrorPage = true;
                Application.Current.MainPage.Navigation.PushAsync(new NoInternetConnectionView());
            }
            else if (onErrorPage)
            {
                Application.Current.MainPage.Navigation.PopAsync();
                onErrorPage = false;
            }
        }
    }
}
