using System;
using System.Collections.Generic;
using Xamarin.Forms;
using DigiFyy.Views;
using DigiFyy.Services;
using DigiFyy.ViewModels;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace DigiFyy.Services
{
    public class NavigationService : INavigationService
    {
        protected readonly Dictionary<Type, Type> MappingPageAndViewModel;

        protected Application CurrentApplication => Application.Current;

        public ViewModelBase PreviousPageViewModel
        {
            get
            {
                var mainPage = Application.Current.MainPage as CustomNavigationView;
                var viewModel = mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2].BindingContext;
                return viewModel as ViewModelBase;
            }
        }

        public NavigationService()
        {
            MappingPageAndViewModel = new Dictionary<Type, Type>();
            SetPageViewModelMappings();
        }

        public Task InitializeAsync()
        {
            // if (string.IsNullOrEmpty(_settingsService.AuthAccessToken))
          //  if (string.IsNullOrEmpty(Preferences.Get("Token", "")))
        //        return NavigateToAsync<LoginPageViewModel>();
         //   else
                return NavigateToAsync<MainViewModel>();
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task RemoveLastFromBackStackAsync()
        {
            var mainPage = Application.Current.MainPage as CustomNavigationView;

            if (mainPage != null)
            {
                mainPage.Navigation.RemovePage(
                    mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        public Task RemoveBackStackAsync()
        {
            var mainPage = Application.Current.MainPage as CustomNavigationView;

            if (mainPage != null)
            {
                for (int i = 0; i < mainPage.Navigation.NavigationStack.Count - 1; i++)
                {
                    var page = mainPage.Navigation.NavigationStack[i];
                    mainPage.Navigation.RemovePage(page);
                }
            }

            return Task.FromResult(true);
        }



        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType);

            if (page is MainView)
            {
                Application.Current.MainPage = new CustomNavigationView(page);
            }
            else
            {
                CustomNavigationView navigationPage = Application.Current.MainPage as CustomNavigationView;
                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    Application.Current.MainPage = new CustomNavigationView(page);
                }
            }

            // Pass parameter in..

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (MappingPageAndViewModel.ContainsKey(viewModelType))
            {
                return MappingPageAndViewModel[viewModelType];
                //                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            // else - try name mapping directy..          

            var pageName = viewModelType.FullName.Replace("ViewModel", "View");
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var pageAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", pageName, viewModelAssemblyName);
            var pageType = Type.GetType(pageAssemblyName);
            return pageType;
        }

        public Page CreatePage(Type viewModelType)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }

        private void SetPageViewModelMappings()
        {
            MappingPageAndViewModel.Add(typeof(MainViewModel), typeof(MainView));
            MappingPageAndViewModel.Add(typeof(LoginViewModel), typeof(LoginView));
            MappingPageAndViewModel.Add(typeof(SignUpViewModel), typeof(SignUpView));
            MappingPageAndViewModel.Add(typeof(ForgotPasswordViewModel), typeof(ForgotPasswordView));
            MappingPageAndViewModel.Add(typeof(ResetPasswordViewModel), typeof(ResetPasswordView));
            MappingPageAndViewModel.Add(typeof(BikeDetailViewModel), typeof(BikeDetailView));
            MappingPageAndViewModel.Add(typeof(AboutUsViewModel), typeof(AboutUsSimpleView));
            MappingPageAndViewModel.Add(typeof(ScanViewModel), typeof(ScanView));
            
            MappingPageAndViewModel.Add(typeof(ShowPartsViewModel), typeof(ShowPartsView));
            MappingPageAndViewModel.Add(typeof(ShowSpecsViewModel), typeof(ShowSpecsView));
            MappingPageAndViewModel.Add(typeof(DocumentsViewModel), typeof(DocumentsView));
            MappingPageAndViewModel.Add(typeof(ShowPositionViewModel), typeof(ShowPositionView));
            MappingPageAndViewModel.Add(typeof(MessagesViewModel), typeof(MessagesView));
            MappingPageAndViewModel.Add(typeof(SettingsViewModel), typeof(SettingsView));
            MappingPageAndViewModel.Add(typeof(PdfViewerViewModel), typeof(PdfViewerView));
            MappingPageAndViewModel.Add(typeof(ImageViewerViewModel), typeof(ImageViewerView));
            MappingPageAndViewModel.Add(typeof(AddPictureViewModel), typeof(AddPictureView));
            MappingPageAndViewModel.Add(typeof(AddExtraViewModel), typeof(AddExtraView));
            MappingPageAndViewModel.Add(typeof(HomeViewModel), typeof(HomeView));
            MappingPageAndViewModel.Add(typeof(DeregisterViewModel), typeof(DeregisterView));
            MappingPageAndViewModel.Add(typeof(LoginSignupViewModel), typeof(LoginSignupView));

        }

        public async Task GoBackAsync()
        {
            CustomNavigationView navigationPage = Application.Current.MainPage as CustomNavigationView;
            if (navigationPage != null)
            {
                await navigationPage.PopAsync();
            }

        }

    }
}