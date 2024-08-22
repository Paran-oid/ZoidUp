using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Services.FriendshipService
{
    public class FriendshipService : IFriendshipService
    {
        private readonly AppDbContext _context;

        public FriendshipService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllFriends(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("user doesn't exist");
            }
            var friendsIds1 = await _context.Friendships
                .Where(f => f.UserId == userId)
                .Select(f => f.FriendId)
                .ToListAsync();

            var friendsIds2 = await _context.Friendships
                .Where(f => f.FriendId == userId)
                .Select(f => f.UserId)
                .ToListAsync();

            var friendsIds = friendsIds1.Concat(friendsIds2).ToList();

            var friends = await _context.Users
                .Where(u => friendsIds.Contains(u.Id))
                .ToListAsync();

            return friends;
        }
        public async Task<string> RemoveFriendship(int userId, int friendId)
        {
            var friendship = await _context.Friendships.FirstOrDefaultAsync(f => (f.UserId == userId && f.FriendId == friendId) ||
                (f.UserId == friendId && f.FriendId == userId));
            if (friendship == null)
            {
                throw new Exception("request doesn't exist");
            }
            _context.Remove(friendship);
            await _context.SaveChangesAsync();

            return "success!";
        }
    }
}
