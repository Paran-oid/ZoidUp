using API.Models;
using API.Models.DTOs.AuthRelated;

namespace API.Data.Services.AuthService
{
    public interface IAuthService
    {
        public Task<object> GetUser(string token);
        public Task<AccessTokenResponse> Register(RegisterEntry model);
        public Task<AccessTokenResponse> Login(LoginEntry model);
    }
}
