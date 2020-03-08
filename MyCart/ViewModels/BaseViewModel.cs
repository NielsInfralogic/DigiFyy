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
        public IDigifyyDataService DataStore;

        protected INavigationService NavigationService { get; private set; }
        protected IAnalyticsService AnalyticsService { get; private set; }

        public ViewModelBase(INavigationService navigationService, IAnalyticsService analyticsService)
        {
            NavigationService = navigationService;
            AnalyticsService = analyticsService;

            DataStore = new AWSDigifyyDataService(analyticsService);

        }
        

        public virtual void Init()
        {
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
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

       

    /*    protected async Task<Bike> GetBikeInfo()
        {
            var email = Preferences.Get("email", "");
            //if (string.IsNullOrEmpty(email))
            //{
            //    return null;
            //}

            var result = await DataStore.GetInfo(.GetItemsAsync();

            var cartList = new ObservableCollection<Product>();

            foreach (var productID in result)
            {
                var product = AllProducts.Where(item => item.Id.ToString() == productID).FirstOrDefault();
                if (product != null && !cartList.Contains(product))
                {
                    cartList.Add(product);
                }
                //TODO: Increase order count.
                continue;
            }
            return cartList;
        }*/

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

    public class ViewModelBase<TParameter> : ViewModelBase
    {
        protected ViewModelBase(INavigationService navigationService, IAnalyticsService analyticsService)
            : base(navigationService, analyticsService)
        {
        }

        public override void Init()
        {
            Init(default(TParameter));
        }

        public virtual void Init(TParameter parameter)
        {
        }
    }
}
