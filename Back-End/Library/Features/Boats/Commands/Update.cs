using AutoMapper;
using Library.Enums;
using Library.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Boats.Commands
{
    public class UpdateBoatCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? MainImageUrl { get; set; }
        public int YearBuilt { get; set; }
        public bool HasWifi { get; set; }
        public bool HasFoodFacility { get; set; }
        public bool HasToilet { get; set; }
        public BoatStatus Status { get; set; }
        public List<Guid> CaptainIds { get; set; } = new List<Guid>();
    }
    public class UpdateBoatCommandHandler : IRequestHandler<UpdateBoatCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateBoatCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateBoatCommand data, CancellationToken cancellationToken)
        {
            var boat = await _unitOfWork.Boats.GetBoatWithDetailsAsync(data.Id);

            if (boat == null) return false;

            _mapper.Map(data, boat);

            boat.Captains.Clear();

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

            _unitOfWork.Boats.Update(boat);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
