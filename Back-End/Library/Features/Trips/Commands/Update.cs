using AutoMapper;
using Library.Enums;
using Library.IRepository;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Features.Trips.Commands
{
    public class UpdateTripCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
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
        public TripStatus Status { get; set; }
        public List<IFormFile>? Images { get; set; }
    }

    public class UpdateTripCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, IMemoryCache _cache, IFileService _fileService) : IRequestHandler<UpdateTripCommand, bool>
    {
        public async Task<bool> Handle(UpdateTripCommand data, CancellationToken cancellationToken)
        {
            var trip = await _unitOfWork.Trips.GetTripWithDetailsByIdAsync(data.Id);
            if (trip == null) return false;

            if (trip.BoatId != data.BoatId || trip.StartTime != data.StartTime || trip.EndTime != data.EndTime)
            {
                var isBoatAvailable = await _unitOfWork.Boats.IsBoatAvailableAsync(data.BoatId, data.StartTime, data.EndTime);
                if (!isBoatAvailable) throw new InvalidOperationException("Boat is not available.");
            }

            if (trip.CaptainId != data.CaptainId || trip.StartTime != data.StartTime || trip.EndTime != data.EndTime)
            {
                var isCaptainAvailable = await _unitOfWork.Captains.IsCaptainAvailableAsync(data.CaptainId, data.StartTime, data.EndTime);
                if (!isCaptainAvailable) throw new InvalidOperationException("Captain is not available.");
            }

            _mapper.Map(data, trip);
            trip.Images ??= new List<TripImage>();

            if (data.Images != null && data.Images.Any())
            {
                if (trip.Images.Any())
                {
                    foreach (var oldImage in trip.Images)
                        _fileService.DeleteFile(oldImage.ImageUrl);
                    
                    trip.Images.Clear();
                }

                foreach (var file in data.Images)
                {
                    var imageUrl = await _fileService.SaveFileAsync(file, "trips");
                    trip.Images.Add(new TripImage { ImageUrl = imageUrl });
                }
            }

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

            _unitOfWork.Trips.Update(trip);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AllTripsCacheKey");
            _cache.Remove($"TripDetailsCacheKey_{data.Id}");
            _cache.Remove("AllCaptainsCacheKey");
            _cache.Remove("AllBoatsCacheKey");
            return true;
        }
    }
}