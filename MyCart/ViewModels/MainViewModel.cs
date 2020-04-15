using DigiFyy.Helpers;
using DigiFyy.Models;
using DigiFyy.Services;
using DigiFyy.ViewModels;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
    
        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            if (navigationData is TabParameter)
            {
                // Change selected application tab
                var tabIndex = ((TabParameter)navigationData).TabIndex;
                MessagingCenter.Send(this, MessageKeys.ChangeTab, tabIndex);
            }

            return base.InitializeAsync(navigationData);
        }

        /// <summary>
        /// Initializes a new instance for the <see cref="MainViewModel" /> class.
        /// </summary>
        public MainViewModel()
        {
    
        }
    
    }
}
