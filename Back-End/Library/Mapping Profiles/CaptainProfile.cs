using AutoMapper;
using Library.Features.Captains;
using Library.Features.Captains.Commands;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Mapping_Profiles
{
    public class CaptainProfile : Profile
    {
        public CaptainProfile()
        {
            CreateMap<Captain, DtoCaptain>();

            CreateMap<CreateCaptainCommand, Captain>()
                .ForMember(dest => dest.Boats, opt => opt.Ignore())
                .ForMember(dest => dest.Trips, opt => opt.Ignore());

            CreateMap<UpdateCaptainCommand, Captain>()
                .ForMember(dest => dest.Boats, opt => opt.Ignore())
                .ForMember(dest => dest.Trips, opt => opt.Ignore());
        }
    }
}
