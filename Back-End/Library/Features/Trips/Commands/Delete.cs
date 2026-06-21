using Library.IRepository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public DeleteTripCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<bool> Handle(DeleteTripCommand data, CancellationToken cancellationToken)
        {
            var trip = await _unitOfWork.Trips.GetByIdAsync(data.Id);
            if (trip == null) return false;

            _unitOfWork.Trips.Delete(trip);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("AvailableUpcomingTripsCacheKey");
            _cache.Remove($"TripDetailsCacheKey_{data.Id}");

            return true;
        }
    }
}
