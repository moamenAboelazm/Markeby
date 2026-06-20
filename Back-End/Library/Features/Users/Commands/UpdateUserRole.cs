using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Library.Features.Users.Commands
{
    public class UpdateUserRoleCommand : IRequest<bool>
    {
        public string UserId { get; set; } = string.Empty;
        public string TargetRole { get; set; } = string.Empty;
    }

    public class UpdateUserRoleCommandHandler(
    UserManager<AppUser> _userManager,
    RoleManager<IdentityRole> _roleManager) : IRequestHandler<UpdateUserRoleCommand, bool>
    {
        public async Task<bool> Handle(UpdateUserRoleCommand data, CancellationToken cancellationToken)
        {
            if (data.TargetRole != "Admin" && data.TargetRole != "User")
                throw new Exception("Invalid role name. Only 'Admin' or 'User' are allowed.");

            var user = await _userManager.FindByIdAsync(data.UserId);
            if (user == null)
                throw new Exception("User not found.");

            var roleExists = await _roleManager.RoleExistsAsync(data.TargetRole);
            if (!roleExists)
                throw new Exception("The target role does not exist in the system.");

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                throw new Exception("Failed to reset current roles.");

            var addResult = await _userManager.AddToRoleAsync(user, data.TargetRole);
            if (!addResult.Succeeded)
                throw new Exception("Failed to assign the new role.");

            return true;
        }
    }

}
