using API.Data.Services.MessageService;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Services.HubService
{
    public class HubService : IHubService
    {
        private readonly AppDbContext _context;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public HubService(IMapper mapper, IMessageService messageService, AppDbContext context)
        {
            _mapper = mapper;
            _messageService = messageService;
            _context = context;
        }

        public async Task CreateConnection(Connection model)
        {
            _context.Connections.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Connection> GetConnection(string signalId)
        {
            var connection = await _context.Connections.Where(c => c.SignalId == signalId).SingleOrDefaultAsync();
            if (connection == null)
            {
                throw new Exception("connection not found");
            }
            return connection;
        }

        public async Task<IEnumerable<Connection>> GetConnections(string signalId, int userId, bool withCurrentSignal)
        {
            if (withCurrentSignal)
            {
                var connections = await _context.Connections.Where(c => c.SignalId == signalId || c.UserId == userId).ToListAsync();
                return connections;
            }
            else
            {
                var connections = await _context.Connections.Where(c => c.UserId == userId && c.SignalId != signalId).ToListAsync();
                return connections;
            }


        }

        public async Task<IEnumerable<Connection>> GetUserConnections(int userId)
        {
            var connections = await _context.Connections
                .Where(c => c.UserId == userId).ToListAsync();

            return connections;
        }

        public async Task RemoveConnections(IEnumerable<Connection> connections)
        {
            _context.RemoveRange(connections);
            await _context.SaveChangesAsync();

        }
    }
}
