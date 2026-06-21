using AutoMapper;
using Library.Features.Boats.Commands;
using Library.Models;

namespace Library.Mapping_Profiles
{
    public class BoatProfile : Profile
    {
        public BoatProfile()
        {
            CreateMap<CreateBoatCommand, Boat>()
                .ForMember(dest => dest.Captains, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Trips, opt => opt.Ignore());

            CreateMap<UpdateBoatCommand, Boat>()
                .ForMember(dest => dest.Captains, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Trips, opt => opt.Ignore());
        }
    }
}