using AutoMapper;

namespace WebApi.Profiles;

public class MainProfile : Profile
{
    public MainProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserDTO>();
    }
}
