using AutoMapper;
using EVWUser.API.Dtos;
using EVWUser.API.Models;

namespace EVWUser.API.Extensions.AutoMapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Status,
                           opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Roles,
                           opt => opt.Ignore());

            CreateMap<UserRequest, User>();
        }
    }
}
