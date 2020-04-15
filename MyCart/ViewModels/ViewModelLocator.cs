using DigiFyy.DataService;
using DigiFyy.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using TinyIoC;
using Xamarin.Forms;

namespace DigiFyy.ViewModels
{
    public static class ViewModelLocator
    {
        private static readonly TinyIoCContainer _container;

        public static readonly BindableProperty AutoWireViewModelProperty =
           BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(ViewModelLocator.AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(ViewModelLocator.AutoWireViewModelProperty, value);
        }

        static ViewModelLocator()
        {
            _container = new TinyIoCContainer();

            // Services - by default, TinyIoC will register interface registrations as singletons.
            _container.Register<INavigationService, NavigationService>();
            _container.Register<IDialogService, DialogService>();
            _container.Register<IAnalyticsService, AppCenterAnalyticsService>();
            //IF NUS AWS
            _container.Register<IDigifyyDataService, AWSDigifyyDataService>();
            // ELSE
            // _container.Register<IDigifyyDataService, AzureDigifyyDataService>();

            // View models - by default, TinyIoC will register concrete classes as multi-instance.
            _container.Register<MainViewModel>();
            _container.Register<LoginSignupViewModel>();
            _container.Register<LoginViewModel>();
            _container.Register<SignUpViewModel>();
            _container.Register<ForgotPasswordViewModel>();
            _container.Register<ResetPasswordViewModel>();
            _container.Register<BikeDetailViewModel>();
            _container.Register<AboutUsViewModel>();
            _container.Register<ScanViewModel>();
            _container.Register<DocumentsViewModel>();
            _container.Register<ShowPositionViewModel>();
            _container.Register<MessagesViewModel>();
            _container.Register<SettingsViewModel>();
            _container.Register<PdfViewerViewModel>();
            _container.Register<AddPictureViewModel>();
            _container.Register<AddExtraViewModel>();
            _container.Register<HomeViewModel>();
            _container.Register<ShowPartsViewModel>();
            _container.Register<ShowSpecsViewModel>();
            _container.Register<ImageViewerViewModel>();          
            _container.Register<DeregisterViewModel>();

        }

        public static void RegisterSingleton<TInterface, T>() where TInterface : class where T : class, TInterface
        {
            _container.Register<TInterface, T>().AsSingleton();
        }

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Element;
            if (view == null)
            {
                return;
            }

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }
            var viewModel = _container.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}
