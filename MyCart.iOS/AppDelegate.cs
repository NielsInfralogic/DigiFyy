using System;
using System.Collections.Generic;
using Syncfusion.XForms.iOS.Expander;
using Syncfusion.SfRotator.XForms.iOS;
using Syncfusion.XForms.iOS.BadgeView;
using Syncfusion.XForms.iOS.ComboBox;
using Syncfusion.XForms.iOS.Core;
using Syncfusion.XForms.iOS.Graphics;
using Syncfusion.SfRating.XForms.iOS;
using Syncfusion.ListView.XForms.iOS;
using FFImageLoading.Forms.Platform;
using Syncfusion.XForms.iOS.Border;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.SfPdfViewer.iOS;
using Syncfusion.SfPdfViewer.XForms.iOS;
using Syncfusion.SfRangeSlider.XForms.iOS;
using Foundation;
using UIKit;
using CoreNFC;
using CoreFoundation;
using Xamarin.Forms;
using System.IO;
using DigiFyy.iOS.NFC;
using System.Resources;
using DigiFyy.Helpers;
using Plugin.Multilingual;
using System.Reflection;
using System.Linq;
using DigiFyy.Models;
using DigiFyy.Models.NFC;

namespace DigiFyy.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, INFCTagReaderSessionDelegate, INFCNdefReaderSessionDelegate
    {
        private static AppDelegate appDelegate;
        bool _customInvalidation = false;


        public static AppDelegate GetInstance()
        {
            return appDelegate;
        }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjMyNTM4QDMxMzgyZTMxMmUzMGk5SnZMR3JhaWRBSzY0cEFCaDhQVGI0WDQ2ZDcrbTVYQVJWMmRyR0dybjA9");

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();

            SfExpanderRenderer.Init();
            SfCheckBoxRenderer.Init();
            Core.Init();
            SfSegmentedControlRenderer.Init();
            SfRotatorRenderer.Init();
            SfRatingRenderer.Init();
            SfComboBoxRenderer.Init();
            SfListViewRenderer.Init();
            CachedImageRenderer.Init();
            SfBadgeViewRenderer.Init();
            SfGradientViewRenderer.Init();
            SfBorderRenderer.Init();
            SfButtonRenderer.Init();
            SfPdfDocumentViewRenderer.Init();
            SfRangeSliderRenderer.Init();
            Xamarin.FormsMaps.Init();

            LoadApplication(new App());

            appDelegate = this;

            return base.FinishedLaunching(app, options);
        }

        List<NFCNdefMessage> DetectedMessages = new List<NFCNdefMessage> { };
        NFCNdefReaderSession NdefSession = null;
        NFCTagReaderSession NfcSession = null;
        string CellIdentifier = "reuseIdentifier";

        private bool IsiOS13ornewer()
        {
            var splitted = UIDevice.CurrentDevice.SystemVersion?.Split('.');
            if (splitted != null && splitted.Length > 0 && int.TryParse(splitted[0], out var majorVersion))
                return majorVersion >= 13;

            return false;
        }
        public void StartListening()
        {
            ObjCRuntime.Class.ThrowOnInitFailure = false;

            _customInvalidation = false;


            bool ios13 = IsiOS13ornewer();

            // TEST:
            ios13 = false;

            var resmgr = new ResourceManager("DigiFyy.Resources.AppResources", typeof(TranslateExtension).GetTypeInfo().Assembly);
            var ci = CrossMultilingual.Current.CurrentCultureInfo;

            if (ios13)
            {
             
                NfcSession = new NFCTagReaderSession(NFCPollingOption.Iso14443 | NFCPollingOption.Iso15693, this, DispatchQueue.CurrentQueue);
                if (NfcSession != null)
                {
                    NfcSession.AlertMessage = resmgr.GetString("PointPhoneOnTag", ci);
                    NfcSession.BeginSession();
                };
            }
            else
            {
                if (NFCNdefReaderSession.ReadingAvailable)
                {
                    NdefSession = new NFCNdefReaderSession(this, DispatchQueue.CurrentQueue, true);
                    if (NdefSession != null)
                    {
                        NdefSession.AlertMessage = resmgr.GetString("PointPhoneOnTag", ci);
                        NdefSession.BeginSession();
                    };
                }

            }
        }

        #region NFCNDEFReaderSessionDelegate



        /// <summary>
        /// 
        /// Event raised when NFC tags are detected
        /// </summary>
        /// <param name="session">iOS <see cref="NFCTagReaderSession"/></param>
        /// <param name="tags">Array of iOS <see cref="INFCTag"/></param>
        public  void DidDetectTags(NFCTagReaderSession session, INFCTag[] tags)
        {
            _customInvalidation = false;
            INFCTag _tag = tags.First();
            TagRead tagRead = new TagRead();

            string connectionError = string.Empty;
            session.ConnectTo(_tag, (error) =>
            {
                if (error != null)
                {
                    connectionError = error.LocalizedDescription;
                    tagRead.ErrorMessage = connectionError;
                    MessagingCenter.Send<App, TagRead>((App)Xamarin.Forms.Application.Current, "Tag", tagRead);
                    Invalidate(session, connectionError);
                    return;
                }
                
                var ndefTag = NFCHelpers.GetNdefTag(_tag);

                if (ndefTag == null)
                {
                    connectionError = "Tag is not NDEF compliant";
                    tagRead.ErrorMessage = connectionError;
                    MessagingCenter.Send<App, TagRead>((App)Xamarin.Forms.Application.Current, "Tag", tagRead);
                    Invalidate(session, connectionError);
                    return;
                }
                string technology = "";
                var identifier1 = NFCHelpers.GetTagIdentifier(ndefTag, ref technology);
                tagRead.Model = technology;
        
                ndefTag.ReadNdef((message, error1) =>
                {
                    if (error1 != null)
                    {
                        connectionError = "Read error. Please try again";
                        MessagingCenter.Send<App, TagRead>((App)Xamarin.Forms.Application.Current, "Tag", tagRead);
                        Invalidate(session, connectionError);
                     	return;
                    }

                    if (message == null)
                    {
                        tagRead.Data = "";
                    //	Invalidate(session, UIMessages.NFCErrorEmptyTag);
                    //	return;
                    }

                    //session.AlertMessage = "Tag Read Success";

                    if (message != null)
                    {
                        List<string> textMessages = NFCHelpers.GetRecords(message.Records);
                        foreach (string s in textMessages)
                        {
                            if (tagRead.Data != "")
                                tagRead.Data += "\n";
                            tagRead.Data += s;
                        }
                    }
                    MessagingCenter.Send<App, TagRead>((App)Xamarin.Forms.Application.Current, "Tag", tagRead);
                    Invalidate(session); // normal termination of session.
                });
            });
        }


       
        // NDEF Version
        public void DidDetect(NFCNdefReaderSession session, NFCNdefMessage[] messages)
        {
            _customInvalidation = false;
            TagRead tagRead = new TagRead();
            if (messages == null)
            {
                InvalidateNdef(session, "NFC Data tags not found");
                return;
            }

           foreach (NFCNdefMessage msg in messages)
           {
              
                foreach (NFCNdefPayload payload in msg.Records)
                {
                    NSString typeString = payload.Type.ToString(NSStringEncoding.UTF8);
                    NSString identifierString = payload.Identifier.ToString(NSStringEncoding.UTF8);
                    var payloadLength = (int)payload.Payload.Length;
                    
                    switch (payload.TypeNameFormat)
                    {
                        case NFCTypeNameFormat.NFCWellKnown:

                            if (payloadLength < 1)
                                return;

                            if (typeString == "T")
                            {

                                NdefMessageParser.ParseTextPayload(payload.Payload.ToArray(), payload.Payload.Length, out string langCode, out string text);
                                tagRead.Data = text;

                            }
                            else if (typeString == "U")
                            {
                                NdefMessageParser.ParseUriPayload(payload.Payload.ToArray(), payload.Payload.Length, out string text);
                                tagRead.Data = text;

                            }
                            else if (typeString == "Sp")
                            {

                                int length = (int)payload.Payload.Length;
                                byte[] bytes = payload.Payload.ToArray();
                                int index = 0;

                                while (true)
                                {
                                    var statusByte = payload.Payload[index++];
                                    var headerIsMessageBegin = statusByte & 0x80;
                                    var headerIsMessageEnd = statusByte & 0x40;
                                    var headerIsChunkedUp = statusByte & 0x20;
                                    var headerIsShortRecord = statusByte & 0x10;
                                    var headerIsIdentifierPresent = statusByte & 0x08;
                                    var headerTypeNameFormatCode = statusByte & 0x07;
                                    int headerPayloadLength = 0;
                                    byte headerIdentifier = 0;

                                    if (index + 1 > payloadLength)
                                        break;

                                    var typeLength = payload.Payload[index++];

                                    if ((headerIsShortRecord != 0 && (index + 1) > payloadLength) || (headerIsShortRecord == 0 && index + 4 > payloadLength))
                                        break;

                                    if (headerIsShortRecord != 0)
                                    {
                                        headerPayloadLength = payload.Payload[index];
                                        index += 1;
                                    }
                                    else
                                    {
                                        byte[] b = new byte[4];
                                        Array.Copy(payload.Payload.ToArray(), index, b, 0, 4);
                                        headerPayloadLength = BitConverter.ToInt32(b, 0);

                                        index += 4;
                                    }

                                    int identifierLength = 0;

                                    if (headerIsIdentifierPresent != 0)
                                    {
                                        if (index + 1 > payloadLength)
                                            break;

                                        identifierLength = payload.Payload[index++];
                                    }

                                    if (index + typeLength > payloadLength)
                                        break;

                                    NSString headerType = (Foundation.NSString)NSData.FromStream(new MemoryStream(bytes, index, typeLength)).ToString(NSStringEncoding.UTF8);
                                    if (string.IsNullOrEmpty(headerType))
                                        break;

                                    index += typeLength;

                                    if (identifierLength > 0)
                                    {
                                        if (index + 1 > payloadLength)
                                            break;

                                        headerIdentifier = payload.Payload[index];
                                        index += identifierLength;

                                    }

                                    var headerPayloadOffset = index;

                                    length -= headerPayloadOffset;
                                    byte[] newPayloadArray = new byte[headerPayloadLength];
                                    Array.Copy(payload.Payload.ToArray(), headerPayloadOffset, newPayloadArray, 0, headerPayloadLength);

                                    if (headerType == "U")
                                    {
                                        NdefMessageParser.ParseUriPayload(newPayloadArray, (System.nuint)headerPayloadLength, out string text);
                                        tagRead.Data = text;

                                    }
                                    else if (headerType == "T")
                                    {
                                        NdefMessageParser.ParseTextPayload(newPayloadArray, (System.nuint)headerPayloadLength, out string langCode, out string text);
                                        tagRead.Data = text;
                                    }

                                    length -= headerPayloadLength;
                                    if (headerIsMessageEnd != 0 || length == 0)
                                        break;

                                    index += headerPayloadLength;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }


                MessagingCenter.Send<App, TagRead>((App)Xamarin.Forms.Application.Current, "Tag", tagRead);
            }

            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                    //this.TableView.ReloadData();
                });
        }


        /// <summary>
        /// Stops tags detection
        /// </summary>
        //public void StopListening() => Session?.InvalidateSession();

        public void StopListening()
        {
            if (NdefSession != null)
                NdefSession?.InvalidateSession();
            else
                NfcSession?.InvalidateSession();

        }

        void InvalidateNdef(NFCNdefReaderSession session, string message = null)
        {
            _customInvalidation = true;
            if (string.IsNullOrWhiteSpace(message))
                session.InvalidateSession();
            else
                session.InvalidateSession(message);
        }

        void Invalidate(NFCTagReaderSession session, string message = null)
        {
            _customInvalidation = true;
            if (string.IsNullOrWhiteSpace(message))
                session.InvalidateSession();
            else
                session.InvalidateSession(message);
        }


        UIViewController GetCurrentController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;
            return vc;
        }

        // Native NCF version
        public void DidInvalidate(NFCTagReaderSession session, NSError error)
        {
            var readerError = (NFCReaderError)(long)error.Code;
            if (readerError != NFCReaderError.ReaderSessionInvalidationErrorFirstNDEFTagRead &&
                readerError != NFCReaderError.ReaderSessionInvalidationErrorUserCanceled)
            {
                InvokeOnMainThread(() =>
                {
                    var alertController = UIAlertController.Create("Session Invalidated", error.LocalizedDescription, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    DispatchQueue.MainQueue.DispatchAsync(() =>
                    {
                        GetCurrentController().PresentViewController(alertController, true, null);
                    });
                });
            }
            else if (readerError == NFCReaderError.ReaderSessionInvalidationErrorUserCanceled)
            {
                TagRead tagRead = new TagRead() { Data = "(Empty)", Model = "Cancelled", SerialNumber = "Cancelled", ErrorMessage = "" }; 

                MessagingCenter.Send<App, TagRead>((App)Xamarin.Forms.Application.Current, "Tag", tagRead);
            }
        }


        // NDEF version
        public void DidInvalidate(NFCNdefReaderSession session, NSError error)
        {

            var readerError = (NFCReaderError)(long)error.Code;

            if (readerError != NFCReaderError.ReaderSessionInvalidationErrorFirstNDEFTagRead &&
                readerError != NFCReaderError.ReaderSessionInvalidationErrorUserCanceled)
            {
                InvokeOnMainThread(() =>
                {
                    var alertController = UIAlertController.Create("Session Invalidated", error.LocalizedDescription, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    DispatchQueue.MainQueue.DispatchAsync(() =>
                    {
                        GetCurrentController().PresentViewController(alertController, true, null);
                    });
                });

            }
            else if (readerError == NFCReaderError.ReaderSessionInvalidationErrorUserCanceled && !_customInvalidation)
            {
                TagRead tagRead = new TagRead() { Data = "(Empty)", Model = "Cancelled", SerialNumber = "Cancelled", ErrorMessage = "" };
                MessagingCenter.Send<App, TagRead>((App)Xamarin.Forms.Application.Current, "Tag", tagRead);
            }

            //OniOSReadingSessionCancelled?.Invoke(null, EventArgs.Empty);

        }


        #endregion






    }

}
