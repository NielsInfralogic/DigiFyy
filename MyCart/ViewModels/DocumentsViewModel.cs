using DigiFyy.Models.AWS;
using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    public class DocumentsViewModel : ViewModelBase
    {
        public DocumentsViewModel(INavigationService navigationService, IAnalyticsService analyticsService) : base(navigationService, analyticsService)
        {           
            LookupDocumentsInfo();
        }

        private async void LookupDocumentsInfo()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            string uuid = Preferences.Get("UUID", "");
            string user = Preferences.Get("Email", "");
            string token = Preferences.Get("Token", "");

            if (token != "" && user != "" && uuid != "")
            {
                try
                {

                  /*  UIDInfo result = await DataStore.GetInfo(user, token, uuid);
                    IsBusy = false;

                    Brand = result.FrameNumber.Manufacturer;
                    Model = result.FrameNumber.Model;
                    UUID = result.FrameNumber.UID;
                    this.Frame = result.FrameNumber.Frame;
                    Status = result.FrameNumberStatus.Status;*/
                }
                catch (Exception e)
                {
                    AnalyticsService.TrackError(e, new Dictionary<string, string>
                    {
                        { "Method", "DocumentsViewModel..LookupBikeInfo" }
                    });
                }

            }
            IsBusy = false;
        }

        private Command<object> itemTappedCommand;


        public Command<object> ItemTappedCommand
        {
            get
            {
                return this.itemTappedCommand ?? (this.itemTappedCommand = new Command<object>(this.NavigateToNextPage));
            }
        }


        public ObservableCollection<FrameNumberDocument> DocumentsList { get; set; }

        /// <summary>
        /// Invoked when an item is selected from the documents list.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private void NavigateToNextPage(object selectedItem)
        {
            // Do something
        }
    }
}
