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
        public DateTime? MeetingTime { get; set; }
        public int AvailableSeats { get; set; }
        public string? IncludedItems { get; set; }
        public string? ExcludedItems { get; set; }
        public TripType Type { get; set; }
        public List<IFormFile>? Images { get; set; }
    }

    public class CreateTripCommandHandler : IRequestHandler<CreateTripCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IFileService _fileService;

        public CreateTripCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
            _fileService = fileService;
        }

        public async Task<Guid> Handle(CreateTripCommand data, CancellationToken cancellationToken)
        {
            var isBoatAvailable = await _unitOfWork.Boats.IsBoatAvailableAsync(data.BoatId, data.StartTime, data.EndTime);
            if (!isBoatAvailable)
                throw new InvalidOperationException("Boat is not available.");

            var isCaptainAvailable = await _unitOfWork.Captains.IsCaptainAvailableAsync(data.CaptainId, data.StartTime, data.EndTime);
            if (!isCaptainAvailable)
                throw new InvalidOperationException("Captain is not available.");

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

            _cache.Remove("AvailableUpcomingTripsCacheKey");

            return trip.Id;
        }
    }
}