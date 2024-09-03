using API.Models;

namespace API.Data.Services.UserService
{
    public interface IUserService
    {
        public Task<User> GetUser(int userId);
    }
}
