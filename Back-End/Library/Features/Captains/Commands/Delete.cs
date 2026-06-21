using Library.IRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Captains.Commands
{
    public class DeleteCaptainCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteCaptainCommandHandler : IRequestHandler<DeleteCaptainCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public DeleteCaptainCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<bool> Handle(DeleteCaptainCommand request, CancellationToken cancellationToken)
        {
            var captain = await _unitOfWork.Captains.GetByIdAsync(request.Id);

            if (captain == null) return false;

            _unitOfWork.Captains.Delete(captain);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AllCaptainsCacheKey");
            _cache.Remove($"CaptainDetailsCacheKey_{request.Id}");

            return true;
        }
    }
}
