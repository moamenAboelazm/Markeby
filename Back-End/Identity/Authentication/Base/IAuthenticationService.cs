
using library.DTOs;

namespace Identity.Authentication.Base
{
    public interface IAuthenticationService
    {
        Task<DtoResponse> CreateUser(DtoCreateUser user);
        Task<DtoLoginResponse> LoginUser(DtoLoginUser user);
        Task<DtoLoginResponse> RetrieveToken(string refreshToken);
    }
}
