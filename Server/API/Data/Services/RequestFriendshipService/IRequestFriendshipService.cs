using API.Models;
using API.Models.DTOs;

namespace API.Data.Services.RequestFriendshipService
{
    public interface IRequestFriendshipService
    {
        public Task<bool> HasRequests(int userId);
        public Task<IEnumerable<User>> GetAllRecommended(int userId);
        public Task<IEnumerable<RequestUserDTO>> GetAllReceivedRequests(int receiverId);
        public Task<IEnumerable<RequestUserDTO>> GetAllSentRequests(int senderId);
        public Task<string> SendRequest(int senderId, int receiverId);
        public Task<string> UnsendRequest(int senderId, int receiverId);

    }
}
