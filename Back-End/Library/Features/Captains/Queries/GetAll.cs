using AutoMapper;
using Library.IRepository;
using Library.Models;
using MediatR;

namespace Library.Features.Captains.Queries
{
    public class GetPagedCaptainsQuery : IRequest<PagedResult<CaptainListDto>>
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }

    public class GetPagedCaptainsQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetPagedCaptainsQuery, PagedResult<CaptainListDto>>
    {
        public async Task<PagedResult<CaptainListDto>> Handle(GetPagedCaptainsQuery data, CancellationToken cancellationToken)
        {
            var allCaptains = await _unitOfWork.Captains.GetAllCaptainsWithDetailsAsync();
            var query = allCaptains.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(data.SearchTerm))
            {
                var term = data.SearchTerm.ToLower();
                query = query.Where(c => (c.FullName != null && c.FullName.ToLower().Contains(term)) || (c.Email != null && c.Email.ToLower().Contains(term)) ||
                (c.PhoneNumber != null && c.PhoneNumber.Contains(term)));
            }

            var now = DateTime.UtcNow.AddHours(3);

            if (!string.IsNullOrWhiteSpace(data.Status))
            {
                query = query.Where(c =>
                {
                    bool isOnMission = c.Trips != null && c.Trips.Any(t => t.StartTime <= now && t.EndTime >= now);
                    string status = isOnMission ? "OnMission" : "Available";
                    return status.Equals(data.Status, StringComparison.OrdinalIgnoreCase);
                });
            }

            var totalCount = query.Count();

            var pagedEntities = query.OrderBy(c => c.FullName).Skip((data.PageNumber - 1) * data.PageSize).Take(data.PageSize).ToList();

            var mappedItems = _mapper.Map<List<CaptainListDto>>(pagedEntities);

            return new PagedResult<CaptainListDto>
            {
                Items = mappedItems,
                TotalCount = totalCount,
                PageNumber = data.PageNumber,
                PageSize = data.PageSize
            };
        }
    }
}
