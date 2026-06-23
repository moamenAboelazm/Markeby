using AutoMapper;
using Library.Features.Captains;
using Library.Features.Captains.Commands;
using Library.Models;

namespace Library.Mapping_Profiles
{
    public class CaptainProfile : Profile
    {
        public CaptainProfile()
        {
            CreateMap<Captain, DtoCaptain>();

            CreateMap<CreateCaptainCommand, Captain>()
                .ForMember(dest => dest.ProfilePhotoUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Boats, opt => opt.Ignore())
                .ForMember(dest => dest.Trips, opt => opt.Ignore());

            CreateMap<UpdateCaptainCommand, Captain>()
                .ForMember(dest => dest.ProfilePhotoUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Boats, opt => opt.Ignore())
                .ForMember(dest => dest.Trips, opt => opt.Ignore());
        }
    }
}
