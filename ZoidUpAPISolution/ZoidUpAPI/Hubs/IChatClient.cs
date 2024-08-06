namespace ZoidUpAPI.Hubs
{
    public interface IChatClient
    {
        Task HasJoined(string message);
        Task ReceiveMessage(string user, string message);
        Task ShowErrorMessage(string message);
    }
}
