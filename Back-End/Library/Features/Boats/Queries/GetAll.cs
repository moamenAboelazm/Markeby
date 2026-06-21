using AutoMapper;
using Library.IRepository;
using Library.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Features.Boats.Queries
{
    public class GetAllActiveBoatsQuery : IRequest<PagedResult<DtoBoat>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Name { get; set; }
        public int? Capacity { get; set; }
        public string? Status { get; set; }
        public int? YearBuilt { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }

    public class GetAllActiveBoatsQueryHandler : IRequestHandler<GetAllActiveBoatsQuery, PagedResult<DtoBoat>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllActiveBoatsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<DtoBoat>> Handle(GetAllActiveBoatsQuery request, CancellationToken cancellationToken)
        {
            var boats = await _unitOfWork.Boats.GetActiveBoatsAsync();
            var queryableBoats = boats.AsQueryable();

            if (!string.IsNullOrEmpty(request.Name))
            {
                queryableBoats = queryableBoats.Where(b => b.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (request.Capacity.HasValue)
            {
                queryableBoats = queryableBoats.Where(b => b.Capacity >= request.Capacity.Value);
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                queryableBoats = queryableBoats.Where(b => b.Status.ToString().Equals(request.Status, StringComparison.OrdinalIgnoreCase));
            }

            if (request.YearBuilt.HasValue)
            {
                queryableBoats = queryableBoats.Where(b => b.YearBuilt == request.YearBuilt.Value);
            }

            if (!string.IsNullOrEmpty(request.SortBy))
            {
                queryableBoats = request.SortBy.ToLower() switch
                {
                    "name" => request.SortDescending ? queryableBoats.OrderByDescending(b => b.Name) : queryableBoats.OrderBy(b => b.Name),
                    "capacity" => request.SortDescending ? queryableBoats.OrderByDescending(b => b.Capacity) : queryableBoats.OrderBy(b => b.Capacity),
                    "status" => request.SortDescending ? queryableBoats.OrderByDescending(b => b.Status) : queryableBoats.OrderBy(b => b.Status),
                    "yearbuilt" => request.SortDescending ? queryableBoats.OrderByDescending(b => b.YearBuilt) : queryableBoats.OrderBy(b => b.YearBuilt),
                    _ => request.SortDescending ? queryableBoats.OrderByDescending(b => b.Id) : queryableBoats.OrderBy(b => b.Id)
                };
            }
            else
                queryableBoats = request.SortDescending ? queryableBoats.OrderByDescending(b => b.Id) : queryableBoats.OrderBy(b => b.Id);
            

            var totalCount = queryableBoats.Count();
            var pagedBoats = queryableBoats.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

            return new PagedResult<DtoBoat>
            {
                Items = _mapper.Map<List<DtoBoat>>(pagedBoats),
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
