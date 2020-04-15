using DigiFyy.Models.AWS;
using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace DigiFyy.ViewModels
{
    public class AddExtraViewModel : ViewModelBase
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


        public Helpers.Command BackButtonCommand { get; set; }

        public AddExtraViewModel()
        {
            this.BackButtonCommand = new Helpers.Command(this.BackButtonClicked);

        }

        private async void BackButtonClicked(object obj)
        {
            await NavigationService.GoBackAsync();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData == null)
                return;
            if (navigationData is List<Models.AWS.FrameNumberExtra>)
                (navigationData as List<Models.AWS.FrameNumberExtra>).ForEach(p => _extras.Add(p));
            Extras = _extras;
        }

    }
}
