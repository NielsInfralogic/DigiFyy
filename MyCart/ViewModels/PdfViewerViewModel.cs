using DigiFyy.Models.AWS;
using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigiFyy.ViewModels
{
    public class PdfViewerViewModel : ViewModelBase
    {
        private string _pdfUrl;
        public string PdfUrl
        {
            get
            {
                return _pdfUrl;
            }
            set
            {
                SetProperty(ref _pdfUrl, value);
            }
        }

        private string _pdfTitle;
        public string PdfTitle
        {
            get
            {
                return _pdfTitle;
            }
            set
            {
                SetProperty(ref _pdfTitle, value);
            }
        }

        private Stream pdfDocumentStream;
        public Stream PdfDocumentStream
        {
            get { return pdfDocumentStream; }
            set
            {
                SetProperty(ref pdfDocumentStream, value);
            }
        }
        public Helpers.Command BackButtonCommand { get; set; }

        public PdfViewerViewModel()
        {
            this.BackButtonCommand = new Helpers.Command(this.BackButtonClicked);
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is FrameNumberDocument)
            {
                FrameNumberDocument doc = navigationData as FrameNumberDocument;
                PdfUrl = doc.DocumentUrl;
                PdfTitle = doc.DocumentTitle;
            }

            SetStreamAsync();

        }

        private async Task<Stream> DownloadPdfStream(string URL)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(URL);
            //Check whether redirection is needed
            if ((int)response.StatusCode == 302)
            {
                //The URL to redirect is in the header location of the response message
                HttpResponseMessage redirectedResponse = await httpClient.GetAsync(response.Headers.Location.AbsoluteUri);
                return await redirectedResponse.Content.ReadAsStreamAsync();
            }
            return await response.Content.ReadAsStreamAsync();
        }

        private async void SetStreamAsync()
        {
            PdfDocumentStream = await DownloadPdfStream(PdfUrl);
        }

        private async void BackButtonClicked(object obj)
        {
            await NavigationService.GoBackAsync();
        }

    }
}
