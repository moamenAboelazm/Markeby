using AutoMapper;
using Library.Enums;
using Library.IRepository;
using Library.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Boats.Commands
{
    public class CreateBoatCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? MainImageUrl { get; set; }
        public int YearBuilt { get; set; }
        public bool HasWifi { get; set; }
        public bool HasFoodFacility { get; set; }
        public bool HasToilet { get; set; }
        public List<Guid> CaptainIds { get; set; } = new List<Guid>();
    }

    public class CreateBoatCommandHandler : IRequestHandler<CreateBoatCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateBoatCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateBoatCommand data, CancellationToken cancellationToken)
        {
            var boat = _mapper.Map<Boat>(data);
            boat.Status = BoatStatus.Active;

            if (data.CaptainIds != null && data.CaptainIds.Any())
            {
                foreach (var captainId in data.CaptainIds)
                {
                    var captain = await _unitOfWork.Captains.GetByIdAsync(captainId);
                    if (captain != null)
                    {
                        boat.Captains.Add(captain);
                    }
                }
            }

            await _unitOfWork.Boats.AddAsync(boat);
            await _unitOfWork.CompleteAsync();

            return boat.Id;
        }
    }
}
