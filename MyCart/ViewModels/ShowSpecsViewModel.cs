using DigiFyy.Models.AWS;
using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DigiFyy.ViewModels
{
    public class ShowSpecsViewModel : ViewModelBase
    {
        private ObservableCollection<ManufacturerSpec> _specs = new ObservableCollection<ManufacturerSpec>();
        public ObservableCollection<ManufacturerSpec> Specs
        {
            get => _specs;
            set
            {
                _specs = value;
                SetProperty(ref _specs, value);
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

        public ShowSpecsViewModel()
        {
            this.BackButtonCommand = new Helpers.Command(this.BackButtonClicked);
        }

        private async void BackButtonClicked(object obj)
        {
            await NavigationService.GoBackAsync();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            LookupSpecs();
            ImagePath = "bikespecs.png"; // Manufacturer specific image?
        }

        private async void LookupSpecs()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            string uuid = Preferences.Get("UUID", "");
            string user = Preferences.Get("Email", "");
            string token = Preferences.Get("Token", "");

            if (uuid != "")
            {
                try
                {
                    List<ManufacturerSpec> specsFromAPI = await DataStore.GetSpecs(user, token, uuid);
                    if (specsFromAPI != null)
                        foreach (ManufacturerSpec m in specsFromAPI)
                            _specs.Add(new ManufacturerSpec() { SpecPartType = m.SpecPartDetails, SpecPartName = m.SpecPartType, SpecPartDetails = m.SpecPartName });
                    Specs = _specs;
                    BrandModel = Preferences.Get("BrandModel", "");
                    IsBusy = false;
                }
                catch (Exception e)
                {
                    AnalyticsService.TrackError(e, new Dictionary<string, string>
                    {
                        { "Method", "howSpecsViewModel.LookupSpecs" }
                    });
                }

            }
            IsBusy = false;
        }


    }
}
