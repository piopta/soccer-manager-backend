using AutoMapper;
using WebApi.Models.DTOs;

namespace WebApi.Profiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>();
        }
    }
}
