using Microsoft.AspNetCore.SignalR;
using API.Hubs;

namespace API.Hubs
{
    public class AppHub : Hub<IChatClient>
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
