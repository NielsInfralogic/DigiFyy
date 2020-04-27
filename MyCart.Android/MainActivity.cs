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

using Android;
using System.Collections.Generic;
using DigiFyy.Droid.NFC;
using Xamarin.Forms;
using Android.Nfc.Tech;
using DigiFyy.Models;
using DigiFyy.Models.NFC;

namespace DigiFyy.Droid
{
    [Activity(Label = "DigiFyy", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { NfcAdapter.ActionNdefDiscovered, NfcAdapter.ActionTagDiscovered, Intent.CategoryDefault })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private NfcAdapter _nfcAdapter;

        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjMyNTM4QDMxMzgyZTMxMmUzMGk5SnZMR3JhaWRBSzY0cEFCaDhQVGI0WDQ2ZDcrbTVYQVJWMmRyR0dybjA9");

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

          //  CrossNFC.Init(this);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            CachedImageRenderer.Init(true);

            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);

            LoadApplication(new App());
            
            _nfcAdapter = NfcAdapter.GetDefaultAdapter(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            //base.OnNewIntent(intent);
            // CrossNFC.OnNewIntent(intent);

            string manuf = "";
            string data = "";
            string serialNumber = "";

            TagRead tagRead = new TagRead();


            if (intent.Action == NfcAdapter.ActionTagDiscovered)
            {

                List<string> tags = new List<string>();
                NdefMessage ndefMessage = null;

                var id = intent.GetByteArrayExtra(NfcAdapter.ExtraId);

                if (id != null)
                {
                    serialNumber =  NFCUtils.ByteArrayToHexString(id, ":");
                    /*for (int ii = 0; ii < id.Length; ii++)
                    {
                        if (!string.IsNullOrEmpty(data))
                            serialNumber += "-";
                        serialNumber += id[ii].ToString("X2");
                    }*/
                    tagRead.SerialNumber = serialNumber;
                    tags.Add(serialNumber);

                }
                else
                    tags.Add(null);

                var tag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;
                if (tag != null)
                {
                    manuf = NFCHelpers.GetManufacturer(tag);
                    tags.Add(manuf);
                    tagRead.Model = manuf;

                    if (NFCHelpers.IsNDEFTag(tag))
                    {
                        var ndef = Ndef.Get(tag);

                        if (ndef != null)
                        {
                            if (ndefMessage == null)
                                ndefMessage = ndef.CachedNdefMessage;

                            if (ndefMessage != null)
                            {
                                NdefRecord[] records = ndefMessage.GetRecords();
                                var ndefrecords = NFCHelpers.GetRecords(records);
                                for (var i = 0; i < ndefrecords.Count; i++)
                                {
                                    if (data != "")
                                        data += "\n";
                                    data += ndefrecords[i];
                                }
                                tags.Add(data);
                                tagRead.Data = data;
                            }
                        }
                    }
                  
/*
                    var rawTagMessages = intent.GetParcelableArrayExtra(NfcAdapter.ExtraTag);

                    // First get all the NdefMessage
                    var rawMessages = intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
                    if (rawMessages != null)
                    {

                        // https://medium.com/@ssaurel/create-a-nfc-reader-application-for-android-74cf24f38a6f

                        foreach (var message in rawMessages)
                        {

                            foreach (var r in NdefMessageParser.GetInstance().Parse((NdefMessage)message))
                            {
                                System.Diagnostics.Debug.WriteLine("TAG: " + r.Str());
                                tags.Add(r.Str());
                            }

                        }
                    }*/
                }

                MessagingCenter.Send<App, TagRead>((App)Xamarin.Forms.Application.Current, "Tag", tagRead);

            }
            else if (intent.Action == NfcAdapter.ActionNdefDiscovered)
            {
                System.Diagnostics.Debug.WriteLine("ActionNdefDiscovered");
            }
        }

    
        protected override void OnResume()
        {
            base.OnResume();

            if (_nfcAdapter == null)
            {
                var alert = new AlertDialog.Builder(this).Create();
                alert.SetMessage("NFC is not supported on this device.");
                alert.SetTitle("NFC Unavailable");
                alert.Show();
            }
            else
            {
                var tagDetected = new IntentFilter(NfcAdapter.ActionTagDiscovered);
                var ndefDetected = new IntentFilter(NfcAdapter.ActionNdefDiscovered);
                var techDetected = new IntentFilter(NfcAdapter.ActionTechDiscovered);

                var filters = new[] { ndefDetected, tagDetected, techDetected };

                var intent = new Intent(this, this.GetType()).AddFlags(ActivityFlags.SingleTop);

                var pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);

                _nfcAdapter.EnableForegroundDispatch(this, pendingIntent, filters, null);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();

            if (_nfcAdapter != null)
                _nfcAdapter.DisableForegroundDispatch(this);
        }


    }

}