using DigiFyy.Helpers;
using DigiFyy.Services;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace DigiFyy.ViewModels
{
    /// <summary>
    /// ViewModel for forgot password page.
    /// </summary>
    public class ForgotPasswordViewModel : ViewModelBase
    {
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

        #region Constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordViewModel" /> class.
        /// </summary>
        public ForgotPasswordViewModel()
        {
           
            this.SendCommand = new Command(this.SendClicked);
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Send button is clicked.
        /// </summary>
        public Command SendCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Send button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SendClicked(object obj)
        {
            string email = Email != "" ? Email : Preferences.Get("Email", "");
               
            // TODO - send email 
            if (email != "")
            {
                EmailSender emailSender = new EmailSender();
                await emailSender.SendEmail("Digifyy reset password", "Test message only...", new List<string> { email });
            }
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