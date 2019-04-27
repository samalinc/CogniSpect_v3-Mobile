using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSMobile.Infrastructure.Common;
using CSMobile.Infrastructure.Mvvm.Navigation;
using CSMobile.Infrastructure.Mvvm.ViewModelsCore;
using Xamarin.Forms;

namespace CSMobile.Presentation.Views.Services
{
    internal class NavigationService : INavigationService
    {
        private readonly ISafeInjectionResolver _safeInjectionResolver;
        private readonly IDictionary<Type, Type> _pagesByKey;
        private readonly Stack<NavigationPage> _navigationPageStack;

        public NavigationService(ISafeInjectionResolver safeInjectionResolver)
        {
            _safeInjectionResolver = safeInjectionResolver;
            _pagesByKey = new Dictionary<Type, Type>();
            _navigationPageStack = new Stack<NavigationPage>();
        }

        public NavigationPage CurrentNavigationPage => _navigationPageStack.Peek();

        public void Configure<TViewModel, TViewPage>() 
            where TViewModel : BasePageViewModel
            where TViewPage : IViewPage<TViewModel>
        {
            var viewModelType = typeof(TViewModel);
            if (_pagesByKey.ContainsKey(viewModelType))
            {
                _pagesByKey[viewModelType] = typeof(TViewPage);
            }
            else
            {
                _pagesByKey.Add(viewModelType, typeof(TViewPage));
            }
        }

        public Page SetRootPage<TViewModel>() where TViewModel : BasePageViewModel
        {
            Page rootPage = GetPage(typeof(TViewModel));
            _navigationPageStack.Clear();
            var mainPage = new NavigationPage(rootPage);
            _navigationPageStack.Push(mainPage);
            return mainPage;
        }

        public async Task GoToRoot()
        {
            if (CurrentNavigationPage.NavigationProxy.NavigationStack.Count > 0)
            {
                await CurrentNavigationPage.Navigation.PopToRootAsync();                
            }
        }

        public async Task GoBack()
        {
            if (CurrentNavigationPage.Navigation.NavigationStack.Count > 1)
            {
                await CurrentNavigationPage.Navigation.PopAsync();
                return;
            }

            if (_navigationPageStack.Count > 1)
            {
                _navigationPageStack.Pop();
                await CurrentNavigationPage.Navigation.PopModalAsync();
                return;
            }

            await CurrentNavigationPage.PopAsync();
        }

        public async Task NavigateModalAsync<TViewModel>(bool animated = false) where TViewModel : BasePageViewModel
        {
            var page = GetPage(typeof(TViewModel));
            NavigationPage.SetHasNavigationBar(page, false);
            var modalNavigationPage = new NavigationPage(page);
            await CurrentNavigationPage.Navigation.PushModalAsync(modalNavigationPage, animated);
            _navigationPageStack.Push(modalNavigationPage);
        }

        public async Task NavigateAsync<TViewModel>(bool animated = false) where TViewModel : BasePageViewModel
        {
            var page = GetPage(typeof(TViewModel));
            await CurrentNavigationPage.Navigation.PushAsync(page, animated);
        }

        private Page GetPage(Type viewModelType)
        {
            if (!_pagesByKey.ContainsKey(viewModelType))
            {
                throw new ArgumentException(
                    $"No such page for viewmodel: {viewModelType}. Did you forget to call NavigationService.Configure?");
            }

            return (Page) _safeInjectionResolver.ResolveService(_pagesByKey[viewModelType]);
        }
    }
}