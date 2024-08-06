using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using ZoidUpAPI.Data;

namespace ZoidUpAPI.Hubs
{
    public sealed class ChatHub : Hub<IChatClient>
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public ChatHub(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public override Task OnConnectedAsync()
        {

            Clients.All.HasJoined($"{this.Context.ConnectionId} has joined");
            //we will get the currently logged in user name, and add that connection id to him
            var name = this.Context.User.Identity.Name;

            var userID = Guid.Parse(Context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _context.Users.
                Include(u => u.Connections).
                FirstOrDefault(u => u.ChatID == name);
            if (user == null)
            {
                //assign the chatid property of his entity to that of conncetionID
            }
            else
            {
                user.Connections.Add(new Models.Connection
                {
                    ID = this.Context.ConnectionId,
                    UserAgent = this.Context.GetHttpContext().Request.Headers["User-Agent"],
                    Connected = true
                });
                _context.SaveChanges();
            }
            return base.OnConnectedAsync();

        }

        public async Task SendMessageToUser(string userTo, string message)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Default"));

            string name = this.Context.User.Identity.Name;

            //this will try to find the user we want to send info to
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userTo);

            if (user == null)
            {
                await Clients.Caller.ShowErrorMessage("Couldn't find user");
            }
            else
            {
                _context
                    .Entry(user)
                    .Collection(u => u.Connections)
                    .Query()
                    .Where(c => c.Connected == true)
                    .Load();

                //we will load users connections to the entity itself from the Database
                //if he has no connections we will send an error else we will send the message to the respective user

                if (user.Connections == null)
                {
                    await Clients.Caller.ShowErrorMessage("The user is no longer connected");
                }
                else
                {
                    foreach (var connection in user.Connections)
                    {
                        await Clients.Client(connection.ID)
                                .ReceiveMessage(name, message);
                    }
                }

            }

        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = _context.Connections.Find(Context.ConnectionId);
            connection.Connected = false;
            _context.SaveChanges();
            return base.OnDisconnectedAsync(exception);
        }

    }
}
