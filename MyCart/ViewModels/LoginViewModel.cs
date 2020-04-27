using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System.Net.Http;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Auth;
using DigiFyy.Models;
using DigiFyy.ViewModels;
using DigiFyy.Helpers;
using DigiFyy.Services;
using Newtonsoft.Json;
using Xamarin.Essentials;
using DigiFyy.DataService;
using DigiFyy.Models.AWS;

namespace DigiFyy.ViewModels
{

    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        #region Fields

        private bool firstTimeRegister;


        private string email;
        public string Email
        {
            get
            {
                return this.email;
            }

            set
            {
                SetProperty(ref email, value);
            }
        }

        private bool wrongPassword;
        public bool WrongPassword
        {
            get
            {
                return wrongPassword;
            }

            set
            {
                SetProperty(ref wrongPassword, value);
            }
        }

        private bool isInvalidEmail;
        public bool IsInvalidEmail
        {
            get
            {
                return this.isInvalidEmail;
            }

            set
            {
                SetProperty(ref isInvalidEmail, value);
            }
        }


        private string password;
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

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="LoginViewModel" /> class.
        /// </summary>
        public LoginViewModel()
        {

            this.LoginCommand = new Helpers.Command(this.LoginClicked);
            this.BackButtonCommand = new Helpers.Command(this.BackButtonClicked);
            this.ForgotPasswordCommand = new Helpers.Command(this.ForgotPasswordClicked);
            this.SocialMediaLoginCommand = new Helpers.Command(this.SocialLoggedIn);
            this.SocialMediaLoginCommand2 = new Helpers.Command(this.SocialLoggedInGoogle);

            Email = Preferences.Get("Email", "");
            Password = Preferences.Get("Password", "");
            WrongPassword = false;

        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is LogoutParameter)
            {
                LogoutParameter logoutParameter = (LogoutParameter)navigationData;

                if (logoutParameter.Logout)
                {
                    Logout();
                }
            }

            if (navigationData is Boolean)
                firstTimeRegister = (Boolean)navigationData;

            WrongPassword = false;
            return base.InitializeAsync(navigationData);
        }

        private void Logout()
        {
            Preferences.Set("Token", "");
/*            var authIdToken = _settingsService.AuthIdToken;
            var logoutRequest = _identityService.CreateLogoutRequest(authIdToken);

            if (!string.IsNullOrEmpty(logoutRequest))
            {
                // Logout
                LoginUrl = logoutRequest;
            }

            if (_settingsService.UseMocks)
            {
                _settingsService.AuthAccessToken = string.Empty;
                _settingsService.AuthIdToken = string.Empty;
            }

            _settingsService.UseFakeLocation = false;*/
        }


        #endregion


        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Helpers.Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Forgot Password button is clicked.
        /// </summary>
        public Helpers.Command ForgotPasswordCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the social media login button is clicked.
        /// </summary>
        public Helpers.Command SocialMediaLoginCommand { get; set; }
        public Helpers.Command SocialMediaLoginCommand2 { get; set; }

        public Helpers.Command BackButtonCommand { get; set; }

        #endregion

        #region methods


        private async void BackButtonClicked(object obj)
        {
            await NavigationService.GoBackAsync();
        }

        /// <summary>
        /// Invoked when the Log In button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void LoginClicked(object obj)
        {
            //   NavigationService.NavigateTo(typeof(BikeDetailViewModel), PreferenceService.Get("UUID"), string.Empty, true);
            Email = Email.Trim();
            Password = Password.Trim();

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
                WrongPassword = false; 
                Preferences.Set("Token", result.Token);
                Preferences.Set("Email", Email);
                Preferences.Set("Password", Password);
                Preferences.Set("IsLoggedIn", "1");

                IsBusy = false;

                if (result.UIDs != null && Preferences.Get("UUID", "") != "")
                {
                    foreach(string uuid in result.UIDs)
                    {
                        if (uuid.Trim() == Preferences.Get("UUID", "").Trim())
                        {
                            Preferences.Set("RegisteredToBike", "1");
                            break;
                        }
                    }
                }

                if (Preferences.Get("RegisteredToBike", "0") != "1" && Preferences.Get("UUID","") != "")
                {
                    int imageType = (int)ImageTypes.Invoice;
                    await NavigationService.NavigateToAsync<AddPictureViewModel>(imageType);
                }
                else
                    await NavigationService.NavigateToAsync<MainViewModel>(2);
           
            }
            else
                WrongPassword = true;
            IsBusy = false;
        }        

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SignUpClicked(object obj)
        {
            await NavigationService.NavigateToAsync<SignUpViewModel>();
             await NavigationService.RemoveBackStackAsync();
  
        }

        /// <summary>
        /// Invoked when the Forgot Password button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void ForgotPasswordClicked(object obj)
        {
            await NavigationService.NavigateToAsync<ForgotPasswordViewModel>();          
        }

        /// <summary>
        /// Invoked when social media login button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SocialLoggedIn(object obj)
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

           
         
            var accounts = await SecureStorageAccountStore.FindAccountsForServiceAsync(Constants.AppName);

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

        private async void SocialLoggedInGoogle(object obj)
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

        //   account = store.FindAccountsForService(Constants.AppName).FirstOrDefault();
            var accounts = await SecureStorageAccountStore.FindAccountsForServiceAsync(Constants.AppName);

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
                    //await store.SaveAsync(account = e.Account, Constants.AppName);

                    try
                    {
                        await SecureStorageAccountStore.SaveAsync(e.Account, Constants.AppName);
                    }
                    catch { }
                 

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
//                    await NavigationService.NavigateTo(typeof(BikeDetailViewModel), string.Empty, string.Empty, true);
                    await NavigationService.NavigateToAsync<BikeDetailViewModel>();
                }
                else
                {
                    SocialMediaUser user = null;

                    // If the user is authenticated, request their basic user data from Google
                    // UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
                    var request = new OAuth2Request("GET", new Uri(Constants.GoogleUserInfoUrl), null, e.Account);
                    var response = await request.GetResponseAsync();
                    if (response != null)
                    {
                        // Deserialize the data and store it in the account store

                        // The users email address will be used to identify data in SimpleDB

                        string userJson = await response.GetResponseTextAsync();

                        user = JsonConvert.DeserializeObject<SocialMediaUser>(userJson);
                    }

                    /*if (account != null)
                    {
                        store.Delete(account, Constants.AppName);
                    }
                    await store.SaveAsync(account = e.Account, Constants.AppName); */

                    try
                    {
                        await SecureStorageAccountStore.SaveAsync(e.Account, Constants.AppName);
                    }
                    catch { }

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
                    await NavigationService.NavigateToAsync<BikeDetailViewModel>();
//                    await NavigationService.NavigateTo(typeof(BikeDetailViewModel), string.Empty, string.Empty, true);
                }
            }
        }

        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            if (sender is OAuth2Authenticator authenticator)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }
            Debug.WriteLine("Authentication error: " + e.Message);
        }

        #endregion
    }
}