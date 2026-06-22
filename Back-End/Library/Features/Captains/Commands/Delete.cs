using Library.IRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Features.Captains.Commands
{
    public class DeleteCaptainCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteCaptainCommandHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) : IRequestHandler<DeleteCaptainCommand, bool>
    {
        public async Task<bool> Handle(DeleteCaptainCommand data, CancellationToken cancellationToken)
        {
            var captain = await _unitOfWork.Captains.GetByIdAsync(data.Id);

            if (captain == null) return false;

            _unitOfWork.Captains.Delete(captain);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AllCaptainsCacheKey");
            _cache.Remove($"CaptainDetailsCacheKey_{data.Id}");

            return true;
        }
    }
}
