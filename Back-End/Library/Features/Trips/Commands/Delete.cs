using Library.IRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Features.Trips.Commands
{
    public class DeleteTripCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteTripCommandHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache, IFileService _fileService) : IRequestHandler<DeleteTripCommand, bool>
    {
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

            _cache.Remove("AllTripsCacheKey");
            _cache.Remove($"TripDetailsCacheKey_{data.Id}");
            _cache.Remove("AllCaptainsCacheKey");
            _cache.Remove("AllBoatsCacheKey");

            return true;
        }
    }
}