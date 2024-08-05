using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ZoidUpAPI.Hubs
{
    public sealed class ChatHub : Hub<IChatClient>
    {

        public override async Task OnConnectedAsync()
        {
            await Clients.All.HasJoined($"{this.Context.ConnectionId} has joined");
        }
        public async Task SendMessageToAll(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }

        //implement send message privately to a user
    }
}
