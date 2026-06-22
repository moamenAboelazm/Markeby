using AutoMapper;
using Library.IRepository;
using Library.Models;
using MediatR;

namespace Library.Features.Boats.Queries
{
    public class GetAllBoatsQuery : IRequest<PagedResult<DtoBoat>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Name { get; set; }
        public int? Capacity { get; set; }
        public string? Status { get; set; }
        public int? YearBuilt { get; set; }
        //public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }

    public class GetAllBoatsQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllBoatsQuery, PagedResult<DtoBoat>>
    {
        public async Task<PagedResult<DtoBoat>> Handle(GetAllBoatsQuery data, CancellationToken cancellationToken)
        {
            var boats = await _unitOfWork.Boats.GetAllBoatsAsync();
            var queryableBoats = boats.AsQueryable();

            if (!string.IsNullOrEmpty(data.Name))
            {
                queryableBoats = queryableBoats.Where(b => b.Name.Contains(data.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (data.Capacity.HasValue)
            {
                queryableBoats = queryableBoats.Where(b => b.Capacity >= data.Capacity.Value);
            }

            if (!string.IsNullOrEmpty(data.Status))
            {
                queryableBoats = queryableBoats.Where(b => b.Status.ToString().Equals(data.Status, StringComparison.OrdinalIgnoreCase));
            }

            if (data.YearBuilt.HasValue)
            {
                queryableBoats = queryableBoats.Where(b => b.YearBuilt == data.YearBuilt.Value);
            }

            /*
               if (!string.IsNullOrEmpty(data.SortBy))
              {
                  queryableBoats = data.SortBy.ToLower() switch
                  {
                      "name" => data.SortDescending ? queryableBoats.OrderByDescending(b => b.Name) : queryableBoats.OrderBy(b => b.Name),
                      "capacity" => data.SortDescending ? queryableBoats.OrderByDescending(b => b.Capacity) : queryableBoats.OrderBy(b => b.Capacity),
                      "status" => data.SortDescending ? queryableBoats.OrderByDescending(b => b.Status) : queryableBoats.OrderBy(b => b.Status),
                      "yearbuilt" => data.SortDescending ? queryableBoats.OrderByDescending(b => b.YearBuilt) : queryableBoats.OrderBy(b => b.YearBuilt),
                      _ => data.SortDescending ? queryableBoats.OrderByDescending(b => b.Id) : queryableBoats.OrderBy(b => b.Id)
                  };
              }
              else
                  queryableBoats = data.SortDescending ? queryableBoats.OrderByDescending(b => b.Id) : queryableBoats.OrderBy(b => b.Id);
             */


            var totalCount = queryableBoats.Count();
            var pagedBoats = queryableBoats.Skip((data.PageNumber - 1) * data.PageSize).Take(data.PageSize).ToList();

            return new PagedResult<DtoBoat>
            {
                Items = _mapper.Map<List<DtoBoat>>(pagedBoats),
                TotalCount = totalCount,
                PageNumber = data.PageNumber,
                PageSize = data.PageSize
            };
        }
    }

}
