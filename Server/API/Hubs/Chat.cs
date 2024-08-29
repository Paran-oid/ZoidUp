using API.Data.Services.MessageService;
using API.Models.DTOs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Hubs
{
    public partial class AppHub
    {
        public async Task SendMessage(CreateMessageDTO model)
        {
            var result = await _messageService.Post(model);
            if(result != null)
            {
                var userConn = await _context.Connections
                    .Where(c => c.UserId == model.senderId)
                    .SingleOrDefaultAsync();

                var friendConn = await _context.Connections
                    .Where(c => c.UserId == model.receiverId)
                .SingleOrDefaultAsync();

                await Clients.Client(userConn.SignalId).SendMessageSuccess(result);
                await Clients.Client(friendConn.SignalId).SendMessageSuccess(result);

            }
            else
            {
                await Clients.Caller.SendMessageFailure("there was an error sending the message");
            }
        }
    }
}
