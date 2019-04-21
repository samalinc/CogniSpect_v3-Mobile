﻿using System;
using Autofac;
using Autofac.Core;
using CommonServiceLocator;
using CSMobile.Domain.Services;
using CSMobile.Domain.Services.Authentication;
using CSMobile.Infrastructure.Common.Contexts;
using CSMobile.Infrastructure.Common.Extensions;
using CSMobile.Infrastructure.Mvvm;
using CSMobile.Infrastructure.Mvvm.Navigation;
using CSMobile.Infrastructure.Security;
using CSMobile.Infrastructure.Services;
using CSMobile.Infrastructure.WebSockets;
using CSMobile.Infrastructure.WebSockets.Extensions;
using CSMobile.Presentation.ViewModels;
using CSMobile.Presentation.ViewModels.Profiles;
using CSMobile.Presentation.ViewModels.ViewModels.Authentication;
using CSMobile.Presentation.ViewModels.ViewModels.Core;
using CSMobile.Presentation.ViewModels.ViewModels.Profile;
using CSMobile.Presentation.ViewModels.ViewModels.Statistics;
using CSMobile.Presentation.ViewModels.ViewModels.Tests;
using CSMobile.Presentation.ViewModels.ViewModels.Tests.List;
using CSMobile.Presentation.Views.Pages;
using CSMobile.Presentation.Views.Pages.Layouts;
using CSMobile.Presentation.Views.Services;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xamarin.Forms;

namespace CSMobile.Presentation.Views
{
    public class App : Application
    {
        private INavigationService _navigationService;
        private IServiceLocator _serviceLocator;

        public static App Instance { get; private set; }
        public ApplicationContext Context { get; }

        /// <summary>
        /// Creates main platform independent entry class
        /// </summary>
        /// <param name="platformSpecificModule">
        /// Platform specific services module.
        /// Used to provide implementations for a specific platform 
        /// </param>
        public App([NotNull] IModule platformSpecificModule)
        {
            Context = BuildApplication(platformSpecificModule);
            Instance = this;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private ApplicationContext BuildApplication(IModule platformSpecificModule)
        {
            var context = new ApplicationContext(
                ConfigureContainer(builder => builder
                    .RegisterAutomapper(cfg => cfg
                        .RegisterProfile<TestsMappingProfile>()
                        .RegisterProfile<AuthenticationMappingProfile>()
                    )
                    .RegisterResponseHandlerResolver(cfg => cfg
                        .RegisterRecorder<WebSocketsHandlersRecorder>()
                    )
                    .RegisterModule(platformSpecificModule)
                    .RegisterModule<PresentationViewsModule>()
                    .RegisterModule<MvvmModule>()
                    .RegisterModule<WebSocketsModule>()
                    .RegisterModule<PresentationViewModelsModule>()
                    .RegisterModule<DomainServicesModule>()
                    .RegisterModule<InfrastructureServicesModule>()
                    .RegisterModule<InfrastructureSecurityModule>()
                ));

            _serviceLocator = new AutofacServiceLocator(context);
            ServiceLocator.SetLocatorProvider(() => _serviceLocator);
            
            ConfigureNavigation();
            ConfigureJsonSerializing();
            
            return context;
        }

        private IContainer ConfigureContainer(Action<ContainerBuilder> action)
        {
            var containerBuilder = new ContainerBuilder();
            action(containerBuilder);

            return containerBuilder.Build();
        }

        private void ConfigureNavigation()
        {
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            
            _navigationService.Configure<AuthenticationViewModel, AuthenticationPage>();
            _navigationService.Configure<ProfileViewModel, ProfilePage>();
            _navigationService.Configure<TestItemsViewModel, TestItemsPage>();
            _navigationService.Configure<TestViewModel, TestPage>();
            _navigationService.Configure<StatisticsViewModel, StatisticsPage>();
            _navigationService.Configure<TabbedLayoutViewModel, TabbedLayoutPage>();

            MainPage = ((NavigationService) _navigationService).SetRootPage<AuthenticationViewModel>();
        }

        private void ConfigureJsonSerializing()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}