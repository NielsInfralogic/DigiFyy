using DigiFyy.DataService;
using DigiFyy.Helpers;
using DigiFyy.Models;
using DigiFyy.Models.AWS;
using DigiFyy.Services;
using Newtonsoft.Json;
using Plugin.Multilingual;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    /// <summary>
    /// ViewModel for sign-up page.
    /// </summary>
    public class SignUpViewModel : ViewModelBase
    {
        #region Fields

        private bool firstTimeRegister;

        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                SetProperty(ref name, value);
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

        private string confirmPassword;
        public string ConfirmPassword
        {
            get
            {
                return this.confirmPassword;
            }

            set
            {
                SetProperty(ref confirmPassword, value);
            }
        }

        private bool wrongPassword = false;
        public bool WrongPassword
        {
            get
            {
                return this.wrongPassword;
            }

            set
            {
                SetProperty(ref wrongPassword, value);
            }
        }

        private string passwordError;
        public string PasswordError
        {
            get
            {
                return this.passwordError;
            }

            set
            {
                SetProperty(ref passwordError, value);
            }
        }


        private string email = "";
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

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="SignUpViewModel" /> class.
        /// </summary>
        public SignUpViewModel()
        {
            this.BackButtonCommand = new Helpers.Command(this.BackButtonClicked);
            this.SignUpCommand = new Helpers.Command(this.SignUpClicked);
            this.SocialMediaLoginCommand = new Helpers.Command(this.SocialLoggedIn);
            this.SocialMediaLoginCommand2 = new Helpers.Command(this.SocialLoggedInGoogle);

            Email = "";
            Password = "";
            ConfirmPassword = "";
            WrongPassword = false;
            PasswordError = "";

        }

        #endregion

        public override Task InitializeAsync(object navigationData)
        {           
            if (navigationData is Boolean)
                firstTimeRegister = (Boolean)navigationData;
             
            return base.InitializeAsync(navigationData);
        }
         
        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Helpers.Command SignUpCommand { get; set; }
        public Helpers.Command BackButtonCommand { get; set; }

        /// </summary>
        public Helpers.Command SocialMediaLoginCommand { get; set; }
        public Helpers.Command SocialMediaLoginCommand2 { get; set; }

        #endregion


        private async void BackButtonClicked(object obj)
        {

            await NavigationService.GoBackAsync();
        }


        #region Methods

        /// <summary>
        /// Invoked when the Log in button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void LoginClicked(object obj)
        {
         //   NavigationService.NavigateTo(typeof(LoginPageViewModel), string.Empty, string.Empty);
            await NavigationService.NavigateToAsync<LoginViewModel>(firstTimeRegister);
            await NavigationService.RemoveBackStackAsync();
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SignUpClicked(object obj)
        {
            // Do something

            // Register user..1

            Email = Email.Trim();
            Password = Password.Trim();
            ConfirmPassword = ConfirmPassword.Trim();

            var resmgr = new ResourceManager("DigiFyy.Resources.AppResources", typeof(TranslateExtension).GetTypeInfo().Assembly);
            var ci = CrossMultilingual.Current.CurrentCultureInfo;

          

            string uuid = Preferences.Get("UUID", "");

            if (string.IsNullOrEmpty(Email))
            {
                IsInvalidEmail = true;
                return;
            }

            if (Password.Length < 8)
            {

                PasswordError = resmgr.GetString("PasswordTooShort", ci); // "Password too short (8 chars min)";
                WrongPassword = true;
                return;
            }

            if (ConfirmPassword != Password)
            {
                PasswordError = resmgr.GetString("PasswordMismatch", ci);
                WrongPassword = true;
                return;

            }


            IsBusy = true;
            Owner owner = new Owner() { Email = Email, Name = "anonymous", OwnerID = 0 };
            UniqueID uniqueID = new UniqueID() { Username = Email, Name = Email, Password = Password, UID = uuid, Owner = owner };

            // Step 1 - register new user

            var result1 = await DataStore.RegisterUUID(uniqueID);

            int registerStatus = result1.Item2;

            if (registerStatus == (int)OwnerRegisterErrorType.UIDDoesNotExist)
                await DialogService.Show("Error registering", "The UUID does not exist", "Ok");
            else if (registerStatus == (int)OwnerRegisterErrorType.UIDRegisterToOtherUser)
                await DialogService.Show("Error registering", "The UUID is registered to another user", "Ok");
            else if (registerStatus == (int)OwnerRegisterErrorType.UIDReportedStolen)
                await DialogService.Show("Error registering", "The bike is reported stolen and cannot be registered", "Ok");
            else
            {
                

                // Step 2: Login user to obtain token

                var result2 = await DataStore.LoginUser(Email, Password);
                if (result2.Token != "")
                {
                    Preferences.Set("Token", result2.Token);
                    Preferences.Set("Email", Email);
                    Preferences.Set("Password", Password);
                    Preferences.Set("IsLoggedIn", "1");
                    IsBusy = false;

                    if (result1.Item1.UID != "")
                    {
                        // Step 3 - Link user to owner to UUID 

                        if (Preferences.Get("RegisteredToBike", "0") != "1")
                        {
                            int imageType = (int)ImageTypes.Invoice;
                            await NavigationService.NavigateToAsync<AddPictureViewModel>(imageType);
                        }
                        else
                            await NavigationService.NavigateToAsync<MainViewModel>(2);
                    }
                }
            }

            

           
            IsBusy = false;


            //  int imageType = (int)ImageTypes.Invoice;
            // await NavigationService.NavigateToAsync<AddPictureViewModel>(imageType);
        }

        #endregion

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


            // account = store.FindAccountsForService(Constants.AppName).FirstOrDefault();

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
            catch (Exception ex)
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

    }
}