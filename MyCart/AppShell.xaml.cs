using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using DigiFyy.ViewModels;
using DigiFyy.Services;
using DigiFyy.Views;
using System;

namespace DigiFyy
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public ICommand LogoutCommand => new Command(GoToLogout);

        readonly NavigationService navigationService = (NavigationService)TypeLocator.Instance.Resolve(typeof(INavigationService));

        public AppShell()
        {
            InitializeComponent();

            FlyoutItem item = new FlyoutItem() { Title = "Home" };
            ShellContent content = new ShellContent
            {
                //Home Page
                Title = "Home",
                Content = navigationService.GetPageWithBindingContext(typeof(BikeDetailViewModel), Preferences.Get("UUID", ""), string.Empty)
            };
            item.Items.Add(content);
            this.Items.Add(item);

            //Aboutus Page
            item = new FlyoutItem() { Title = "About" };
            content = new ShellContent
            {
                Title = "About",
                Content = navigationService.GetPageWithBindingContext(typeof(AboutUsViewModel), string.Empty, string.Empty)
            };
            item.Items.Add(content);
            this.Items.Add(item);

            //Logout Menu
            MenuItem logout = new MenuItem() { Text = "Logout", Command = new Command(GoToLogout) };
            this.Items.Add(logout);

            RegisterRoutes();
        }

        internal static Page Init()
        {
            ListenNetworkChanges();
            IntializeBuildContainer();

            var navigationService = TypeLocator.Instance.Resolve(typeof(INavigationService)) as INavigationService;
           // var startup = TypeLocator.Instance.Resolve(typeof(Startup)) as Startup;

            var mainPage = GetMainPage();//startup.GetMainPage();

            return ((NavigationService)navigationService).GetPageWithBindingContext(mainPage, string.Empty, string.Empty);
        }

        public static Type GetMainPage()
        {
            var isNew = Preferences.Get("isnew", "");
            //if (isNew == "true")
            if (true)
            {
                Preferences.Set("isnew", "false");
                return typeof(OnBoardingViewModel);
            }
            else
            {
                var email = Preferences.Get("Email", "");

                if (string.IsNullOrEmpty(email))
                {
                    return typeof(LoginPageViewModel);
                }
                else
                {
                    return typeof(BikeDetailViewModel);
                }
            }
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("login", typeof(SimpleLoginPage));
            Routing.RegisterRoute("forgotpassword", typeof(SimpleForgotPasswordPage));
            Routing.RegisterRoute("resetpassword", typeof(SimpleResetPasswordPage));
            Routing.RegisterRoute("signup", typeof(SimpleSignUpPage));
            Routing.RegisterRoute("detail", typeof(BikeDetailView));
        }

        void GoToLogout()
        {
            Preferences.Set("Email", "");
            navigationService.NavigateTo(typeof(LoginPageViewModel), string.Empty, string.Empty, true);
        }

        private static void IntializeBuildContainer()
        {
            TypeLocator.Instance.Build();
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
                Application.Current.MainPage.Navigation.PushAsync(new NoInternetConnectionPage());
            }
            else if (onErrorPage)
            {
                Application.Current.MainPage.Navigation.PopAsync();
                onErrorPage = false;
            }
        }
    }
}