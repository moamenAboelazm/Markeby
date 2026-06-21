using Library.IRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Features.Trips.Commands
{
    public class DeleteTripCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteTripCommandHandler : IRequestHandler<DeleteTripCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private readonly IFileService _fileService;

        public DeleteTripCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _fileService = fileService;
        }

        public async Task<bool> Handle(DeleteTripCommand data, CancellationToken cancellationToken)
        {
            var trip = await _unitOfWork.Trips.GetTripWithDetailsByIdAsync(data.Id);
            if (trip == null) return false;

            if (trip.Images != null && trip.Images.Any())
            {
                foreach (var image in trip.Images)
                {
                    _fileService.DeleteFile(image.ImageUrl);
                }
            }

            _unitOfWork.Trips.Delete(trip);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AvailableUpcomingTripsCacheKey");
            _cache.Remove($"TripDetailsCacheKey_{data.Id}");

            return true;
        }
    }
}