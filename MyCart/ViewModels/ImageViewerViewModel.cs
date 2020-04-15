using DigiFyy.Helpers;
using DigiFyy.Models.AWS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigiFyy.ViewModels
{
    public class ImageViewerViewModel : ViewModelBase
    {

        public string _imageTitle;
        public string ImageTitle
        {
            get
            {
                return _imageTitle;
            }
            set
            {
                SetProperty(ref _imageTitle, value);
            }
        }


        private string _imageUrl;
        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                SetProperty(ref _imageUrl, value);
            }
        }

        public Helpers.Command BackButtonCommand { get; set; }

        public ImageViewerViewModel()
        {
            this.BackButtonCommand = new Helpers.Command(this.BackButtonClicked);
        }

        private async void BackButtonClicked(object obj)
        {
            await NavigationService.GoBackAsync();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is FrameNumberDocument)
            {
                FrameNumberDocument doc =navigationData as FrameNumberDocument;
                ImageUrl = doc.DocumentUrl;
                ImageTitle = doc.DocumentTitle;
            }
            else if (navigationData is FrameNumberImage)
            {
            FrameNumberImage im = navigationData as FrameNumberImage;
                ImageUrl = im.ImageUrl;
                ImageTitle = im.ImageTitle;
            }

        }
        
    }
}
