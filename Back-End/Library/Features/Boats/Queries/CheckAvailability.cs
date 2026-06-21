using Library.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Boats.Queries
{
    public class CheckBoatAvailabilityQuery : IRequest<bool>
    {
        public Guid BoatId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class CheckBoatAvailabilityQueryHandler : IRequestHandler<CheckBoatAvailabilityQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckBoatAvailabilityQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CheckBoatAvailabilityQuery data, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Boats.IsBoatAvailableAsync(
                data.BoatId,
                data.StartTime,
                data.EndTime
            );
        }
    }


}
