using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DigiFyy.Services;
using DigiFyy.Models;
using DigiFyy.DataService;


namespace DigiFyy.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        // Singletons!
        public IDigifyyDataService DataStore; 
        protected INavigationService NavigationService;
        protected IAnalyticsService AnalyticsService;
        protected readonly IDialogService DialogService;

        public ViewModelBase()
        {
            DialogService = ViewModelLocator.Resolve<IDialogService>();
            NavigationService = ViewModelLocator.Resolve<INavigationService>();
            AnalyticsService = ViewModelLocator.Resolve<IAnalyticsService>();
            DataStore = ViewModelLocator.Resolve<IDigifyyDataService>();
        }
        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

    
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null /*, Func<T, T, bool>? validateValue = null*/)
        {
            //if value didn't change
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            //if value changed but didn't validate
           // if (validateValue != null && !validateValue(backingStore, value))
            //    return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
 
