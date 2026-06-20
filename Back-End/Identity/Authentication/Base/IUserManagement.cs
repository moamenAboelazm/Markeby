using Library.Models;
using System.Security.Claims;

namespace Identity.Authentication.Base
{
    public interface IUserManagement
    {
        Task<bool> CreateUser(AppUser user);
        Task<bool> LoginUser(AppUser user);
        Task<AppUser?> GetUserByEmail(string email);
        Task<AppUser?> GetUserById(string id);
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<int> DeleteUserByEmail(string id);
        Task<List<Claim>> GetUserClaims(string email);
    }
}
