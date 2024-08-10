using API.Models.DTOs;

namespace API.Data.Services.MessageService
{
    public interface IMessageService
    {
        public Task Get(int MessageID);
        public Task GetAll(int UserID);
        public Task Post(CreateMessageDTO model);
        public Task Put(EditMessageDTO model);
        public Task Delete(int MessageID);
    }
}
