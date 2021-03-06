using CSMobile.Domain.Models.Sessions;
using CSMobile.Infrastructure.Common.Extensions;
using JetBrains.Annotations;

namespace CSMobile.Presentation.ViewModels.Sessions
{
    [UsedImplicitly]
    internal class SessionViewModelsMapperProfile : AutoMapper.Profile
    {
        public SessionViewModelsMapperProfile()
        {
            CreateMap<SessionListItem, SessionListItemViewModel>()
                .ConstructByDiContainer()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.SecurityPoints, o => o.MapFrom(s => s.SecurityPoints))
                .IgnoreAllOther();

            CreateMap<SessionListItemViewModel, SessionListItem>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.SecurityPoints, o => o.MapFrom(s => s.SecurityPoints))
                .IgnoreAllOther();
        }
    }
}