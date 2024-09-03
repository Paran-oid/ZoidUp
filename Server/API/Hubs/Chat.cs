using API.Data.Services.MessageService;
using API.Models;
using API.Models.DTOs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Hubs
{
    public partial class AppHub
    {
        public async Task SendMessage(Message message)
        {
            var userConnections = await _hubService.GetUserConnections(message.SenderId);
            var friendConnections = await _hubService.GetUserConnections(message.ReceiverId);

            var userConn = userConnections.FirstOrDefault();
            var friendConn = friendConnections.FirstOrDefault();

            if (friendConn != null || userConn != null)
            {

                await Clients.Client(userConn!.SignalId).SendMessageSuccess(message);
                await Clients.Client(friendConn!.SignalId).SendMessageSuccess(message);
            }
        }
    }
}
