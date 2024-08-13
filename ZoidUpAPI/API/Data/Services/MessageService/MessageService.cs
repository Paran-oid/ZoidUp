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
        public async Task<string>? Delete(int messageID)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.ID == messageID);
            if (message == null)
            {
                return null;
            }
            _context.Remove(message);
            await _context.SaveChangesAsync();

            return "success!";
        }

        public async Task<Message> Get(int messageID)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.ID == messageID);

            if (message == null)
            {
                return null;
            }

            return message;
        }

        public async Task<IEnumerable<Message>>? GetAll(int userID)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ID == userID);
            if (user == null)
            {
                return null;
            }

            var messages = _context.Messages.Where(m => m.SenderID == userID).ToList();

            return messages;
        }
        public async Task<Message>? Post(CreateMessageDTO model)
        {
            var sender = await _context.Users.FirstOrDefaultAsync(u => u.ID == model.senderID);
            var receiver = await _context.Users.FirstOrDefaultAsync(u => u.ID == model.receiverID);
            if (sender == null || receiver == null)
            {
                return null;
            }
            var message = _mapper.Map<Message>(model);

            _context.Add(message);
            await _context.SaveChangesAsync();


            return message;
        }
        public async Task<Message>? Put(EditMessageDTO model)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.ID == model.messageID);
            if (message == null)
            {
                return null;
            }

            message.Body = model.body;
            await _context.SaveChangesAsync();

            return message;
        }
    }
}
