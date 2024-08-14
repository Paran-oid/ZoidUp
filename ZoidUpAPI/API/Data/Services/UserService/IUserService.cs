using API.Models;
using API.Models.Others.AuthRelated;

namespace API.Data.Services.UserService
{
    public interface IUserService
    {
        public Task<object?> GetUser(string token);
        public Task<AccessTokenResponse?> Register(RegisterEntry model);
        public Task<AccessTokenResponse?> Login(LoginEntry model);
    }
}
