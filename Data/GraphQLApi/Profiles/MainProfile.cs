using AutoMapper;

namespace GraphQLApi.Profiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<AddTeamInput, TeamModel>();

            CreateMap<AddTeamInput, LogoModel>()
                .ForMember(dest => dest.MainColor, src => src.MapFrom(p => p.LogoMainColor))
                .ForMember(dest => dest.SecondaryColor, src => src.MapFrom(p => p.LogoSecondaryColor))
                .ForMember(dest => dest.Type, src => src.MapFrom(p => p.LogoType));

            CreateMap<AddTeamInput, ShirtModel>()
                .ForMember(dest => dest.MainColor, src => src.MapFrom(p => p.FirstMainColor))
                .ForMember(dest => dest.SecondaryColor, src => src.MapFrom(p => p.FirstSecondaryColor))
                .ForMember(dest => dest.Type, src => src.MapFrom(p => p.FirstType));

            CreateMap<AddUserPreferencesInput, UserPreferencesModel>();
        }
    }
}
