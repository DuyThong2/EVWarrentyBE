using AutoMapper;
using EVWUser.Business.Dtos;
using EVWUser.Data.Models;

namespace EVWUser.Business.AutoMapper
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

            CreateMap<UserCreateRequest, User>();

            CreateMap<UserUpdateRequest, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
