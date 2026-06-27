using AutoMapper;
using Library.Enums;
using Library.Features.Captains;
using Library.Features.Captains.Commands;
using Library.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers;

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

            CreateMap<Captain, CaptainListDto>()
                .ForMember(dest => dest.Vessel, opt => opt.MapFrom(src =>
                    src.Trips != null && src.Trips.FirstOrDefault(t => t.Status == TripStatus.Ongoing) != null ?
                    src.Trips.FirstOrDefault(t => t.Status == TripStatus.Ongoing).Boat.Name : "UnAssigned"))
                    
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    (src.Trips != null && src.Trips.Any(t => t.Status == TripStatus.Ongoing))
                    ? "ON MISSION" : (src.IsAvailable ? "AVAILABLE" : "ON LEAVE")));
        }
    }
}