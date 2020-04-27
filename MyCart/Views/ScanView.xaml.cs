using Amazon.S3.Model.Internal.MarshallTransformations;
using DigiFyy.DataService;
using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigiFyy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanView : ContentPage
    {
	//	 NFCNdefTypeFormat _type;

		private bool _nfcIsEnabled;
		public bool NfcIsEnabled
		{
			get => _nfcIsEnabled;
			set
			{
				_nfcIsEnabled = value;
				OnPropertyChanged(nameof(NfcIsEnabled));
				OnPropertyChanged(nameof(NfcIsDisabled));
			}
		}
		public bool NfcIsDisabled => !NfcIsEnabled;

		private bool IsInitialized;

		private bool iOSFirstAppear = true;
		public ScanView()
        {
            InitializeComponent();
			IsInitialized = false;
			iOSFirstAppear = true;
			//ScanInfo.IsVisible = false;
		}
		public string TagId { get; set; }

		private List<string> _arg;
		public string ReceivedAt { get; set; }
		protected async override void OnAppearing()
		{
			
			if (IsInitialized)
			{
				if (Device.RuntimePlatform == Device.iOS && iOSFirstAppear == false)
					ScanAgainButton.IsVisible = true;
				return;
			}

		//	UseScanButton.IsEnabled = false;
		//	UseScanButton.IsVisible = false;
			base.OnAppearing();

			
		//	CrossNFC.Current.StopListening();
		//	UnsubscribeEvents();

			// TEST ONLY
			//if (Preferences.Get("UseFakeUUID", "0") == "1")
			//{
			//	UseScanButton.IsEnabled = true;
			//	UseScanButton.IsVisible = true;
			//	ScanInfo.IsVisible = true;
			//	MessageLabel.Text = Constants.FakeUUID;
			//}



			/*
			if (CrossNFC.IsSupported)
			{
				bool nfcOK = true;
				if (!CrossNFC.Current.IsAvailable)
				{
					await ShowAlert("NFC is not available", null);
					nfcOK = false;
				}

				if (nfcOK)
				{
					if (!CrossNFC.Current.IsEnabled)
					{
						await ShowAlert("NFC is disabled", null);
						nfcOK = false;
					}
				}

				if (nfcOK)
				{
					SubscribeEvents();

					if (Device.RuntimePlatform == Device.iOS)
					{
						WaitingForScanLabel.IsVisible = false;
						if (iOSFirstAppear)
						{
							ScanAgainButton.IsVisible = false;
							CrossNFC.Current.StartListening();
						}
						iOSFirstAppear = false;
					}
					else
					{
						// Start NFC tag listening manually
						CrossNFC.Current.StartListening();
						ScanAgainButton.IsVisible = false;

					}
				}
			}*/
			SubscribeEvents();
			IsInitialized = true;
		}

		protected override  bool OnBackButtonPressed()
		{
			UnsubscribeEvents();
			CrossNFC.Current.StopListening();
			return base.OnBackButtonPressed();
		}

		protected  override  void OnDisappearing()
		{
			base.OnDisappearing();
			//UnsubscribeEvents();
			//	if (IsInitialized)
			//return;

//			CrossNFC.Current.StopListening();
		
		}

		void SubscribeEvents()
		{
			return;
			MessagingCenter.Subscribe<App, List<string>>(this, "Tag", (sender, arg) =>
			{
				TagId = arg[0]; // Pos 0 = TagID
				_arg = arg;
				DateTime dateTime = DateTime.Now;
				ReceivedAt = dateTime.ToLongDateString() + "\n" + dateTime.ToLongTimeString();

				//

				ScanInfo.IsVisible = true;
				if (arg.Count > 2)
				{
					MessageLabel.Text = arg[2];
				}
				else
				{
					MessageLabel.Text = "(Empty)";
					//	await ShowAlert("Empty tag", title);
				}

				NFCModelLabel.Text = arg[1];
				SerialNumberLabel.Text = arg[0];
				UseScanButton.IsEnabled = true;
				UseScanButton.IsVisible = true;

				//
			});
		//	CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
		//	CrossNFC.Current.OnNfcStatusChanged += Current_OnNfcStatusChanged;

		//	if (Device.RuntimePlatform == Device.iOS)
		//	CrossNFC.Current.OniOSReadingSessionCancelled += Current_OniOSReadingSessionCancelled;			
		}

		void UnsubscribeEvents()
		{
			return; ;
			MessagingCenter.Unsubscribe<App, List<string>>(this, "Tag");
			//CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;
			//CrossNFC.Current.OnNfcStatusChanged -= Current_OnNfcStatusChanged;
			//if (Device.RuntimePlatform == Device.iOS)
			//	CrossNFC.Current.OniOSReadingSessionCancelled -= Current_OniOSReadingSessionCancelled;			
		}

		async void Current_OnNfcStatusChanged(bool isEnabled)
		{
			NfcIsEnabled = isEnabled;
			await ShowAlert($"NFC has been {(isEnabled ? "enabled" : "disabled")}");
		}

		async void Current_OnMessageReceived(ITagInfo tagInfo)
		{
			if (Device.RuntimePlatform == Device.iOS && iOSFirstAppear == false)
				ScanAgainButton.IsVisible = true;

			if (tagInfo == null)
			{
				await ShowAlert("No tag found", null);
				return;
			}

			// Customized serial number
			var identifier = tagInfo.Identifier;
			var serialNumber = NFCUtils.ByteArrayToHexString(identifier, ":");
			var nfcType = tagInfo.MifareType != "" ? tagInfo.MifareType : "Unknown" ;
			var title = !string.IsNullOrWhiteSpace(serialNumber) ? $"Tag [{serialNumber}]" : "Tag Info";

			if (!tagInfo.IsSupported)
			{
				await ShowAlert("Unsupported tag (app)", title);
			}
			else
			{
				ScanInfo.IsVisible = true;
				if (tagInfo.IsEmpty == false)
				{
					var first = tagInfo.Records[0];
					MessageLabel.Text = GetMessage(first);
				}
				else
				{
					MessageLabel.Text = "(Empty)";
				//	await ShowAlert("Empty tag", title);
				}

				NFCModelLabel.Text = nfcType;
				SerialNumberLabel.Text = serialNumber;
				UseScanButton.IsEnabled = true;
				UseScanButton.IsVisible = true;
				
			}

			if (Device.RuntimePlatform == Device.Android)
			{
				//CrossNFC.Current.StopListening();
				//UnsubscribeEvents();
				//SubscribeEvents();
				//CrossNFC.Current.StartListening();
			}
		}

		async void Current_OniOSReadingSessionCancelled(object sender, EventArgs e)
		{
			//Debug("User has cancelled NFC Session");
			await ShowAlert("Session cancelled/timed out","NFC read");
			CrossNFC.Current.StopListening();
			//CrossNFC.Current.StartListening();
		}


		string GetMessage(NFCNdefRecord record)
		{
			return record.Message;
			var message = $"Message: {record.Message}";
			/*message += Environment.NewLine;
			message += $"RawMessage: {Encoding.UTF8.GetString(record.Payload)}";
			message += Environment.NewLine;
			message += $"Type: {record.TypeFormat.ToString()}";

			if (!string.IsNullOrWhiteSpace(record.MimeType))
			{
				message += Environment.NewLine;
				message += $"MimeType: {record.MimeType}";
			}

			return message;*/
		}

		void Debug(string message) => System.Diagnostics.Debug.WriteLine(message);

	 	Task ShowAlert(string message, string title = null) => DisplayAlert(string.IsNullOrWhiteSpace(title) ? "NCF" : title, message, "Cancel");

		private void ScanButton_Clicked(object sender, EventArgs e)
		{
			//CrossNFC.Current.StopListening();
			//CrossNFC.Current.StartListening();
		}
	}
}