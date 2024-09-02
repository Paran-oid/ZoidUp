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
            var userConn = await _context.Connections
                .Where(c => c.UserId == message.SenderId)
                .SingleOrDefaultAsync();

            var friendConn = await _context.Connections
                .Where(c => c.UserId == message.ReceiverId)
            .SingleOrDefaultAsync();


            if(friendConn != null || userConn != null)
            {

                await Clients.Client(userConn!.SignalId).SendMessageSuccess(message);
                await Clients.Client(friendConn!.SignalId).SendMessageSuccess(message);
            }
        }
    }
}
