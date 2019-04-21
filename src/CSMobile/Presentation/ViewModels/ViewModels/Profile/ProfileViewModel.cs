using System.Threading.Tasks;
using System.Windows.Input;
using CSMobile.Domain.Services.Authentication;
using CSMobile.Infrastructure.Mvvm.ViewModelsCore;

namespace CSMobile.Presentation.ViewModels.ViewModels.Profile
{
    public class ProfileViewModel : BasePageViewModel
    {
        private readonly IAuthenticationService _authenticationService;

        public override string Title { get; set; } = "Profile";

        public ICommand SignOutCommand { get; }

        public ProfileViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            SignOutCommand = Command(SignOut);
        }

        private async Task SignOut()
        {
            await _authenticationService.SignOut();
        }
    }
}