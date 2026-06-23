using Library.IRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Features.Boats.Commands
{
    public class DeleteBoatCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteBoatCommandHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache, IFileService _fileService) : IRequestHandler<DeleteBoatCommand, bool>
    {
        public async Task<bool> Handle(DeleteBoatCommand data, CancellationToken cancellationToken)
        {
            var boat = await _unitOfWork.Boats.GetByIdAsync(data.Id);

            if (boat == null) return false;

            if (boat.Images != null && boat.Images.Any())
            {
                foreach (var image in boat.Images)
                {
                    _fileService.DeleteFile(image.ImageUrl);
                }
            }

            _unitOfWork.Boats.Delete(boat);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AllBoatsCacheKey");
            _cache.Remove("ActiveBoatsCacheKey");
            _cache.Remove($"BoatDetailsCacheKey_{data.Id}");

            return true;
        }
    }
}
