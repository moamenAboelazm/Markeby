using AutoMapper;
using library.DTOs;
using Library.Features.Users;
using Library.Features.Users.Commands;
using Library.Models;

namespace Library.MappingProfiles
{    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DtoCreateUser, AppUser>();
            CreateMap<UpdateProfileCommand, AppUser>();
            CreateMap<DtoLoginUser, AppUser>();
            CreateMap<AppUser, DtoGetUser>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.UtcNow.Year - src.BirthDate.Year - (DateTime.UtcNow.DayOfYear < src.BirthDate.DayOfYear ? 1 : 0)));
        }
    }
}