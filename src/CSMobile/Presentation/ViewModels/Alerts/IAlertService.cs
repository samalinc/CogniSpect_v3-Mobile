using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CSMobile.Presentation.ViewModels.Alerts
{
    public interface IAlertService
    {
        Task<bool> Alert([NotNull] AlertConfigs configs);
        Task ErrorAlert([NotNull] string message);
    }
}