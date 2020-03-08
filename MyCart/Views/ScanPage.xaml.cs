using Plugin.NFC;
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
    public partial class ScanPage : ContentPage
    {
		 NFCNdefTypeFormat _type;

		public ScanPage()
        {
            InitializeComponent();
        }

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			CrossNFC.Current.StopListening();
			UnsubscribeEvents();
			UseScanButton.IsEnabled = false;

			if (CrossNFC.IsSupported)
			{
				if (!CrossNFC.Current.IsAvailable)
					await ShowAlert("NFC is not available", null);

				if (!CrossNFC.Current.IsEnabled)
					await ShowAlert("NFC is disabled", null);

				SubscribeEvents();

				//if (Device.RuntimePlatform != Device.iOS)
				//{
					// Start NFC tag listening manually
					CrossNFC.Current.StartListening();
				//}
			}
		}

		protected async override  void OnDisappearing()
		{
			base.OnDisappearing();
			CrossNFC.Current.StopListening();
			UnsubscribeEvents();
		}

		void SubscribeEvents()
		{
			CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
			if (Device.RuntimePlatform == Device.iOS)
				CrossNFC.Current.OniOSReadingSessionCancelled += Current_OniOSReadingSessionCancelled;
		}

		void UnsubscribeEvents()
		{
			CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;
			if (Device.RuntimePlatform == Device.iOS)
				CrossNFC.Current.OniOSReadingSessionCancelled -= Current_OniOSReadingSessionCancelled;
		}

		async void Current_OnMessageReceived(ITagInfo tagInfo)
		{
			if (tagInfo == null)
			{
				await ShowAlert("No tag found", null);
				return;
			}

			// Customized serial number
			var identifier = tagInfo.Identifier;
			var serialNumber = NFCUtils.ByteArrayToHexString(identifier, ":");
			var title = !string.IsNullOrWhiteSpace(serialNumber) ? $"Tag [{serialNumber}]" : "Tag Info";

			if (!tagInfo.IsSupported)
			{
				await ShowAlert("Unsupported tag (app)", title);
			}
			else if (tagInfo.IsEmpty)
			{
				await ShowAlert("Empty tag", title);
			}
			else
			{
				var first = tagInfo.Records[0];
				MessageLabel.Text = GetMessage(first);
				UseScanButton.IsEnabled = true;
				//await ShowAlert(GetMessage(first), title);

			}
		}

		void Current_OniOSReadingSessionCancelled(object sender, EventArgs e) => Debug("User has cancelled NFC Session");

		

		string GetMessage(NFCNdefRecord record)
		{
			var message = $"Message: {record.Message}";
			message += Environment.NewLine;
			message += $"RawMessage: {Encoding.UTF8.GetString(record.Payload)}";
			message += Environment.NewLine;
			message += $"Type: {record.TypeFormat.ToString()}";

			if (!string.IsNullOrWhiteSpace(record.MimeType))
			{
				message += Environment.NewLine;
				message += $"MimeType: {record.MimeType}";
			}

			return message;
		}

		void Debug(string message) => System.Diagnostics.Debug.WriteLine(message);

	 	Task ShowAlert(string message, string title = null) => DisplayAlert(string.IsNullOrWhiteSpace(title) ? "NCF" : title, message, "Cancel");
	}
}