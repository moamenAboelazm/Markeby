using AutoMapper;
using Library.Features.Boats;
using Library.Features.Captains;
using Library.Features.Trips;
using Library.Features.Trips.Commands;
using Library.Models;

namespace Library.Mapping_Profiles
{
    public class TripProfile : Profile
    {
        public TripProfile()
        {
            CreateMap<CreateTripCommand, Trip>()
                .ForMember(dest => dest.Boat, opt => opt.Ignore())
                .ForMember(dest => dest.Captain, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore());

            CreateMap<UpdateTripCommand, Trip>()
                .ForMember(dest => dest.Boat, opt => opt.Ignore())
                .ForMember(dest => dest.Captain, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore());

            CreateMap<Trip, DtoTrip>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.BoatSeats, opt => opt.MapFrom(src => src.Boat.Capacity))
                .ForMember(dest => dest.BoatName, opt => opt.MapFrom(src => src.Boat != null ? src.Boat.Name : "Unassigned"))
                .ForMember(dest => dest.CaptainName, opt => opt.MapFrom(src => src.Captain != null ? src.Captain.FullName : "Unassigned"));

            CreateMap<Trip, DtoTripById>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.BoatSeats, opt => opt.MapFrom(src => src.Boat.Capacity))
                .ForMember(dest => dest.BoatName, opt => opt.MapFrom(src => src.Boat != null ? src.Boat.Name : "Unassigned"))
                .ForMember(dest => dest.CaptainName, opt => opt.MapFrom(src => src.Captain != null ? src.Captain.FullName : "Unassigned"))
                .ForMember(dest => dest.ImagesURLs, opt => opt.MapFrom(src => src.Images != null ? src.Images.Select(i => i.ImageUrl).ToList() : new List<string>()))
                .ForMember(dest => dest.CaptainImg, opt => opt.MapFrom(src => src.Captain != null ? src.Captain.ProfilePhotoUrl : null))
                .ForMember(dest => dest.BoatImg, opt => opt.MapFrom(src =>
                    src.Boat != null && src.Boat.Images != null && src.Boat.Images.Any() ? src.Boat.Images.FirstOrDefault().ImageUrl : null));

            CreateMap<Trip, DtoCaptainTripsTable>()
                .ForMember(dest => dest.BoatName, opt => opt.MapFrom(src => src.Boat != null ? src.Boat.Name : string.Empty))
                .ForMember(dest => dest.BoatImg, opt => opt.MapFrom(src =>src.Boat != null && src.Boat.Images != null && src.Boat.Images.Any()?
                src.Boat.Images.First().ImageUrl: null));
           
            CreateMap<Trip, DtoBoatTripsTable>()
                .ForMember(dest => dest.CaptainName, opt => opt.MapFrom(src => src.Captain.FullName))
                .ForMember(dest => dest.CaptainImg, opt => opt.MapFrom(src => src.Captain.ProfilePhotoUrl));
                
        }
    }
}
