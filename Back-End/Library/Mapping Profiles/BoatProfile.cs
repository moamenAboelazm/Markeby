using AutoMapper;
using Library.Features.Boats;
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

            CreateMap<Boat, DtoBoats>()
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src =>
                    src.Images != null && src.Images.Any() ? src.Images.First().ImageUrl : null))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Boat, DtoBoat>()
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src =>
                    src.Images != null && src.Images.Any() ? src.Images.First().ImageUrl : null))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            
            CreateMap<BoatImage, DtoBoatImage>();
        }
    }
}