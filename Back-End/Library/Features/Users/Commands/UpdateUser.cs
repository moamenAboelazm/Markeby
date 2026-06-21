using AutoMapper;
using Library.Enums;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Library.Features.Users.Commands
{
    public class UpdateProfileCommand : IRequest<bool>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public GenderType Gender { get; set; }
        public NationalityType Nationality { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImgUrl { get; set; }
    }

    public class UpdateProfileCommandHandler(UserManager<AppUser> _userManager,IHttpContextAccessor _httpContextAccessor,IMapper _mapper) : IRequestHandler<UpdateProfileCommand, bool>
    {
        public async Task<bool> Handle(UpdateProfileCommand data, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Unauthorized access. Please log in.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            _mapper.Map(data, user);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("Failed to update profile data.");

            return true;
        }
    }
}
