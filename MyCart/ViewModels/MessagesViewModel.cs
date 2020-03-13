using DigiFyy.Models.AWS;
using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    public class MessagesViewModel : ViewModelBase
    {
        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get => messages;
            set
            {
                messages = value;
                SetProperty(ref messages, value);
            }
        }
        private Command<object> itemSelectedCommand;

        public MessagesViewModel(INavigationService navigationService, IAnalyticsService analyticsService) : base(navigationService, analyticsService)
        {
            LookupMessageInfo();
        }

        private async void LookupMessageInfo()
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
                    List<Message> messagesFromAPI = await DataStore.GetMessages(user, token, uuid);
                    if (messagesFromAPI != null)
                        foreach (Message m in messagesFromAPI)
                            messages.Add(m);
                    IsBusy = false;
                }
                catch (Exception e)
                {
                    AnalyticsService.TrackError(e, new Dictionary<string, string>
                    {
                        { "Method", "DMessagesViewModel..LookupBikeInfo" }
                    });
                }

            }
            IsBusy = false;
        }

        /// <summary>
        /// Gets the command that will be executed when an item is selected.
        /// </summary>
        public Command<object> ItemSelectedCommand
        {
            get
            {
                return this.itemSelectedCommand ?? (this.itemSelectedCommand = new Command<object>(this.NavigateToNextPage));
            }
        }


        /// <summary>
        /// Invoked when an item is selected from the Songs play list.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private void NavigateToNextPage(object selectedItem)
        {
            // Do something
        }

    }
}