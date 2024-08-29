using API.Models;
using API.Models.DTOs;

namespace API.Data.Services.MessageService
{
    public interface IMessageService
    {
        public Task<Message> Get(int messageId);
        public Task<IEnumerable<Message>> GetAllMessagesBetweenUsers(int userId, int FriendId);
        public Task<Message> Post(CreateMessageDTO model);
        public Task<Message> Put(EditMessageDTO model);
        public Task<string> Delete(int messageId);
    }
}
