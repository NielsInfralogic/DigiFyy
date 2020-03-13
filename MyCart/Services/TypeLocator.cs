using Autofac;
using System;
using DigiFyy.ViewModels;
using DigiFyy.DataService;

namespace DigiFyy.Services
{
    public class TypeLocator
    {
        private IContainer container;

        private readonly ContainerBuilder containerBuilder;

        private static readonly TypeLocator TypeLocatorInstance = new TypeLocator();

        public static TypeLocator Instance
        {
            get
            {
                return TypeLocatorInstance;
            }
        }

        public TypeLocator()
        {
            containerBuilder = new ContainerBuilder();            
            containerBuilder.RegisterType<NavigationService>().As<INavigationService>();
            containerBuilder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            containerBuilder.RegisterType<AppCenterAnalyticsService>().As<IAnalyticsService>().SingleInstance();
            containerBuilder.RegisterType<AWSDigifyyDataService>().As<IDigifyyDataService>().SingleInstance(); 
          //  containerBuilder.RegisterType<Startup>();
            containerBuilder.RegisterType<OnBoardingViewModel>();
            containerBuilder.RegisterType<LoginPageViewModel>();
            containerBuilder.RegisterType<SignUpPageViewModel>();
            containerBuilder.RegisterType<ForgotPasswordViewModel>();
            containerBuilder.RegisterType<ResetPasswordViewModel>();  
            containerBuilder.RegisterType<BikeDetailViewModel>();
            containerBuilder.RegisterType<AboutUsViewModel>();
            containerBuilder.RegisterType<ScanViewModel>();
            containerBuilder.RegisterType<DocumentsViewModel>();
            containerBuilder.RegisterType<ShowPositionViewModel>();
            containerBuilder.RegisterType<MessagesViewModel>();
            containerBuilder.RegisterType<SettingsViewModel>();

        }

        public object Resolve(Type type, NamedParameter namedParameter = null)
        {
            if (namedParameter == null)
            {
                return container.IsRegistered(type) ? container.Resolve(type) : null;
            }
            return container.IsRegistered(type) ? container.Resolve(type, namedParameter) : null;
        }

        public void Build()
        {
            container = containerBuilder?.Build();
        }
    }
}
