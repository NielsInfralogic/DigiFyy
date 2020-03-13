using DigiFyy.Helpers;
using DigiFyy.Models;
using DigiFyy.Models.AWS;
using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace DigiFyy.ViewModels
{
    public class BikeDetailViewModel : ViewModelBase
    {
        #region Fields

        public string ImagePath { get; set; } = "Digifyy.png";


        private int _documentCount;
        public int DocumentCount
        {
            get { return _documentCount; }
            set { SetProperty(ref _documentCount, value); }
        }

        public bool ShowDocumentCount { get { return DocumentCount > 0; } }
        public bool ShowMessageCount { get { return MessageCount > 0; } }

        private int _messageCount; 
        public int MessageCount 
        { 
            get { return _messageCount; }
            set { SetProperty(ref _messageCount, value); }
        }



        private string _brand;
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

        string _frame;
        public string Frame
        {
            get
            {
                return _frame;
            }
            set
            {
                SetProperty(ref _frame, value);
            }
        }

        int _status;
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

        public BikeDetailViewModel(INavigationService navigationService, IAnalyticsService analyticsService) : base(navigationService, analyticsService)
        {
            // BikeInfo = new Bike();
            //       if (uuid != "")
            //        Preferences.Set("UUID", uuid);
            this.MessagesCommand = new Helpers.Command(this.MessagesClicked);
            this.DocumentsCommand = new Helpers.Command(this.DocumentsClicked);

            LookupBikeInfo();
        }
        #endregion

        #region Commands
        Command _refreshCommand;
        public Command RefreshCommand => _refreshCommand ?? (_refreshCommand = new Command(LookupBikeInfo));

        public Helpers.Command MessagesCommand { get; set; }
        public Helpers.Command DocumentsCommand { get; set; }


        /// <summary>
        /// Invoked when the MessageClicked button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void MessagesClicked(object obj)
        {
            NavigationService.NavigateTo(typeof(MessagesViewModel), string.Empty, string.Empty, false);
        }

        /// <summary>
        /// Invoked when the MessageClicked button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void DocumentsClicked(object obj)
        {
            NavigationService.NavigateTo(typeof(DocumentsViewModel), string.Empty, string.Empty, false);
        }

        /// <summary>
        /// Invoked when the MessageClicked button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void MessageClicked(object obj)
        {
            NavigationService.NavigateTo(typeof(MessagesViewModel), string.Empty, string.Empty, false);
        }
        #endregion

        private async void LookupBikeInfo()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            string uuid = Preferences.Get("UUID","");
            string user = Preferences.Get("Email","");
            string token = Preferences.Get("Token","");

            if (token != "" && user != "" && uuid != "")
            {
                try
                {
                    UIDInfo result = await DataStore.GetInfo(user, token, uuid);
                   
                    Brand = result.FrameNumber.Manufacturer;
                    Model = result.FrameNumber.Model;
                    UUID = result.FrameNumber.UID;
                    Frame = result.FrameNumber.Frame;
                    Status = result.FrameNumberStatus.Status;
                    string extras = "";
                    foreach(FrameNumberExtra ext in result.FrameNumberExtras)
                    {
                        if (extras != "")
                            extras += ", ";
                        string thisExtra = "";
                        if (ext.ExtraBrand != "")
                            thisExtra = thisExtra + " ";
                        if (ext.ExtraModel != "")
                            thisExtra = ext.ExtraModel + " ";
                        if (ext.ExtraType != "")
                            thisExtra = ext.ExtraType;
                        extras += thisExtra.Trim();
                    }
                    Extras = extras;
                    DocumentCount = result.FrameNumberDocuments.Count;

                    List<Message> messages = await DataStore.GetMessages(user, token, uuid);
                    MessageCount = messages.Count;

                    IsBusy = false;

                }
                catch (Exception e)
                {
                    AnalyticsService.TrackError(e, new Dictionary<string, string>
                    {
                        { "Method", "BikeDetailViewModel..LookupBikeInfo" }
                    });
                }
                
            }
            IsBusy = false;
        }

        public override void Init()
        {

        }

        #region Public properties

       

        #endregion



    }
}
