namespace API.Hubs
{
    public interface IChatClient
    {
        //Authentication
        public Task AuthSuccess(string text);
        public Task AuthFailed(string text);

        //Connection
        public Task Disconnected(string text);
        public Task Connected(string text);

        //Testing
        public Task Test(string text);
    }
}
