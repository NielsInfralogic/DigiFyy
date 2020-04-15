using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DigiFyy.ViewModels
{
    public class LoginSignupViewModel : ViewModelBase
    {
        private bool firstTimeRegister;

        public Helpers.Command BackButtonCommand { get; set; }
        public Helpers.Command LoginCommand { get; set; }

        public Helpers.Command SignupCommand { get; set; }
        
        public LoginSignupViewModel()
        {
            firstTimeRegister = false;
            this.BackButtonCommand = new Helpers.Command(this.BackButtonClicked);
            this.LoginCommand = new Helpers.Command(this.LoginButtonClicked);
            this.SignupCommand = new Helpers.Command(this.SignupButtonClicked);
        }

        private async void BackButtonClicked(object obj)
        {
            await NavigationService.GoBackAsync();
        }

        private async void LoginButtonClicked(object obj)
        {
            await NavigationService.NavigateToAsync<LoginViewModel>(firstTimeRegister);
        }

        private async void SignupButtonClicked(object obj)
        {
            await NavigationService.NavigateToAsync<SignUpViewModel>(firstTimeRegister);
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData == null)
                return;
            if (navigationData is bool)
                firstTimeRegister = (bool)navigationData;
        }

          

    }
}
