using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Library.Features.Users.Commands
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
    public class ChangePasswordCommandHandler( UserManager<AppUser> _userManager, IHttpContextAccessor _httpContextAccessor) : IRequestHandler<ChangePasswordCommand, bool>
    {
        public async Task<bool> Handle(ChangePasswordCommand data, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Unauthorized access. Please log in.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            var result = await _userManager.ChangePasswordAsync(user, data.CurrentPassword, data.NewPassword);
            if (!result.Succeeded)
                throw new Exception("Failed to change password. Ensure your current password is correct.");

            return true;
        }
    }
}
