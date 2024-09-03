using API.Models;

namespace API.Hubs
{
    public interface IChatClient
    {
        //Authentication
        public Task AuthSuccess(string text);
        public Task AuthFailed(string text);
        public Task ReauthSuccess(Connection model);
        public Task ReauthFailed(string text);


        //Messages
        public Task SendMessageSuccess(Message message);
        public Task SendMessageFailure(string text);


    }
}
