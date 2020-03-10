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

        private  List<Message> messages;

        #endregion

        #region Constructor

        public BikeDetailViewModel(INavigationService navigationService, IAnalyticsService analyticsService) : base(navigationService, analyticsService)
        {
          // BikeInfo = new Bike();
            //       if (uuid != "")
            //        Preferences.Set("UUID", uuid);

            LookupBikeInfo();
        }
        #endregion

        Command _refreshCommand;
        public Command RefreshCommand => _refreshCommand ?? (_refreshCommand = new Command(LookupBikeInfo));


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
                    IsBusy = false;

                    Brand = result.FrameNumber.Manufacturer;
                    Model = result.FrameNumber.Model;
                    UUID = result.FrameNumber.UID;
                    this.Frame = result.FrameNumber.Frame;
                    Status = result.FrameNumberStatus.Status;
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

       /* public override void Init(Bike parameter)
        {
            AnalyticsService.TrackEvent("Bike Detail Page", new Dictionary<string, string>
                {
                   { "UUID", parameter.FrameNumber.UID }
                });

            BikeInfo = parameter;
        }*/

        #region Public properties



        public List<Message> Messages
        {
            get
            {
                return this.messages;
            }

            set
            {
                SetProperty(ref messages, value);
            }
        }

        #endregion

        #region Commands


        #endregion
    }
}
