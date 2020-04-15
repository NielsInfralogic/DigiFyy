using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace DigiFyy.Views
{
    /// <summary>
    /// About us simple page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutUsSimpleView : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DigiFyy.Views.AboutUsSimplePage"/> class.
        /// </summary>
        public AboutUsSimpleView()
        {
            this.InitializeComponent();
        }
    }
}