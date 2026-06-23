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

    public class GetPagedCaptainsQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetPagedCaptainsQuery, PagedResult<CaptainListDto>>
    {
        public async Task<PagedResult<CaptainListDto>> Handle(GetPagedCaptainsQuery data, CancellationToken cancellationToken)
        {
            var allCaptains = await _unitOfWork.Captains.GetAllCaptainsWithDetailsAsync();
            var query = allCaptains.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(data.SearchTerm))
            {
                var term = data.SearchTerm.ToLower();
                query = query.Where(c => c.FullName.ToLower().Contains(term) || c.Email.ToLower().Contains(term) || c.PhoneNumber.Contains(term));
            }

            var now = DateTime.UtcNow;

            var pagedDataList = query.Select(c =>
            {
                bool isOnMission = c.Trips != null && c.Trips.Any(t => t.StartTime <= now && t.EndTime >= now);
                string status = isOnMission ? "ACTIVE" : (c.IsAvailable ? "ACTIVE" : "AVAILABLE");
                string vessel = c.Boats != null && c.Boats.Any() ? c.Boats.First().Name : "Unassigned";

                return new CaptainListDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.Email,
                    Rank = c.Rank,
                    Vessel = vessel,
                    Status = status,
                    PhoneNumber = c.PhoneNumber,
                    ProfilePhotoUrl = c.ProfilePhotoUrl
                };
            });

            if (!string.IsNullOrWhiteSpace(data.Status))
                pagedDataList = pagedDataList.Where(c => c.Status.Equals(data.Status, StringComparison.OrdinalIgnoreCase));

            var totalCount = pagedDataList.Count();

            var pagedData = pagedDataList.OrderBy(c => c.FullName).Skip((data.PageNumber - 1) * data.PageSize).Take(data.PageSize).ToList();

            return new PagedResult<CaptainListDto>
            {
                Items = pagedData,
                TotalCount = totalCount,
                PageNumber = data.PageNumber,
                PageSize = data.PageSize
            };
        }
    }
}
