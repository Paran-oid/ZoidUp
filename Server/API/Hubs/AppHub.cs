using Microsoft.AspNetCore.SignalR;
using API.Hubs;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Models;
using API.Data.Services.MessageService;
using AutoMapper;
using API.Data.Services.UserService;
using API.Data.Services.HubService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Hubs
{
    public partial class AppHub : Hub<IChatClient>
    {
        private readonly IUserService _userService;
        private readonly IHubService _hubService;


        public AppHub(AppDbContext context, IMessageService messageService, IMapper mapper, IHubService hubService, IUserService userService)
        {
            _hubService = hubService;
            _userService = userService;
        }

        public override async Task OnConnectedAsync()
        {
            //verify if user exists on the db, if not there is a problem
        }

        public async Task Authenticate(int userId)
        {
            var user = await _userService.GetUser(userId);

            if (user != null)
            {
                user.LoggedOn = DateTime.UtcNow;
                Models.Connection conn = new Models.Connection
                {
                    SignalId = Context.ConnectionId,
                    UserId = userId,
                    Time = DateTime.UtcNow
                };

                await _hubService.CreateConnection(conn);
                await Clients.Caller.AuthSuccess($"you're signed in now as {user.Username}");
            }
            else
            {
                await Clients.Caller.AuthFailed("this user doesn't exist");
            }
        }

        public async Task Reauthenticate(int userId)
        {
            var user = await _userService.GetUser(userId);
            if (user != null)
            {
                await RemoveConnections();
                user.LoggedOn = DateTime.UtcNow;
                Models.Connection conn = new Models.Connection
                {
                    UserId = userId,
                    SignalId = Context.ConnectionId,
                    Time = DateTime.UtcNow
                };

                await _hubService.CreateConnection(conn);
                await Clients.Caller.ReauthSuccess(conn);
            }
            else
            {
                await Clients.Caller.ReauthFailed("there was an error with reauthenticating");
            }
        }
        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            await RemoveConnections(allConnections: true);

        }

        public async Task RemoveConnections(bool allConnections = false)
        {
            var userId = _hubService.GetConnection(Context.ConnectionId).Result.UserId;
            var user = _userService.GetUser(userId);
            if (user == null)
            {
                return;
            }

            if (allConnections)
            {
                var connections = await _hubService.GetConnections(Context.ConnectionId, userId, true);
                await _hubService.RemoveConnections(connections);
            }
            else
            {
                var oldConnections = await _hubService.GetConnections(Context.ConnectionId, userId, false);
                await _hubService.RemoveConnections(oldConnections);
            }
        }

    }
}
