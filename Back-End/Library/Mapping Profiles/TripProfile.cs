using AutoMapper;
using Library.Features.Trips;
using Library.Features.Trips.Commands;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Mapping_Profiles
{
    public class TripProfile : Profile
    {
        public TripProfile()
        {
            CreateMap<Trip, DtoTrip>();

            CreateMap<CreateTripCommand, Trip>()
                .ForMember(dest => dest.Boat, opt => opt.Ignore())
                .ForMember(dest => dest.Captain, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Passengers, opt => opt.Ignore());

            CreateMap<UpdateTripCommand, Trip>()
                .ForMember(dest => dest.Boat, opt => opt.Ignore())
                .ForMember(dest => dest.Captain, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Passengers, opt => opt.Ignore());
        }
    }
}
