using API.Data.Services.MessageService;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMessageService messageService, IMapper mapper)
        {
            _context = context;
            _messageService = messageService;
            _mapper = mapper;
        }

        public async Task<User> GetUser(int userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("user not found");
            }

            return user;
        }
    }
}
