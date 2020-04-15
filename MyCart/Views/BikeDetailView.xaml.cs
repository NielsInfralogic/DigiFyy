using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace DigiFyy.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BikeDetailView : ContentPage
    {
        public BikeDetailView()
        {
            InitializeComponent();
        }

        private void SfButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}