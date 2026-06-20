using AutoMapper;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Library.Features.Users.Queries
{
    public class GetProfile : IRequest<DtoGetUser>
    {
    }

    public class GetProfileQueryHandler(UserManager<AppUser> _userManager,IHttpContextAccessor _httpContextAccessor,IMapper _mapper) : IRequestHandler<GetProfile, DtoGetUser>
    {
        public async Task<DtoGetUser> Handle(GetProfile data, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Unauthorized access. Please log in.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            var dtoUser = _mapper.Map<DtoGetUser>(user);
            dtoUser.Roles = await _userManager.GetRolesAsync(user);

            return dtoUser;
        }
    }


}
