using API.Models;
using API.Models.DTOs;

namespace API.Data.Services.RequestFriendshipService
{
    public interface IRequestFriendshipService
    {
        public Task<bool> HasRequests(int userID);
        public Task<IEnumerable<User>>? GetAllRecommendedFriends(int userID);
        public Task<IEnumerable<User>>? GetAllFriends(int userID);
        public Task<IEnumerable<RequestUserDTO>>? GetAllReceivedRequests(int receiverID);
        public Task<IEnumerable<RequestUserDTO>>? GetAllSentRequests(int senderID);
        public Task<string> SendRequest(int senderID, int receiverID);
        public Task<string> UnsendRequest(int senderID, int receiverID);
        public Task<string> RemoveFriendship(int userID, int friendID);

    }
}
