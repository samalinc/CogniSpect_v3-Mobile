using System;
using CommonServiceLocator;
using CSMobile.Infrastructure.Interfaces.Localization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CSMobile.Presentation.Views.ViewExtensions
{
    [ContentProperty("Text")]
    internal class TranslateExtension : IMarkupExtension
    {
        private readonly ILocalizer _localizer;
        
        public string Text { get; set; }

        public TranslateExtension()
        {
            _localizer = ServiceLocator.Current.GetInstance<ILocalizer>();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Text == null ? "" : _localizer[Text];
        }
    }
}