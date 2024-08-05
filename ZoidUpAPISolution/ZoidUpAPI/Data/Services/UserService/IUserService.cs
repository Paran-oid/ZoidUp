using ZoidUpAPI.Models;
using ZoidUpAPI.Models.Others.AuthRelated;

namespace ZoidUpAPI.Data.Services.UserService
{
    public interface IUserService
    {
        public Task<List<User>?> GetAllUsers();
        public Task<User?> GetUser(string token);
        public Task<AccessTokenResponse?> Register(RegisterEntry model);
        public Task<AccessTokenResponse?> Login(LoginEntry model);
    }
}
