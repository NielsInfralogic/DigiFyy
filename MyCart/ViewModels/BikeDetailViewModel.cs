using DigiFyy.DataService;
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
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    public class BikeDetailViewModel : ViewModelBase
    {
        #region Fields

        private string _imagePath = "digifyy_bikelogo.png";
        public string ImagePath
        {
            get
            { return _imagePath; }
            set
            {
                SetProperty(ref _imagePath, value);
            }
        }


        private bool _notRegistered;
        public bool NotRegistered
        {
            get { return _notRegistered; }
            set
            {
                SetProperty(ref _notRegistered, value);
            }
        }

        private bool _registered;
        public bool Registered
        {
            get { return _registered; }
            set
            {
                SetProperty(ref _registered, value);
            }
        }


        private bool _loggedIn;
        public bool LoggedIn
        {
            get { return _loggedIn; }
            set
            {
                SetProperty(ref _loggedIn, value);
            }
        }

        private bool _notLoggedIn;
        public bool NotLoggedIn
        {
            get { return _notLoggedIn; }
            set
            {
                SetProperty(ref _notLoggedIn, value);
            }
        }


        private bool _registeredToOtherUser;
        public bool RegisteredToOtherUser
        {
            get { return _registeredToOtherUser; }
            set
            {
                SetProperty(ref _registeredToOtherUser, value);
            }
        }

        private bool _stolen;
        public bool Stolen
        {
            get { return _stolen; }
            set
            {
                SetProperty(ref _stolen, value);
            }
        }


        private List<FrameNumberExtra> FrameNumberExtras;

        private FrameNumberStatus _frameNumberStatus;
        public FrameNumberStatus FrameNumberStatus
        {
            get { return _frameNumberStatus; }
            set
            {
                SetProperty(ref _frameNumberStatus, value);
            }
        }

        private List<FrameNumberDocument> Documents;
        private int _documentCount;
        public int DocumentCount
        {
            get { return _documentCount; }
            set { 
                SetProperty(ref _documentCount, value); 
                SetProperty(ref _showDocumentCount, value > 0);  
            }
        }

        private bool _showDocumentCount;
        public bool ShowDocumentCount
        {
            get { return _showDocumentCount; }
        }
        private bool _showMessageCount;
        public bool ShowMessageCount 
        { 
            get { return _showMessageCount; } 
        }

        private int _messageCount; 
        public int MessageCount 
        { 
            get { return _messageCount; }
            set { 
                SetProperty(ref _messageCount, value);
                SetProperty(ref _showMessageCount, value>0);
            }
        }
   
        string _brand;
        public string Brand
        {
            get
            {
                return _brand;
            }

            set
            {
                SetProperty(ref _brand, value);
            }
        }

        string _model;
        public string Model
        {
            get
            {
                return _model;
            }

            set
            {
                SetProperty(ref _model, value);
            }
        }

        string _uuid;
        public string UUID
        {
            get
            {
                return _uuid;
            }
            set
            {
                SetProperty(ref _uuid, value);
            }
        }

        private int _status;
        public int Status
        {
            get
            {
                return _status;
            }
            set
            {
                SetProperty(ref _status, value);
            }
        }

        int _productionYear;
        public int ProductionYear
        {
            get
            {
                return _productionYear;
            }
            set
            {
                SetProperty(ref _productionYear, value);
            }
        }
        
        private string _extras;
        public string Extras
        {
            get
            {
                return _extras;
            }
            set
            {
                SetProperty(ref _extras, value);
            }
        }

        #endregion

        #region Constructor

        public BikeDetailViewModel()
        {
            this.ChangeStatusCommand = new Helpers.Command(this.ChangeStatusClicked);
            this.DocsCommand = new Helpers.Command(this.DocumentsClicked);
            this.PartsCommand = new Helpers.Command(this.ShowPartsClicked);
            this.AddImageCommand = new Helpers.Command(this.AddImageClicked);
            this.SpecCommand = new Helpers.Command(this.ShowSpecsClicked);
            this.RegisterCommand = new Helpers.Command(this.RegisterClicked);
            this.ReportStolenCommand = new Helpers.Command(this.ReportStolenClicked);

            MessagingCenter.Subscribe<BikeDetailViewModel, int>(this, MessageKeys.UpdateState, async (sender, arg) =>
            {
                // For IsVisible properties in view
                Registered = Preferences.Get("RegisteredToBike", "0") == "1";
                NotRegistered = !Registered;
                LoggedIn = Preferences.Get("IsLoggedIn", "0") == "1";
                NotLoggedIn = !LoggedIn;

                if ( arg > 0)
                {
                    // re-read status from back-end
                    string uuid = Preferences.Get("UUID", "");
                    string user = Preferences.Get("Email", "");
                    string token = Preferences.Get("Token", "");
                    IsBusy = true;
                    FrameNumberStatus frameNumberStatus = await DataStore.GetStatus( user, token, uuid);
                    if (frameNumberStatus != null)
                    {
                        FrameNumberStatus = frameNumberStatus;
                        Stolen = (FrameNumberStatus.Status == (int)FrameNumberStatusType.ReportedStolen);
                        Status = FrameNumberStatus.Status;
                    }
                    IsBusy = false;
                }

              
            });


        }
        #endregion

        public override async Task InitializeAsync(object navigationData)
        {
            Registered = Preferences.Get("RegisteredToBike", "0") == "1";
            NotRegistered = !Registered;
            LoggedIn = Preferences.Get("IsLoggedIn", "0") == "1";
            NotLoggedIn = !LoggedIn;
            if (navigationData is Boolean)
            {
                if ((Boolean)navigationData)
                    LookupBikeInfo();
            }
          //  return base.InitializeAsync(navigationData);
        }

        #region Commands

        Helpers.Command _refreshCommand;
        public Helpers.Command RefreshCommand => _refreshCommand ?? (_refreshCommand = new Helpers.Command(LookupBikeInfo));

        public Helpers.Command ShowPositionCommand { get; set; }

        public Helpers.Command DocsCommand { get; set; }

        public Helpers.Command RegisterCommand { get; set; }
        
        public Helpers.Command AddImageCommand { get; set; }

        public Helpers.Command PartsCommand { get; set; }

        public Helpers.Command SpecCommand { get; set; }

        public Helpers.Command ChangeStatusCommand { get; set; }

        public Helpers.Command ReportStolenCommand { get; set; }

        #endregion

    

        private async void ChangeStatusClicked(object obj)
        {
           
            var resmgr = new ResourceManager("DigiFyy.Resources.AppResources", typeof(TranslateExtension).GetTypeInfo().Assembly);
            var ci = CrossMultilingual.Current.CurrentCultureInfo;

            string changeStatusText = resmgr.GetString("ChangeStatus", ci);
            string cancelText = resmgr.GetString("Cancel", ci);
            string reportStolenText = resmgr.GetString("ReportStolen", ci);
            string unregisterText = resmgr.GetString("Unregister", ci);
            string reclaimText = resmgr.GetString("Reclaim", ci);

            if (FrameNumberStatus.Status == (int)FrameNumberStatusType.ReportedStolen)
            {
                bool res = await DialogService.Show("Report found", "Do you want to report this bike found?", "Yes", "No");
                if (res)
                    ReportStolenClicked(null);
            }
            else if (FrameNumberStatus.Status == (int)FrameNumberStatusType.NotRegistered)
            {
                bool res = await DialogService.Show("Register", "Do you want to register this bike now?", "Yes", "No");
                if (res)
                    RegisterClicked(null);
            }
            else
            {
                string result = "";
                if (FrameNumberStatus.Status == (int)FrameNumberStatusType.Found)
                    result = await DialogService.ShowActionSheet(changeStatusText, cancelText, "", reportStolenText, unregisterText, reclaimText);
                else
                    result = await DialogService.ShowActionSheet(changeStatusText, cancelText, "", reportStolenText, unregisterText, "");

                if (result == reportStolenText)
                {
                    await ChangeStatus((int)FrameNumberStatusType.ReportedStolen);
                }
                else if (result == unregisterText)
                {
                    //  await NavigationService.NavigateToAsync<ShowPositionViewModel>(FrameNumberStatus);
                    await NavigationService.NavigateToAsync<DeregisterViewModel>();
                }

                else if (result == reclaimText)
                {
                    await ChangeStatus((int)FrameNumberStatusType.Registered);
                }
            }
        }


        private async Task<bool> ChangeStatus(int newStatus)
        {
            IsBusy = true;

            string uuid = Preferences.Get("UUID", "");
            string user = Preferences.Get("Email", "");
            string token = Preferences.Get("Token", "");

            if (uuid == "")
            {
                var resmgr = new ResourceManager("DigiFyy.Resources.AppResources", typeof(TranslateExtension).GetTypeInfo().Assembly);
                var ci = CrossMultilingual.Current.CurrentCultureInfo;
                await DialogService.Show(resmgr.GetString("UUIDNotRegistered",ci), "Ok");
                return false;
            }

            FrameNumberStatus fns = new FrameNumberStatus()
            {
                LastUpdateTime = DateTime.Now,
                Latitude = FrameNumberStatus.Latitude,
                Longitude = FrameNumberStatus.Longitude,
                LastUpdateClientID = DeviceInfo.Name,
                Status = newStatus
            };
            

            bool success = await DataStore.UpdateStatus(user, token, uuid, fns);
            if (success == true)
            {
                // Set local status if success..
                FrameNumberStatus = fns;
                Stolen = (FrameNumberStatus.Status == (int)FrameNumberStatusType.ReportedStolen);
                Status = FrameNumberStatus.Status;
            }

            IsBusy = false;
            return success;
        }


        private async void RegisterClicked(object obj)
        {
            if (Preferences.Get("IsLoggedIn","0") == "1")
                await NavigationService.NavigateToAsync<AddPictureViewModel>(2); // == invoice upload
            else
                await NavigationService.NavigateToAsync<LoginSignupViewModel>(true); // True : first time register

        }
        
        private async void AddImageClicked(object obj)
        {
            await NavigationService.NavigateToAsync<AddPictureViewModel>((int)ImageTypes.ProfileImage);
        }

        private async void ShowPartsClicked(object obj)
        {
            await NavigationService.NavigateToAsync<ShowPartsViewModel>(FrameNumberExtras);
        }

        private async void ShowSpecsClicked(object obj)
        {
            await NavigationService.NavigateToAsync<ShowSpecsViewModel>();
        }

        private async void ReportStolenClicked(object obj)
        {
            if (await ChangeStatus((int)FrameNumberStatusType.Found))
            {
                var resmgr = new ResourceManager("DigiFyy.Resources.AppResources", typeof(TranslateExtension).GetTypeInfo().Assembly);
                var ci = CrossMultilingual.Current.CurrentCultureInfo;

                bool reportPosition = await DialogService.Show(resmgr.GetString("ReportLocation", ci),
                                            resmgr.GetString("WeLikeToReportLocation", ci),
                                            resmgr.GetString("Accept", ci),
                                            resmgr.GetString("Deny", ci));
                if (reportPosition)
                    await NavigationService.NavigateToAsync<ShowPositionViewModel>(FrameNumberStatus);
            }
        }

        // Not used..
        private async void ShowPositionClicked(object obj)
        {
            await NavigationService.NavigateToAsync<ShowPositionViewModel>(FrameNumberStatus);
        }


        /// <summary>
        /// Invoked when the MessageClicked button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void DocumentsClicked(object obj)
        {
            await NavigationService.NavigateToAsync<DocumentsViewModel>(Documents);
        }


        private async void LookupBikeInfo()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            string uuid = Preferences.Get("UUID","");
            string user = Preferences.Get("Email", "");
            string token = Preferences.Get("Token", "");

           
           
            if (user != "" && token == "")
                user = "";
            bool isMyBike = false;
            if (uuid != "")
            {
                try
                {                    
                    UIDInfo result = await DataStore.GetInfo(user, token, uuid);

                    if (result.FrameNumber == null)
                    {
                        await DialogService.Show("UUID unknown", $"The scanned UUID {uuid} is unknown", "Ok");
                    }
                    else
                    {

                        if (result.Owner != null && token != "" && user != "")
                        {
                            if (string.Equals(result.Owner.Email, user, StringComparison.InvariantCultureIgnoreCase))
                                isMyBike = true;
                        }

                        Brand = result.FrameNumber.Manufacturer;
                        string model = result.FrameNumber.Model + " ";
                        if (result.FrameNumber.Frame != "")
                            model += result.FrameNumber.Frame;

                        Model = model;
                        UUID = result.FrameNumber.UID;
                        Status = result.FrameNumberStatus.Status;

                        Preferences.Set("BrandModel", Brand + " " + Model);

                        Stolen = (Status == (int)FrameNumberStatusValue.Stolen);

                        Registered = isMyBike && (Status != (int)FrameNumberStatusValue.Unregistered && Status != (int)FrameNumberStatusValue.Unknown);

                        Preferences.Set("RegisteredToBike", Registered ? "1" : "0");
                        NotRegistered = !Registered;

                        // Make sure we show 'stolen'
                        if (Stolen)
                            NotRegistered = false;


                        FrameNumberStatus = result.FrameNumberStatus;
                        ProductionYear = result.FrameNumber.ProductionDate.Year;

                        if (result.FrameNumberExtras != null)
                            FrameNumberExtras = result.FrameNumberExtras;
                        /* string extras = "";
                         if (result.FrameNumberExtras != null)
                         {
                             foreach (FrameNumberExtra ext in result.FrameNumberExtras)
                             {
                                 if (extras != "")
                                     extras += ", ";
                                 string thisExtra = "";
                                 if (ext.ExtraBrand != "")
                                     thisExtra += " ";
                                 if (ext.ExtraModel != "")
                                     thisExtra = ext.ExtraModel + " ";
                                 if (ext.ExtraType != "")
                                     thisExtra = ext.ExtraType;
                                 extras += thisExtra.Trim();
                             }
                             Extras = extras;
                         }*/

                        DocumentCount = result.FrameNumberDocuments != null ? result.FrameNumberDocuments.Count : 0;
                        Documents = result.FrameNumberDocuments ?? new List<FrameNumberDocument>();
                        MessageCount = result.NumberOfMessages;

                        var messageViewModel = ViewModelLocator.Resolve<MessagesViewModel>();

                        if (messageViewModel != null)
                            MessagingCenter.Send(messageViewModel, MessageKeys.UpdateMessageCount, MessageCount);


                        
                        if (NotRegistered)
                            ImagePath = Constants.DefaultProfileImage;
                        else if (result.ProfileImage != "")
                        {
                            ImagePath = result.ProfileImage;
                            Preferences.Set("ProfileImage", ImagePath);
                        }
                    }
                    IsBusy = false;

                }
                catch (Exception e)
                {
                    AnalyticsService.TrackError(e, new Dictionary<string, string>
                    {
                        { "Method", "BikeDetailViewModel.LookupBikeInfo" }
                    });
                }

            }
            else
            {
                await DialogService.Show("No scans", "No bike has been scanned. Please scan a bike..", "Ok");

                TabParameter tabParameter = new TabParameter() { TabIndex = 1 };
                await NavigationService.NavigateToAsync<MainViewModel>(tabParameter);
            }
            IsBusy = false;
        }


    }
}
