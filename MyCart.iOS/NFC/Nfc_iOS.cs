using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CoreNFC;
using DigiFyy.DataService;
using DigiFyy.iOS;
using DigiFyy.Models.NFC;
using Foundation;

using UIKit;
using Xamarin.Forms;

[assembly: Dependency (typeof(Nfc_iOS))]
namespace DigiFyy.iOS
{
    public class Nfc_iOS : INfc
    {
        public void StartSession()
        {
            AppDelegate.GetInstance().StartListening();
        }

        public void StopSession()
        {
            AppDelegate.GetInstance().StopListening();
        }
    }

}