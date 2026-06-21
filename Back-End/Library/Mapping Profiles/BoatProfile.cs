using AutoMapper;
using Library.Features.Boats;
using Library.Features.Boats.Commands;
using Library.Features.Captains;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Mapping_Profiles
{
    public class BoatProfile : Profile
    {
        public BoatProfile()
        {
            CreateMap<Boat,DtoBoat>();

            CreateMap<CreateBoatCommand, Boat>()
                .ForMember(dest => dest.Captains, opt => opt.Ignore())
                .ForMember(dest => dest.Trips, opt => opt.Ignore());

            CreateMap<UpdateBoatCommand, Boat>()
                .ForMember(dest => dest.Captains, opt => opt.Ignore())
                .ForMember(dest => dest.Trips, opt => opt.Ignore());
        }
    }
}
