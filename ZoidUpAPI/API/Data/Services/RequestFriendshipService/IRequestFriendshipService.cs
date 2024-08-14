using API.Models;
using API.Models.DTOs;

namespace API.Data.Services.RequestFriendshipService
{
    public interface IRequestFriendshipService
    {
        public Task<IEnumerable<User>>? GetAllFriends(int userID);
        public Task<IEnumerable<RequestUserDTO>>? GetAllReceivedRequests(int receiverID);
        public Task<IEnumerable<RequestUserDTO>>? GetAllSentRequests(int senderID);
        public Task<string> SendRequest(int SenderID, int ReceiverID);
        public Task<string> RemoveRequest(int SenderID, int ReceiverID);
        public Task<string> RemoveFriendship(int userID, int friendID);

    }
}
