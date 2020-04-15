using DigiFyy.DataService;
using DigiFyy.Helpers;
using DigiFyy.Models.AWS;
using DigiFyy.Services;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    public class AddPictureViewModel : ViewModelBase
    {

        private string fileName;
        public string FileName
        {
            get
            {
                return this.fileName;
            }

            set
            {
                SetProperty(ref fileName, value);
            }
        }

        private string instructions;
        public string Instructions
        {
            get
            {
                return this.instructions;
            }

            set
            {
                SetProperty(ref instructions, value);
            }
        }



        private int imageType = 0;

        public AddPictureViewModel()
        {
            this.UsePhotoCommand = new Helpers.Command(this.UsePhotoClicked);
            this.BackButtonCommand = new Helpers.Command(this.BackButtonClicked);

        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData == null)
                return;
            if (navigationData is int)
                imageType = (int)navigationData;

            var resmgr = new ResourceManager("DigiFyy.Resources.AppResources", typeof(TranslateExtension).GetTypeInfo().Assembly);
            var ci = CrossMultilingual.Current.CurrentCultureInfo;

                
            if (imageType == (int)ImageTypes.Invoice)
            {

                Instructions = resmgr.GetString("ToRegisterTakePhoto", ci);
            }
            else
                Instructions = resmgr.GetString("TakePhotoOrSelect", ci);
        }

        public Helpers.Command UsePhotoCommand { get; set; }
        public Helpers.Command BackButtonCommand { get; set; }
        
        private async void BackButtonClicked(object obj)
        {
            await NavigationService.GoBackAsync();
        }
        private async void UsePhotoClicked(object obj)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            string url = await Helpers.Utils.UploadFileS3(FileName);
            if (url != "")
            {
                var bikeDetailViewModel = ViewModelLocator.Resolve<BikeDetailViewModel>();
                string uuid = Preferences.Get("UUID", "");
                string user = Preferences.Get("Email", "");
                string token = Preferences.Get("Token", "");


                if (imageType != (int)ImageTypes.Invoice)
                {
                    FrameNumberImage frameNumberImage = new FrameNumberImage() { ImageTitle = "Receipt", ImageType = imageType, ImageUrl = url };
                    var result = await DataStore.RegisterImage(user, token, uuid, frameNumberImage, false);
                    if (result != null && result.ImageUrl != "")
                    {
                        Preferences.Set("ProfileImage", result.ImageUrl);
                        if (bikeDetailViewModel != null)
                            MessagingCenter.Send(bikeDetailViewModel, MessageKeys.UpdateState, 0);
                        await NavigationService.GoBackAsync();
                    }
                }
                else // if (imageType == (int)ImageTypes.Invoice)
                {
                    // Upload invoice picture as document.
                 
                    FrameNumberDocument frameNumberDocument = new FrameNumberDocument() { DocumentTitle = "Receipt", DocumentType = imageType, DocumentUrl = url };
                    var result = await DataStore.RegisterDocument(user, token, uuid, frameNumberDocument, false);
                    if (result != null && result.DocumentUrl != "")
                    {
                        
                        FrameNumberStatus fns = new FrameNumberStatus()
                        {
                            LastUpdateTime = DateTime.Now,
                            Latitude = 0,
                            Longitude = 0,
                            LastUpdateClientID = DeviceInfo.Name,
                            Status = (int)FrameNumberStatusType.Registered
                        };

                        // Now set status to 'registered'

                        if (await DataStore.UpdateStatus(user, token, uuid, fns))
                        {
                            if (Preferences.Get("IsLoggedIn", "0") == "1")
                            {
                                Preferences.Set("RegisteredToBike", "1");
                            }

                        }
                    }
                    IsBusy = false;

                    // Update state
                    if (bikeDetailViewModel != null)
                        MessagingCenter.Send(bikeDetailViewModel, MessageKeys.UpdateState, 1);

                    //await NavigationService.GoBackAsync();
                    await NavigationService.NavigateToAsync<MainViewModel>(2);
                }
            }
            IsBusy = false;
        }
    }
}
