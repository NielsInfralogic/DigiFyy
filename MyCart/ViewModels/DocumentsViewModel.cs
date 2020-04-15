using DigiFyy.Models.AWS;
using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    public class DocumentsViewModel : ViewModelBase
    {

        private ObservableCollection<FrameNumberDocument> _documents = new ObservableCollection<FrameNumberDocument>();
        public ObservableCollection<FrameNumberDocument> Documents
        {
            get => _documents;
            set
            {
                _documents = value;
                SetProperty(ref _documents, value);
            }
        }

        public Helpers.Command BackButtonCommand { get; set; }


        public DocumentsViewModel()
        {
            
            this.BackButtonCommand = new Helpers.Command(this.BackButtonClicked);

        }

        private async void BackButtonClicked(object obj)
        {           
            await NavigationService.GoBackAsync();
        }

        



        private Command itemSelectedCommand;
        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand
        {
            get { return this.itemSelectedCommand ?? (this.itemSelectedCommand = new Command(this.ItemSelected)); }
        }

        //        public override void Init(object navigationData)
        public override async Task InitializeAsync(object navigationData)
        {            
            if (navigationData == null)
                return;
            if (navigationData is List<Models.AWS.FrameNumberDocument>)
                (navigationData as List<Models.AWS.FrameNumberDocument>).ForEach(p => _documents.Add(p));
            Documents = _documents;
        }


        private async void ItemSelected(object attachedObject)
        {
            if (attachedObject == null)
                return;

            if (attachedObject is FrameNumberDocument document)
            {
                //await NavigationService.NavigateTo(typeof(PdfViewerViewModel), "PDF", document.DocumentUrl);
                string ext = Path.GetExtension(document.DocumentUrl).ToLower().Replace(".", "");
                if (ext == "pdf")
                    await NavigationService.NavigateToAsync<PdfViewerViewModel>(document);
                else
                    await NavigationService.NavigateToAsync<ImageViewerViewModel>(document);
            }

        }        

        public ObservableCollection<FrameNumberDocument> DocumentsList { get; set; }

        /// <summary>
        /// Invoked when an item is selected from the documents list.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private void NavigateToNextPage(object selectedItem)
        {
            // Do something
            FrameNumberDocument selectedDoc = selectedItem as FrameNumberDocument;
        }
    }
}
