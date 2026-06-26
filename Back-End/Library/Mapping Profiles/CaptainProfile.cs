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

            CreateMap<Captain, CaptainListDto>()
                .ForMember(dest => dest.Vessel, opt => opt.MapFrom(src =>
                    src.Boats != null && src.Boats.Any() ? src.Boats.First().Name : "Unassigned"))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    (src.Trips != null && src.Trips.Any(t => t.StartTime <= DateTime.UtcNow.AddHours(3) && t.EndTime >= DateTime.UtcNow.AddHours(3)))
                    ? "ON MISSION" : (src.IsAvailable ? "AVAILABLE" : "ON LEAVE")));
        }
    }
}