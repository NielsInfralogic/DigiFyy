using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System.Net.Http;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Auth;
using DigiFyy.Data;
using DigiFyy.Models;
using DigiFyy.ViewModels;
using DigiFyy.Helpers;
using DigiFyy.Services;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace DigiFyy.ViewModels
{

    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    public class LoginPageViewModel : LoginViewModelBase
    {
        #region Fields

        Account account;
        [Obsolete]
        readonly AccountStore store;

        private string password;
        readonly IDialogService DialogService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="LoginPageViewModel" /> class.
        /// </summary>
        public LoginPageViewModel(INavigationService navigationService, IAnalyticsService analyticsService, IDialogService dialogService)
            :base(navigationService, analyticsService)
        {
            DialogService = dialogService;

            this.LoginCommand = new Helpers.Command(this.LoginClicked);
            this.SignUpCommand = new Helpers.Command(this.SignUpClicked);
            this.ForgotPasswordCommand = new Helpers.Command(this.ForgotPasswordClicked);
            this.SocialMediaLoginCommand = new Helpers.Command(this.SocialLoggedIn);
            this.SocialMediaLoginCommand2 = new Helpers.Command(this.SocialLoggedInGoogle);
    

            store = AccountStore.Create();
        }



        #endregion

        #region property

        /// <summary>
        /// Gets or sets the property that is bound with an entry that gets the password from user in the login page.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                SetProperty(ref password, value);
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Helpers.Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Helpers.Command SignUpCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Forgot Password button is clicked.
        /// </summary>
        public Helpers.Command ForgotPasswordCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the social media login button is clicked.
        /// </summary>
        public Helpers.Command SocialMediaLoginCommand { get; set; }
        public Helpers.Command SocialMediaLoginCommand2 { get; set; }
        #endregion

        #region methods

        /// <summary>
        /// Invoked when the Log In button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void LoginClicked(object obj)
        {
         //   NavigationService.NavigateTo(typeof(BikeDetailViewModel), PreferenceService.Get("UUID"), string.Empty, true);

    
            if (string.IsNullOrEmpty(Email))
            {
                IsInvalidEmail = true;
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                return;
            }
            IsBusy = true;

            var result = await DataStore.LoginUser(Email, Password);

            if (result.Token != "")
            {
                Preferences.Set("Token", result.Token);
                Preferences.Set("Email", Email);
                Preferences.Set("Password", Password);

                Preferences.Set("UUID", "1234567654323456765432-aasdfgfdsa");
                IsBusy = false;
                NavigationService.NavigateTo(typeof(BikeDetailViewModel), Preferences.Get("UUID",""), string.Empty, false);
            }
            IsBusy = false;
        }        

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SignUpClicked(object obj)
        {
            NavigationService.NavigateTo(typeof(SignUpPageViewModel), string.Empty, string.Empty, false);
            //await Application.Current.MainPage.Navigation.PushAsync(new SimpleSignUpPage());
        }

        /// <summary>
        /// Invoked when the Forgot Password button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void ForgotPasswordClicked(object obj)
        {
            //TODO: Find an alternative way to change this color.
            //var label = obj as Label;
            //label.BackgroundColor = Color.FromHex("#70FFFFFF");
            //await Task.Delay(100);
            //label.BackgroundColor = Color.Transparent;

            //await Application.Current.MainPage.Navigation.PushAsync(new SimpleForgotPasswordPage());
            NavigationService.NavigateTo(typeof(ForgotPasswordViewModel), string.Empty, string.Empty, false);
        }

        /// <summary>
        /// Invoked when social media login button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SocialLoggedIn(object obj)
        {
            string clientId = null;

            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constants.FacebookiOSClientId;
                    redirectUri = Constants.FacebookiOSRedirectUrl;
                    break;
                case Device.Android:
                    clientId = Constants.FacebookAndroidClientId;
                    redirectUri = Constants.FacebookAndroidRedirectUrl;
                    break;
            }

            account = store.FindAccountsForService(Constants.AppName).FirstOrDefault();

            OAuth2Authenticator authenticator = new OAuth2Authenticator(
                                           clientId,
                                           Constants.FacebookScope,
                                           new Uri(Constants.FacebookAuthorizeUrl),
                                           new Uri(Constants.FacebookAccessTokenUrl));
            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;
       

            AuthenticationState.Authenticator = authenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();

            try
            {
                presenter.Login(authenticator);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("OAuthLoginPresenter exception error: " + ex.Message);
                if (ex.InnerException != null)
                    Debug.WriteLine("OAuthLoginPresenter inner exception error: " + ex.InnerException.Message);
            }
        }


        private void SocialLoggedInGoogle(object obj)
        {
            string clientId = null;

            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:

                    clientId = Constants.GoogleiOSClientId;

                    redirectUri = Constants.GoogleiOSRedirectUrl;

                    break;



                case Device.Android:

                    clientId = Constants.GoogleAndroidClientId;

                    redirectUri = Constants.GoogleAndroidRedirectUrl;

                    break;
            }

            account = store.FindAccountsForService(Constants.AppName).FirstOrDefault();

            OAuth2Authenticator authenticator = new OAuth2Authenticator(
                                           clientId,
                                           null,
                                           Constants.GoogleScope,
                                           new Uri(Constants.GoogleAuthorizeUrl),
                                           new Uri(redirectUri),
                                            new Uri(Constants.GoogleAccessTokenUrl),
                                            null,
                                                 true);


            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;


            AuthenticationState.Authenticator = authenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();

            try
            {
                presenter.Login(authenticator);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OAuthLoginPresenter exception error: " + ex.Message);
                if (ex.InnerException != null)
                    Debug.WriteLine("OAuthLoginPresenter inner exception error: " + ex.InnerException.Message);
            }
        }



        [Obsolete]
        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            if (e.IsAuthenticated)
            {
                if (authenticator.AuthorizeUrl.Host == "www.facebook.com")
                {
                    FacebookEmail facebookEmail = null;

                    var httpClient = new HttpClient();
                    var json = await httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=id,name,first_name,last_name,email,picture.type(large)&access_token=" + e.Account.Properties["access_token"]);
                    facebookEmail = JsonConvert.DeserializeObject<FacebookEmail>(json);
                    await store.SaveAsync(account = e.Account, Constants.AppName);

                    Application.Current.Properties.Remove("Id");
                    Application.Current.Properties.Remove("FirstName");
                    Application.Current.Properties.Remove("LastName");
                    Application.Current.Properties.Remove("DisplayName");
                    Application.Current.Properties.Remove("EmailAddress");
                    Application.Current.Properties.Remove("ProfilePicture");
                    Application.Current.Properties.Add("Id", facebookEmail.Id);
                    Application.Current.Properties.Add("FirstName", facebookEmail.First_Name);
                    Application.Current.Properties.Add("LastName", facebookEmail.Last_Name);
                    Application.Current.Properties.Add("DisplayName", facebookEmail.Name);
                    Application.Current.Properties.Add("EmailAddress", facebookEmail.Email);
                    Application.Current.Properties.Add("ProfilePicture", facebookEmail.Picture.Data.Url);
                    //await Navigation.PushAsync(new ProfilePage());
                    NavigationService.NavigateTo(typeof(BikeDetailViewModel), "selectedCategory", string.Empty, true);
                }
                else
                {
                    User user = null;

                    // If the user is authenticated, request their basic user data from Google
                    // UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
                    var request = new OAuth2Request("GET", new Uri(Constants.GoogleUserInfoUrl), null, e.Account);
                    var response = await request.GetResponseAsync();
                    if (response != null)
                    {
                        // Deserialize the data and store it in the account store

                        // The users email address will be used to identify data in SimpleDB

                        string userJson = await response.GetResponseTextAsync();

                        user = JsonConvert.DeserializeObject<User>(userJson);
                    }

                    if (account != null)
                    {
                        store.Delete(account, Constants.AppName);
                    }

                    await store.SaveAsync(account = e.Account, Constants.AppName);

                    Application.Current.Properties.Remove("Id");
                    Application.Current.Properties.Remove("FirstName");
                    Application.Current.Properties.Remove("LastName");
                    Application.Current.Properties.Remove("DisplayName");
                    Application.Current.Properties.Remove("EmailAddress");
                    Application.Current.Properties.Remove("ProfilePicture");
                    Application.Current.Properties.Add("Id", user.Id);
                    Application.Current.Properties.Add("FirstName", user.GivenName);
                    Application.Current.Properties.Add("LastName", user.FamilyName);
                    Application.Current.Properties.Add("DisplayName", user.Name);
                    Application.Current.Properties.Add("EmailAddress", user.Email);
                    Application.Current.Properties.Add("ProfilePicture", user.Picture);

                    //await Navigation.PushAsync(new ProfilePage());
                    NavigationService.NavigateTo(typeof(BikeDetailViewModel), Preferences.Get("UUID",""), string.Empty, true);
                }
            }
        }

        [Obsolete]
        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            OAuth2Authenticator authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }
            Debug.WriteLine("Authentication error: " + e.Message);
        }

        #endregion
    }
}