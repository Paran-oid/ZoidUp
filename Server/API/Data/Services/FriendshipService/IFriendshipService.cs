using API.Models;

namespace API.Data.Services.FriendshipService
{
    public interface IFriendshipService
    {
        public Task<IEnumerable<User>> GetAllFriends(int userId);
        public Task<string> RemoveFriendship(int userId, int friendId);

    }
}
