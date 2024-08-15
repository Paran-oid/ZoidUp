using API.Models;
using API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Data.Services.RequestFriendshipService
{
    public class RequestFriendshipService : IRequestFriendshipService
    {
        private readonly AppDbContext _context;
        public RequestFriendshipService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> HasRequests(int userID)
        {
            IEnumerable<RequestUserDTO> requestsEnum = await this.GetAllReceivedRequests(userID);
            int size = requestsEnum
                .ToList()
                .Count;
            return size > 0;
        }

        public async Task<IEnumerable<User>>? GetAllFriends(int userID)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ID == userID);
            if (user == null)
            {
                return null;
            }
            var friendsIDs1 = await _context.Friendships
                .Where(f => f.UserID == userID)
                .Select(f => f.FriendID)
                .ToListAsync();

            var friendsIDs2 = await _context.Friendships
                .Where(f => f.FriendID == userID)
                .Select(f => f.UserID)
                .ToListAsync();

            var friendsIDs = friendsIDs1.Concat(friendsIDs2).ToList();

            var friends = await _context.Users
                .Where(u => friendsIDs.Contains(u.ID))
                .ToListAsync();

            return friends;
        }

        public async Task<IEnumerable<RequestUserDTO>>? GetAllReceivedRequests(int receiverID)
        {
            var temp = await _context.Users.FirstOrDefaultAsync(u => u.ID == receiverID);
            if (temp == null)
            {
                return null;
            }
            // Retrieve all friendship requests received by a specific user
            var requests = _context.Requests
                // Filter requests by the receiver's ID
                .Where(u => u.ReceiverID == receiverID)
                // Select only the SenderID and RequestedOn date for each request
                .Select(u => new { u.SenderID, u.RequestedOn })
                // Execute the query and convert the results to an array
                .ToArray();

            var senders = requests.Select(u => u.SenderID).ToArray();

            var users = _context.Users
                    .Where(u => senders.Contains(u.ID))
                    .Select(u => new { u.ID, u.Username });

            var result = from request in requests
                         join user in users on request.SenderID equals user.ID
                         select new RequestUserDTO
                         {
                             username = user.Username,
                             time = request.RequestedOn
                         };

            return result;
        }

        public async Task<IEnumerable<User>>? GetAllRecommendedFriends(int userID)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ID == userID);
            if (user == null)
            {
                return null;
            }
            var friendsEnum = await this.GetAllFriends(userID);
            var sentRequestsEnum = await this.GetAllSentRequests(userID);

            List<string> friends = friendsEnum
                .Select(f => f.Username)
                .ToList();

            List<string> sentRequests = sentRequestsEnum
                .Select(f => f.username)
                .ToList();

            List<string> total = friends.Concat(sentRequests)
                .ToList();

            var users = await _context.Users
                   .Where(u => !total.Contains(u.Username))
                   .ToListAsync();

            users.Remove(user);

            return users;
        }

        public async Task<IEnumerable<RequestUserDTO>>? GetAllSentRequests(int senderID)
        {
            var temp = await _context.Users.FirstOrDefaultAsync(u => u.ID == senderID);
            if (temp == null)
            {
                return null;
            }

            //get all requests
            var requests = _context.Requests
                .Where(u => u.SenderID == senderID)
                .Select(u => new { u.ReceiverID, u.RequestedOn })
                .ToArray();

            //select all receivers
            var receivers = requests.Select(u => u.ReceiverID);

            //select all users with receiverID
            var users = _context.Users
                .Where(u => receivers.Contains(u.ID))
                .Select(u => new
                {
                    u.ID,
                    u.Username,
                    u.ProfilePicturePath
                });

            //now we join and return request user dto
            var result = from request in requests
                         join user in users on request.ReceiverID equals user.ID
                         select new RequestUserDTO
                         {
                             id = user.ID,
                             username = user.Username,
                             time = request.RequestedOn,
                             profilePicturePath = user.ProfilePicturePath
                         };


            return result;
        }

        public async Task<string> RemoveFriendship(int userID, int friendID)
        {
            var friendship = await _context.Friendships.FirstOrDefaultAsync(f => f.UserID == userID && f.FriendID == friendID);
            if (friendship == null)
            {
                return null;
            }
            _context.Remove(friendship);
            await _context.SaveChangesAsync();

            return "success!";
        }

        public async Task<string> UnsendRequest(int senderID, int receiverID)
        {
            var friendshipExists = await _context.Friendships.FirstOrDefaultAsync(f =>
            (f.UserID == senderID && f.FriendID == receiverID)
            || f.UserID == receiverID && f.FriendID == senderID);

            if (friendshipExists != null)
            {
                return "friendship already exists, can't unsend request";
            }

            var request = await _context.Requests.FirstOrDefaultAsync(r => r.SenderID == senderID && r.ReceiverID == receiverID);
            if (request == null)
            {
                return "request doesn't exist";
            }
            _context.Remove(request);
            await _context.SaveChangesAsync();

            return "success!";
        }


        public async Task<string> SendRequest(int senderID, int receiverID)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.SenderID == senderID && r.ReceiverID == receiverID);
            if (request != null)
            {
                return "request already sent";
            }

            var friendshipExists = await _context.Friendships.FirstOrDefaultAsync(f =>
                (f.UserID == senderID && f.FriendID == receiverID)
                || f.UserID == receiverID && f.FriendID == senderID);

            if (friendshipExists != null)
            {
                return "friendship already exists, can't send request";
            }

            RequestedFriendship model = new RequestedFriendship
            {
                SenderID = senderID,
                ReceiverID = receiverID,
                RequestedOn = DateTime.UtcNow
            };

            _context.Requests.Add(model);
            await _context.SaveChangesAsync();

            //if there is a request already by the other user
            var exists = await _context.Requests.FirstOrDefaultAsync(r => r.ReceiverID == senderID && r.SenderID == receiverID);

            if (exists != null)
            {

                _context.Remove(model);
                _context.Remove(exists);

                var friendship = new Friendship()
                {
                    UserID = receiverID,
                    FriendID = senderID,
                    Since = DateTime.UtcNow
                };

                _context.Friendships.Add(friendship);
                await _context.SaveChangesAsync();

                return "friendship created!";
            }

            return "success!";
        }

    }
}
