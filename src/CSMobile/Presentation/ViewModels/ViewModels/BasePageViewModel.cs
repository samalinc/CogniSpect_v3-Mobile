using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CSMobile.Application.ViewModels.ViewModels
{
    public class BasePageViewModel : BaseViewModel, ICanThink
    {
        private bool _isThinking;
        
        public bool IsThinking
        {
            get => _isThinking;
            set => Set(nameof(IsThinking), ref _isThinking, value);
        }
        
        public virtual Task OnAppearing()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnDisappearing()
        {
            return Task.CompletedTask;
        }
        
        protected sealed override ICommand Command(Func<Task> action)
        {
            return new Command(async () =>
            {
                IsThinking = true;
                await action();
                IsThinking = false;
            });
        }
        
        protected sealed override ICommand Command<TArg>(Action<TArg> action)
        {
            return new Command<TArg>((arg) =>
            {
                IsThinking = true;
                action(arg);
                IsThinking = false;
            });
        }
        
        protected sealed override ICommand Command<TArg>(Func<TArg, Task> action)
        {
            return new Command<TArg>(async arg =>
            {
                IsThinking = true;
                await action(arg);
                IsThinking = false;
            });
        }
    }
}