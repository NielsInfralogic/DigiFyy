using FFImageLoading.Forms.Platform;
using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Android.Content;
using Android.Nfc;
using Plugin.NFC;
using DigiFyy.Data;

namespace DigiFyy.Droid
{
    [Activity(Label = "DigiFyy", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { NfcAdapter.ActionNdefDiscovered }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = Constants.MIME_TYPE)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {


        protected override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjAzMTIzQDMxMzcyZTM0MmUzMFBqOXYxWmNWc1VNVHBFUjZGQ1VMZGdkRGFKR3ZxUWNFbnFaTUFHVUtYenM9");

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            CrossNFC.Init(this);
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            CachedImageRenderer.Init(true);

            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);

            LoadApplication(new App());          
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            CrossNFC.OnNewIntent(intent);
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

    }
}