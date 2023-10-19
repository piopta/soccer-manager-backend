using AutoMapper;

namespace WebApi.Profiles;

public class MainProfile : Profile
{
    public MainProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserDTO>().ForMember(dest => dest.LockoutEnabled,
            opts => opts.MapFrom(src => src.LockoutEnd != null));
    }
}
