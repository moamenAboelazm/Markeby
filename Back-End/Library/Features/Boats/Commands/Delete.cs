using Library.IRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Library.Features.Boats.Commands
{
    public class DeleteBoatCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteBoatCommandHandler : IRequestHandler<DeleteBoatCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public DeleteBoatCommandHandler(IUnitOfWork unitOfWork , IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<bool> Handle(DeleteBoatCommand data, CancellationToken cancellationToken)
        {
            var boat = await _unitOfWork.Boats.GetByIdAsync(data.Id);

            if (boat == null) return false;

            _unitOfWork.Boats.Delete(boat);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AllBoatsCacheKey");
            _cache.Remove("ActiveBoatsCacheKey");
            _cache.Remove($"BoatDetailsCacheKey_{data.Id}");

            return true;
        }
    }
}
