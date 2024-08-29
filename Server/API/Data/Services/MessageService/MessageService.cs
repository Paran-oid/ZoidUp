using API.Models;
using API.Models.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public MessageService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Message> Get(int messageId)
        {

            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);

            if (message == null)
            {
                throw new Exception("message wasn't found");
            }

            return message;

        }

        public async Task<IEnumerable<Message>> GetAllMessagesBetweenUsers(int userId, int FriendId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("user not found");
            }

            var friend = await _context.Users.FirstOrDefaultAsync(u => u.Id == FriendId);
            if (friend == null)
            {
                throw new Exception("user not found");
            }

            var messages = await _context.Messages
                .Where(
                m => (m.SenderId == userId && m.ReceiverId == FriendId) ||
                (m.SenderId == FriendId && m.ReceiverId == userId))
                .OrderBy(m => m.Time)
                .ToListAsync();

            return messages;
        }
        public async Task<Message> Post(CreateMessageDTO model)
        {
            var sender = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.senderId);
            var receiver = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.receiverId);
            if (sender == null || receiver == null)
            {
                throw new Exception("sender or received not found");
            }
            var message = _mapper.Map<Message>(model);

            //check if users are friends or not here

            _context.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }
        public async Task<Message> Put(EditMessageDTO model)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == model.messageId);
            if (message == null)
            {
                throw new Exception("message not found");
            }

            message.Body = model.body;
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<string> Delete(int messageId)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
            if (message == null)
            {
                throw new Exception("message not found");
            }
            _context.Remove(message);
            await _context.SaveChangesAsync();

            return "success!";
        }
    }
}
