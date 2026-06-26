using Library.Enums;
using Library.IRepository;
using Library.Models;
using MediatR;

namespace Library.Features.Trips.Queries
{
    public class GetAvailableResourcesQuery : IRequest<AvailableResourcesDto>
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class GetAvailableResourcesQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetAvailableResourcesQuery, AvailableResourcesDto>
    {
        public async Task<AvailableResourcesDto> Handle(GetAvailableResourcesQuery data, CancellationToken cancellationToken)
        {
            var allTrips = await _unitOfWork.Trips.GetAllAsync();

            var busyBoatIds = allTrips
                .Where(t => t.StartTime < data.EndTime && data.StartTime < t.EndTime && t.Status != TripStatus.Cancelled)
                .Select(t => t.BoatId).Distinct().ToList();

            var busyCaptainIds = allTrips
                .Where(t => t.StartTime < data.EndTime && data.StartTime < t.EndTime && t.Status != TripStatus.Cancelled)
                .Select(t => t.CaptainId).Distinct().ToList();

            var allBoats = await _unitOfWork.Boats.GetAllAsync();
            var availableBoats = allBoats.Where(b => !busyBoatIds.Contains(b.Id))
                .Select(b => new AvailableBoatDto
                {
                    Id = b.Id,
                    Name = b.Name
                }).ToList();

            var allCaptains = await _unitOfWork.Captains.GetAllAsync(); 
            var availableCaptains = allCaptains.Where(c => !busyCaptainIds.Contains(c.Id) && c.IsAvailable)
                .Select(c => new AvailableCaptainDto
                {
                    Id = c.Id,
                    FullName = c.FullName
                }).ToList();

            return new AvailableResourcesDto
            {
                Captains = availableCaptains,
                Boats = availableBoats
            };
        }
    }
}
