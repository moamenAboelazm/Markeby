using AutoMapper;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Features.Users.Queries
{
    public class GetPagedUsersQuery : IRequest<PagedResult<DtoGetUser>>
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool SortDescending { get; set; } = false;
    }

    public class GetPagedUsersQueryHandler(UserManager<AppUser> _userManager, IMapper _mapper) : IRequestHandler<GetPagedUsersQuery, PagedResult<DtoGetUser>>
    {
        public async Task<PagedResult<DtoGetUser>> Handle(GetPagedUsersQuery data, CancellationToken cancellationToken)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(data.SearchTerm))
            {
                var term = data.SearchTerm.ToLower();
                query = query.Where(u =>
                    (u.FirstName != null && u.FirstName.ToLower().Contains(term)) ||
                    (u.LastName != null && u.LastName.ToLower().Contains(term)) ||
                    (u.Email != null && u.Email.ToLower().Contains(term)) ||
                    (u.UserName != null && u.UserName.ToLower().Contains(term)||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(term))                                                           )
                );
            }

            query = data.SortDescending ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id);

            var totalCount = await query.CountAsync(cancellationToken);

            var pagedUsers = await query.Skip((data.PageNumber - 1) * data.PageSize).Take(data.PageSize).ToListAsync(cancellationToken);

            var dtoUsers = new List<DtoGetUser>();

            foreach (var user in pagedUsers)
            {
                var dtoUser = _mapper.Map<DtoGetUser>(user);
                dtoUser.Roles = await _userManager.GetRolesAsync(user);
                dtoUsers.Add(dtoUser);
            }

            return new PagedResult<DtoGetUser>
            {
                Items = dtoUsers,
                TotalCount = totalCount,
                PageNumber = data.PageNumber,
                PageSize = data.PageSize
            };
        }
    }
}