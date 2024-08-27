using Microsoft.AspNetCore.SignalR;
using API.Hubs;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Hubs
{
    public class AppHub : Hub<IChatClient>
    {
        private readonly AppDbContext _context;

        public AppHub(AppDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            await Clients.All.Connected($"new connection : {connectionId}");
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

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = await _context.Connections.Where(c => c.SignalId == Context.ConnectionId).SingleOrDefaultAsync();
            if (connection != null)
            {
                _context.Connections.Remove(connection);
                await _context.SaveChangesAsync();
                await Clients.Caller.Disconnected("You have disconnected from the hub");
            }
        }

        public async Task SendTest()
        {
            await Clients.Caller.Test("it works bro");
        }
    }
}
