using Microsoft.AspNetCore.SignalR;
using API.Hubs;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Hubs
{
    public partial class AppHub : Hub<IChatClient>
    {
        private readonly AppDbContext _context;

        public AppHub(AppDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.Connected(Context.ConnectionId);
            //verify if user exists on the db, if not there is a problem
        }

        public async Task Authenticate(int userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            if (user != null)
            {
                Connection conn = new Connection
                {
                    SignalId = Context.ConnectionId,
                    UserId = userId,
                    Time = DateTime.UtcNow
                };
                _context.Connections.Add(conn);
                await _context.SaveChangesAsync();

                await Clients.Caller.AuthSuccess($"you're signed in now as {user.Username}");
            }
            else
            {
                await Clients.Caller.AuthFailed("this user doesn't exist");
            }
        }

        public async Task Reauthenticate(int UserId)
        {
            var user = await _context.Users
                .Where(u => u.Id == UserId)
                .SingleOrDefaultAsync();
            if(user != null)
            {
                Connection connection = new Connection
                {
                    UserId = UserId,
                    SignalId = Context.ConnectionId,
                    Time = DateTime.UtcNow
                };

                _context.Connections.Add(connection);
                await _context.SaveChangesAsync();

                await Clients.Caller.ReauthSuccess(connection);
            }
            else
            {
                await Clients.Caller.ReauthFailed("there was an error with reauthenticating");
            }
        }
        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.Disconnected(Context.ConnectionId);
            var connection = await _context.Connections.Where(c => c.SignalId == Context.ConnectionId).ToListAsync();
            _context.Remove(connection);
            await _context.SaveChangesAsync();
        }
    }
}
