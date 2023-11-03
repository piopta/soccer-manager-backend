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

            CreateMap<EditUserPreferencesInput, UserPreferencesModel>()
                .ForMember(dest => dest.BottomMenu, src => src.MapFrom((input, original, _) => input.BottomMenu is not null ? input.BottomMenu : original.BottomMenu))
                .ForMember(dest => dest.NavbarColor, src => src.MapFrom((input, original, _) => input.NavbarColor is not null ? input.NavbarColor : original.NavbarColor))
                .ForMember(dest => dest.Mode, src => src.MapFrom((input, original, _) => input.Mode is not null ? input.Mode : original.Mode));


            CreateMap<AddStadiumInput, StadiumModel>();
            CreateMap<EditStadiumInput, StadiumModel>()
                .ForMember(dest => dest.StadiumName, src => src.MapFrom((input, original, _) => input.StadiumName is not null ? input.StadiumName : original.StadiumName))
                .ForMember(dest => dest.SeatQuality, src => src.MapFrom((input, original, _) => input.SeatQuality is not null ? input.SeatQuality : original.SeatQuality))
                .ForMember(dest => dest.FansExtrasQuality, src => src.MapFrom((input, original, _) => input.FansExtrasQuality is not null ? input.FansExtrasQuality : original.FansExtrasQuality))
                .ForMember(dest => dest.Capacity, src => src.MapFrom((input, original, _) => input.Capacity is not null ? input.Capacity : original.Capacity));

            CreateMap<AddAcademyFacilityInput, AcademyFacilityModel>();
            CreateMap<EditAcademyFacilityInput, AcademyFacilityModel>()
                .ForMember(dest => dest.SecondTeamName, src => src.MapFrom((input, original, _) => input.SecondTeamName is not null ? input.SecondTeamName : original.SecondTeamName))
                .ForMember(dest => dest.ManagerQuality, src => src.MapFrom((input, original, _) => input.ManagerQuality is not null ? input.ManagerQuality : original.ManagerQuality))
                .ForMember(dest => dest.FacilitiesQuality, src => src.MapFrom((input, original, _) => input.FacilitiesQuality is not null ? input.FacilitiesQuality : original.FacilitiesQuality));

            CreateMap<AddCalendarEventInput, CalendarEventModel>();
            CreateMap<EditCalendarEventInput, CalendarEventModel>()
                .ForMember(dest => dest.Description, src => src.MapFrom((input, original, _) => input.Description))
                .ForMember(dest => dest.EventType, src => src.MapFrom((input, original, _) => input.EventType));

            CreateMap<AddCalendarEventInput, MatchModel>()
                .ForMember(dest => dest.AwayTeamId, src => src.MapFrom(opt => opt.RivalTeamId));
            CreateMap<AddCalendarEventInput, TrainingModel>();
            CreateMap<EditCalendarEventInput, MatchModel>()
                .ForMember(dest => dest.AwayTeamId, src => src.MapFrom((input, original, _) => input.RivalTeamId is not null ? input.RivalTeamId : original.AwayTeamId))
                .ForMember(dest => dest.Ground, src => src.MapFrom((input, original, _) => input.Ground is not null ? input.Ground : original.Ground));
            CreateMap<EditCalendarEventInput, TrainingModel>();
        }
    }
}
