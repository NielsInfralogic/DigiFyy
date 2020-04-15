using DigiFyy.ViewModels;
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
    public partial class ShowPartsView : ContentPage
    {
        Image rightImage;

        int itemIndex = -1;

        public ShowPartsView()
        {
            InitializeComponent();
        }

        private void rightImage_BindingContextChanged(object sender, EventArgs e)
        {
            if (rightImage == null)
            {
                rightImage = sender as Image;
                (rightImage.Parent as View).GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(Delete) });
                rightImage.Source = ImageSource.FromResource("ListViewSwiping.Images.Delete.png");

            }
        }
        private void Delete()
        {
            ShowPartsViewModel vm = (ShowPartsViewModel)this.BindingContext;
            if (itemIndex >= 0)
                vm.Extras.RemoveAt(itemIndex);
            this.ExtrasList.ResetSwipe();

        }
    }
}