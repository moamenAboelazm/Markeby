using AutoMapper;
using Library.Enums;
using Library.IRepository;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Features.Trips.Commands
{
    public class CreateTripCommand : IRequest<Guid>
    {
        public Guid BoatId { get; set; }
        public Guid CaptainId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StartLocation { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int AvailableSeats { get; set; }
        public TripType Type { get; set; }
        public List<IFormFile>? Images { get; set; }
    }

    public class CreateTripCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, IMemoryCache _cache, IFileService _fileService) : IRequestHandler<CreateTripCommand, Guid>
    {
        public async Task<Guid> Handle(CreateTripCommand data, CancellationToken cancellationToken)
        {
            var isBoatAvailable = await _unitOfWork.Boats.IsBoatAvailableAsync(data.BoatId, data.StartTime, data.EndTime);
            if (!isBoatAvailable)
                throw new InvalidOperationException("Boat is not available.");

            var isCaptainAvailable = await _unitOfWork.Captains.IsCaptainAvailableAsync(data.CaptainId, data.StartTime, data.EndTime);
            if (!isCaptainAvailable)
                throw new InvalidOperationException("Captain is not available.");

            var boat = await _unitOfWork.Boats.GetBoatWithDetailsAsync(data.BoatId);
            var captain = await _unitOfWork.Captains.GetByIdAsync(data.CaptainId);

            if (boat != null && captain != null)
            {
                if (!boat.Captains.Any(c => c.Id == data.CaptainId))
                {
                    boat.Captains.Add(captain);
                    _unitOfWork.Boats.Update(boat);
                }
            }

            var trip = _mapper.Map<Trip>(data);
            trip.Status = TripStatus.Scheduled;
            trip.Images ??= new List<TripImage>();

            if (data.Images != null && data.Images.Any())
            {
                foreach (var file in data.Images)
                {
                    var imageUrl = await _fileService.SaveFileAsync(file, "trips");
                    trip.Images.Add(new TripImage { ImageUrl = imageUrl });
                }
            }

            await _unitOfWork.Trips.AddAsync(trip);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AllTripsCacheKey");
            _cache.Remove("AllCaptainsCacheKey");
            _cache.Remove("AllBoatsCacheKey");

            return trip.Id;
        }
    }
}