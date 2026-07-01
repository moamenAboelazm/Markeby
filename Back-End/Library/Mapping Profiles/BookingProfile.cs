using AutoMapper;
using Library.Features.Bookings;
using Library.Features.Bookings.Commands;
using Library.Features.Users;
using Library.Models;

namespace Library.Mapping_Profiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<CreateBookingCommand, Booking>()
                            .ForMember(dest => dest.Id, opt => opt.Ignore())
                            .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                            .ForMember(dest => dest.BookingDate, opt => opt.Ignore())
                            .ForMember(dest => dest.CancellationReason, opt => opt.Ignore())
                            .ForMember(dest => dest.User, opt => opt.Ignore())
                            .ForMember(dest => dest.Trip, opt => opt.Ignore());

            CreateMap<Booking, DtoBooking>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : "Unknown"))
                .ForMember(dest => dest.TripTitle, opt => opt.MapFrom(src => src.Trip != null ? src.Trip.Title : "Unknown"));

            CreateMap<Booking, DtoBookingsInProfile>()
                .ForMember(dest => dest.TripTitle, opt => opt.MapFrom(src => src.Trip.Title))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Trip.StartTime))
                .ForMember(dest => dest.TripStatus, opt => opt.MapFrom(src => src.Trip.Status))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString().Substring(0,8)));
                
        }
    }
}
