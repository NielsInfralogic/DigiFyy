using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigiFyy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPictureView : ContentPage
    {
        public AddPictureView()
        {
            InitializeComponent();
            UsePhoto.IsVisible = false;
        }

   

        private async void TakePhoto_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = null;
            try
            {
                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Test",
                    SaveToAlbum = true,
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.Medium,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Front
                });
            }
            catch
            {
                await DisplayAlert("No permission", ":( Permission required", "OK");
            }

            if (file == null)
                return;

            //await DisplayAlert("File Location", file.Path, "OK");
          
            PhotoImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                FileName.Text = file.Path;
                file.Dispose();
                return stream;
            });
            PhotoImage.Aspect = Aspect.AspectFit;
            UsePhoto.IsVisible = true;

        }


        private async void PickPhoto_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }

            MediaFile file = null;
            try
            {

                file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
            });
            }
            catch 
            {
                await DisplayAlert("No permission", ":( Permission required", "OK");
            }

            if (file == null) 
                return;

            
            PhotoImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                FileName.Text = file.Path;
                file.Dispose();
                return stream;
            });

            PhotoImage.Aspect = Aspect.AspectFit;

            UsePhoto.IsVisible = true;
        }
    }
}