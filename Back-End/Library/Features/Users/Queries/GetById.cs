using AutoMapper;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Features.Users.Queries
{
    public class GetUserById : IRequest<DtoGetUserProfile>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class GetUserByIdQueryHandler(UserManager<AppUser> _userManager, IMapper _mapper) : IRequestHandler<GetUserById, DtoGetUserProfile>
    {
        public async Task<DtoGetUserProfile> Handle(GetUserById data, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.AsNoTracking().Include(u => u.Bookings).ThenInclude(b => b.Trip)
                .FirstOrDefaultAsync(u => u.Id == data.Id, cancellationToken);

            if (user == null)
                throw new Exception("User not found.");

            var dtoUser = _mapper.Map<DtoGetUserProfile>(user);
            dtoUser.Roles = await _userManager.GetRolesAsync(user);

            return dtoUser;
        }
    }

}
