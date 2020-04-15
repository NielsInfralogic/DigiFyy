using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace DigiFyy.Views
{
    /// <summary>
    /// Page to sign in with user details.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSignUpPage" /> class.
        /// </summary>
        public SignUpView()
        {
            this.InitializeComponent();
        }
    }
}