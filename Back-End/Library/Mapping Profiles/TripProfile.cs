using AutoMapper;
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
                .ForMember(dest => dest.BoatSeats , opt => opt.MapFrom(src => src.Boat.Capacity))
                .ForMember(dest => dest.BoatName, opt => opt.MapFrom(src => src.Boat != null ? src.Boat.Name : "Unassigned"))
                .ForMember(dest => dest.CaptainName, opt => opt.MapFrom(src => src.Captain != null ? src.Captain.FullName : "Unassigned"));
        }
    }
}
