namespace ZoidUpAPI.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
        Task HasJoined(string message);
    }
}
