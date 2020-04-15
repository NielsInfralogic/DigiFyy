using DigiFyy.Models.AWS;
using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DigiFyy.ViewModels
{
    public class ShowPartsViewModel : ViewModelBase
    {
        private ObservableCollection<FrameNumberExtra> _extras = new ObservableCollection<FrameNumberExtra>();
        public ObservableCollection<FrameNumberExtra> Extras
        {
            get => _extras;
            set
            {
                _extras = value;
                SetProperty(ref _extras, value);
            }
        }


        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                SetProperty(ref _imagePath, value);
            }
        }


        private string _brandModel;
        public string BrandModel
        {
            get { return _brandModel; }
            set
            {
                SetProperty(ref _brandModel, value);
            }
        }


        public Helpers.Command BackButtonCommand { get; set; }

        public ShowPartsViewModel()
        {
            this.BackButtonCommand = new Helpers.Command(this.BackButtonClicked);
        }

        private async void BackButtonClicked(object obj)
        {
            await NavigationService.GoBackAsync();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            List<Models.AWS.FrameNumberExtra> tmpList = new List<FrameNumberExtra>();
            if (navigationData == null)
                return;
            if (navigationData is List<Models.AWS.FrameNumberExtra>)
                (navigationData as List<Models.AWS.FrameNumberExtra>).ForEach(p => tmpList.Add(p));

            string previousType = "";
            for(int i=0;i< tmpList.Count; i++)
            {
                if (previousType != "" && tmpList[i].ExtraType == previousType)
                    tmpList[i].ExtraType = "";
                else
                    previousType = tmpList[i].ExtraType;
            }
            tmpList.ForEach(p => Extras.Add(p));

            ImagePath = Preferences.Get("ProfileImage", "digifyy_bikelogo.png");
            BrandModel = Preferences.Get("BrandModel", "");
        }

    }
}
