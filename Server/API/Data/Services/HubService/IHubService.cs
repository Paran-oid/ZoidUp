using API.Models;

namespace API.Data.Services.HubService
{
    public interface IHubService
    {
        public Task<Connection> GetConnection(string signalId);
        public Task<IEnumerable<Connection>> GetConnections(string? signalId, int userId, bool withCurrentSignal);
        public Task<IEnumerable<Connection>> GetUserConnections(int userId);
        public Task CreateConnection(Connection model);
        public Task RemoveConnections(IEnumerable<Connection> connections);

    }
}
