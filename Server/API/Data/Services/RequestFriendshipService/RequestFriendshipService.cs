using API.Data.Services.FriendshipService;
using API.Models;
using API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Data.Services.RequestFriendshipService
{
    public class RequestFriendshipService : IRequestFriendshipService
    {
        private readonly AppDbContext _context;
        private readonly IFriendshipService _friendshipService;
        public RequestFriendshipService(AppDbContext context, IFriendshipService friendshipService)
        {
            _context = context;
            _friendshipService = friendshipService;
        }
        public async Task<bool> HasRequests(int userId)
        {
            IEnumerable<RequestUserDTO> requestsEnum = await this.GetAllReceivedRequests(userId);
            int size = requestsEnum
                .ToList()
                .Count;
            return size > 0;
        }
        public async Task<IEnumerable<RequestUserDTO>> GetAllReceivedRequests(int receiverId)
        {
            var temp = await _context.Users.FirstOrDefaultAsync(u => u.Id == receiverId);
            if (temp == null)
            {
                return null;
            }
            // Retrieve all friendship requests received by a specific user
            var requests = _context.Requests
                // Filter requests by the receiver's Id
                .Where(u => u.ReceiverId == receiverId)
                // Select only the SenderId and RequestedOn date for each request
                .Select(u => new { u.SenderId, u.RequestedOn })
                // Execute the query and convert the results to an array
                .ToArray();

            var senders = requests.Select(u => u.SenderId).ToArray();

            var users = _context.Users
                    .Where(u => senders.Contains(u.Id))
                    .Select(u => new { u.Id, u.Username });

            var result = from request in requests
                         join user in users on request.SenderId equals user.Id
                         select new RequestUserDTO
                         {
                             Id = user.Id,
                             username = user.Username,
                             time = request.RequestedOn
                         };

            return result;
        }

        public async Task<IEnumerable<User>> GetAllRecommended(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }
            var friendsEnum = await this._friendshipService.GetAllFriends(userId);
            var sentRequestsEnum = await this.GetAllSentRequests(userId);

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

        public async Task<IEnumerable<RequestUserDTO>> GetAllSentRequests(int senderId)
        {
            var temp = await _context.Users.FirstOrDefaultAsync(u => u.Id == senderId);
            if (temp == null)
            {
                return null;
            }

            //get all requests
            var requests = _context.Requests
                .Where(u => u.SenderId == senderId)
                .Select(u => new { u.ReceiverId, u.RequestedOn })
                .ToArray();

            //select all receivers
            var receivers = requests.Select(u => u.ReceiverId);

            //select all users with receiverId
            var users = _context.Users
                .Where(u => receivers.Contains(u.Id))
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.ProfilePicturePath
                });

            //now we join and return request user dto
            var result = from request in requests
                         join user in users on request.ReceiverId equals user.Id
                         select new RequestUserDTO
                         {
                             Id = user.Id,
                             username = user.Username,
                             time = request.RequestedOn,
                             profilePicturePath = user.ProfilePicturePath
                         };


            return result;
        }

        public async Task<string> UnsendRequest(int senderId, int receiverId)
        {
            var friendshipExists = await _context.Friendships.FirstOrDefaultAsync(f =>
            (f.UserId == senderId && f.FriendId == receiverId)
            || f.UserId == receiverId && f.FriendId == senderId);

            if (friendshipExists != null)
            {
                throw new Exception("friendship already exists, can't unsend request");
            }

            var request = await _context.Requests.FirstOrDefaultAsync(r => r.SenderId == senderId && r.ReceiverId == receiverId);
            if (request == null)
            {
                throw new Exception("request doesn't exist");
            }
            _context.Remove(request);
            await _context.SaveChangesAsync();

            return "success!";
        }


        public async Task<string> SendRequest(int senderId, int receiverId)
        {
            var friendshipExists = await _context.Friendships.FirstOrDefaultAsync(f =>
            (f.UserId == senderId && f.FriendId == receiverId)
            || f.UserId == receiverId && f.FriendId == senderId);

            if (friendshipExists != null)
            {
                throw new Exception("friendship already exists, can't send request");
            }

            var request = await _context.Requests.FirstOrDefaultAsync(r => r.SenderId == senderId && r.ReceiverId == receiverId);
            if (request != null)
            {
                throw new Exception("request already sent");
            }

            RequestedFriendship model = new RequestedFriendship
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                RequestedOn = DateTime.UtcNow
            };

            _context.Requests.Add(model);
            await _context.SaveChangesAsync();

            //if there is a request already by the other user
            var exists = await _context.Requests.FirstOrDefaultAsync(r => r.ReceiverId == senderId && r.SenderId == receiverId);

            if (exists != null)
            {

                _context.Remove(model);
                _context.Remove(exists);
                await _context.SaveChangesAsync();


                var friendship = new Friendship()
                {
                    UserId = receiverId,
                    FriendId = senderId,
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
