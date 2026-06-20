using AutoMapper;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Features.Users.Queries
{
    public class GetAllUsers : IRequest<IEnumerable<DtoGetUser>>
    {
    }

    public class GetAllUsersQueryHandler(UserManager<AppUser> _userManager, IMapper _mapper) : IRequestHandler<GetAllUsers, IEnumerable<DtoGetUser>>
    {
        public async Task<IEnumerable<DtoGetUser>> Handle(GetAllUsers data, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.ToListAsync(cancellationToken);
            var dtoUsers = new List<DtoGetUser>();

            foreach (var user in users)
            {
                var dtoUser = _mapper.Map<DtoGetUser>(user);
                dtoUser.Roles = await _userManager.GetRolesAsync(user);
                dtoUsers.Add(dtoUser);
            }

            return dtoUsers;
        }
    }
}
