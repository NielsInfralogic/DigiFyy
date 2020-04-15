using DigiFyy.Models.AWS;
using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    public class MessagesViewModel : ViewModelBase
    {
        private ObservableCollection<Message> messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
        {
            get => messages;
            set
            {
                messages = value;
                SetProperty(ref messages, value);
            }
        }

        private string badgeCount = "";
        public string BadgeCount
        {
            get
            {
                return badgeCount;
            }
            set
            {
                SetProperty(ref badgeCount, value);
            }
        }

        public Helpers.Command BackButtonCommand { get; set; }

        public MessagesViewModel()
        {
            this.BackButtonCommand = new Helpers.Command(this.BackButtonClicked);

            MessagingCenter.Subscribe<MessagesViewModel, int>(this, MessageKeys.UpdateMessageCount, (sender, arg) =>
            {
                BadgeCount = arg > 0 ? arg.ToString() : "";
            });
        }

        private async void BackButtonClicked(object obj)
        {
            await NavigationService.GoBackAsync();
        }

        // Data from navigation
        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is Boolean)
            {
                if ((Boolean)navigationData)
                    LookupMessageInfo();

            }
        }

        private Command itemSelectedCommand;

        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand
        {
            get { return this.itemSelectedCommand ?? (this.itemSelectedCommand = new Command(this.ItemSelected)); }
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
                    {
                        messages.Clear();
                        foreach (Message m in messagesFromAPI)
                            messages.Add(m);
                    }
                    BadgeCount = messages.Count > 0 ? messages.Count.ToString() : "";
                    IsBusy = false;
                }
                catch (Exception e)
                {
                    AnalyticsService.TrackError(e, new Dictionary<string, string>
                    {
                        { "Method", "MessagesViewModel.LookupMessageInfo" }
                    });
                }

            }
            IsBusy = false;
        }

        private async void ItemSelected(object attachedObject)
        {
            if (attachedObject == null)
                return;

            if (attachedObject is Message message)
            {
                // await NavigationService.NavigateToAsync<PdfViewerViewModel>(message);
                await DialogService.Show(message.SenderName, message.MessageText, "Ok");
            }

        }


    }
}