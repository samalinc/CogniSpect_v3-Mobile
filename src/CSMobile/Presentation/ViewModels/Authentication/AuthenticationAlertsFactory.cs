using System.Threading.Tasks;
using CSMobile.Infrastructure.Interfaces.Localization;
using CSMobile.Presentation.ViewModels.Services.Dialogs.Alerts;
using JetBrains.Annotations;

namespace CSMobile.Presentation.ViewModels.Authentication
{
    [UsedImplicitly]
    internal class AuthenticationAlertsFactory : IAuthenticationAlertsFactory
    {
        private readonly IAlertService _alertService;
        private readonly ILocalizer _localizer;

        public AuthenticationAlertsFactory(
            IAlertService alertService,
            ILocalizer localizer)
        {
            _alertService = alertService;
            _localizer = localizer;
        }
        
        public async Task IncorrectLoginOrPassword()
        {
            await _alertService.ErrorAlert(_localizer["IncorrectLoginOrPassword"]);
        }
    }
}