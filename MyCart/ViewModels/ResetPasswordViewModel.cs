using DigiFyy.Helpers;
using DigiFyy.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DigiFyy.ViewModels
{
    /// <summary>
    /// ViewModel for reset password page.
    /// </summary>
    public class ResetPasswordViewModel : ViewModelBase
    {


        #region Fields

        private string newPassword;
        public string NewPassword
        {
            get
            {
                return this.newPassword;
            }

            set
            {
                SetProperty(ref newPassword, value);
            }
        }

        private string email;
        public string Email
        {
            get
            {
                return this.email;
            }

            set
            {
                SetProperty(ref email, value);
            }
        }


        private bool isInvalidEmail;
        public bool IsInvalidEmail
        {
            get
            {
                return this.isInvalidEmail;
            }

            set
            {
                SetProperty(ref isInvalidEmail, value);
            }
        }

        private string confirmPassword;

        public string ConfirmPassword
        {
            get
            {
                return this.confirmPassword;
            }

            set
            {
                SetProperty(ref confirmPassword, value);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordViewModel" /> class.
        /// </summary>
        public ResetPasswordViewModel()
        {
            this.SubmitCommand = new Command(this.SubmitClicked);
            this.SignUpCommand = new Command(this.SignUpClicked);
        }

        #endregion

        #region Event

        /// <summary>
        /// The declaration of the property changed event.
        /// </summary>
        //public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Submit button is clicked.
        /// </summary>
        public Command SubmitCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Invoked when the Submit button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SubmitClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SignUpClicked(object obj)
        {
            // Do something
        }

        #endregion
    }
}