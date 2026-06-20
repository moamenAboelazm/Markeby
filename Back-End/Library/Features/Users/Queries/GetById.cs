using AutoMapper;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Library.Features.Users.Queries
{
    public class GetUserById : IRequest<DtoGetUser>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class GetUserByIdQueryHandler(UserManager<AppUser> _userManager, IMapper _mapper) : IRequestHandler<GetUserById, DtoGetUser>
    {
        public async Task<DtoGetUser> Handle(GetUserById data, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(data.Id);

            if (user == null)
                return null;
            
            var dtoUser = _mapper.Map<DtoGetUser>(user);
            dtoUser.Roles = await _userManager.GetRolesAsync(user);

            return dtoUser;
        }
    }

}
