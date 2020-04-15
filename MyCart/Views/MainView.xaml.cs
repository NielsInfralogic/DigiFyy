using DigiFyy.Services;
using DigiFyy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace DigiFyy.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : TabbedPage
    {
        private int previousPage = -1;
        public MainView()
        {
            InitializeComponent();
            previousPage = -1;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<MainViewModel, int>(this, MessageKeys.ChangeTab, (sender, arg) =>
            {
                switch (arg)
                {
                    case 0:
                        CurrentPage = HomePage;
                        break;
                    case 1:
                        CurrentPage = ScanPage;
                        break;
                    case 2:
                        CurrentPage = BikeDetailPage;
                        break;
                    case 3:
                      CurrentPage = MessagesPage;
                        break;
                    case 4:
                       CurrentPage = SettingsPage;
                        break;
                }
            });
         
        }

        protected override async void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            if (CurrentPage is HomeView)
            {
                if (previousPage != 0)
                    await (HomePage.BindingContext as ViewModelBase).InitializeAsync(null);
                previousPage = 0;
            }
            else if (CurrentPage is ScanView)
            {
            
                if (previousPage != 1)
                    await (ScanPage.BindingContext as ViewModelBase).InitializeAsync(null);
                previousPage = 1;
            }
            else if (CurrentPage is BikeDetailView)
            {
          
                if (previousPage != 2)
                    await (BikeDetailPage.BindingContext as ViewModelBase).InitializeAsync(true);
                previousPage = 2;
            }
            else if (CurrentPage is MessagesView)
            {
          
                  if (previousPage != 3)
                      await (MessagesPage.BindingContext as ViewModelBase).InitializeAsync(true);
                previousPage = 3;
            }

            else if (CurrentPage is SettingsView)
            {
             
                 if (previousPage != 4)
                   await (SettingsPage.BindingContext as ViewModelBase).InitializeAsync(null);
                previousPage = 4;
            }
        }
    }
}