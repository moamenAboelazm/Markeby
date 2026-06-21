using Library.IRepository;
using Library.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dashboards.Captains
{
    public class GetPagedCaptainsQuery : IRequest<PagedResult<CaptainListDto>>
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }

    public class GetPagedCaptainsQueryHandler : IRequestHandler<GetPagedCaptainsQuery, PagedResult<CaptainListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPagedCaptainsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<CaptainListDto>> Handle(GetPagedCaptainsQuery request, CancellationToken cancellationToken)
        {
            var allCaptains = await _unitOfWork.Captains.GetAllCaptainsWithDetailsAsync();
            var query = allCaptains.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                query = query.Where(c => c.FullName.ToLower().Contains(term) || c.Email.ToLower().Contains(term));
            }

            var now = DateTime.UtcNow;

            var pagedDataList = query.Select(c =>
            {
                bool isOnMission = c.Trips != null && c.Trips.Any(t => t.StartTime <= now && t.EndTime >= now);
                string status = isOnMission ? "ACTIVE" : (c.IsAvailable ? "ACTIVE" : "ON LEAVE");
                string vessel = c.Boats != null && c.Boats.Any() ? c.Boats.First().Name : "Unassigned";

                return new CaptainListDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.Email,
                    Rank = c.Rank,
                    Vessel = vessel,
                    Status = status,
                    ProfilePhotoUrl = c.ProfilePhotoUrl
                };
            });

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                pagedDataList = pagedDataList.Where(c => c.Status.Equals(request.Status, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = pagedDataList.Count();

            var pagedData = pagedDataList
                .OrderBy(c => c.FullName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResult<CaptainListDto>
            {
                Items = pagedData,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
