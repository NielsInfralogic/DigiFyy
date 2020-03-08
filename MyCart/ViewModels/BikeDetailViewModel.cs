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
        Bike _bikeinfo;
        public Bike BikeInfo
        {
            get => _bikeinfo;
            set
            {
                _bikeinfo = value;
                SetProperty(ref _bikeinfo, value);
            }
        }


        private  List<Message> messages;

        #endregion

        #region Constructor

        public BikeDetailViewModel(string uuid, INavigationService navigationService, IAnalyticsService analyticsService) : base(navigationService, analyticsService)
        {

            if (uuid != "")
                Preferences.Set("UUID", uuid);

            LookupBikeInfo();
        }
        #endregion



        private async void LookupBikeInfo()
        {
            string uuid = Preferences.Get("UUID","");
            string user = Preferences.Get("Email","");
            string token = Preferences.Get("Token","");

            if (token != "" && user != "" && uuid != "")
            {
                UIDInfo result = await DataStore.GetInfo(user, token, uuid);
                BikeInfo.FrameNumber = result.FrameNumber;
                BikeInfo.FrameNumberDocuments = result.FrameNumberDocuments;
                BikeInfo.FrameNumberExtras = result.FrameNumberExtras;
                BikeInfo.FrameNumberStatus = result.FrameNumberStatus;          
            }

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
