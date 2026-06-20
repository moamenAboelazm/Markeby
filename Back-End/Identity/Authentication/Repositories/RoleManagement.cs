using Identity.Authentication.Base;
using Library.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Authentication.Repositories
{
    public class RoleManagement(UserManager<AppUser> userManager , RoleManager<IdentityRole> roleManager) : IRoleManagement
    {
        public async Task<bool> AddUserToRole(AppUser user, string rolename)
        {
            if (!await roleManager.RoleExistsAsync(rolename))
            {
                await roleManager.CreateAsync(new IdentityRole(rolename));
            }

            return (await userManager.AddToRoleAsync(user, rolename)).Succeeded;
        }

        public async Task<string?> GetUserRole(string UserEmail)
        {
            var user = await userManager.FindByEmailAsync(UserEmail);
            return (await userManager.GetRolesAsync(user!)).FirstOrDefault();
        }
    }
}
