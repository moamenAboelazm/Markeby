
using Library.Models;

namespace Identity.Authentication.Base
{
    public interface IRoleManagement
    {
        Task<string?> GetUserRole(string UserEmail);
        Task<bool> AddUserToRole(AppUser user , string rolename);
    }
}
