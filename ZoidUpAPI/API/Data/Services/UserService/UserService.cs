using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using API.Models;
using API.Models.Others.AuthRelated;
using API.Utilities.Tokens_Hashers;

namespace API.Data.Services.UserService
{
    public class UserService : IUserService

    {
        private readonly AppDbContext _context;
        private readonly Hash _hasher;
        private readonly TokenAuth _token;

        public UserService(AppDbContext context, Hash hasher, TokenAuth token)
        {
            _context = context;
            _hasher = hasher;
            _token = token;
        }

        public async Task<IEnumerable<User>?> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<object?> GetUser(string token)
        {
            var claims = _token.ReadAuthToken(token);

            if (claims.Count == 0)
            {
                return null;
            }


            var result = new
            {
                username = claims.FirstOrDefault(c => c.Type == "username")!.Value,
                token = claims.FirstOrDefault(c => c.Type == "token")!.Value,
                date = claims.FirstOrDefault(c => c.Type == "date")!.Value,
            };

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == result.username
            );

            if (user == null)
            {
                return null;
            }


            return result;
        }

        public async Task<AccessTokenResponse?> Login(LoginEntry model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null)
            {
                return null;
            }
            bool isSamePassword = _hasher.VerifyPassword(user.Password, model.Password);

            if (isSamePassword)
            {
                var token = _token.GenerateAuthToken(user);

                user.Token = token;
                await _context.SaveChangesAsync();

                var result = new AccessTokenResponse(token, Convert.ToInt32((DateTime.UtcNow.AddMinutes(60) - DateTime.UtcNow).TotalSeconds));

                return result;
            }
            return null;
        }

        public async Task<AccessTokenResponse?> Register(RegisterEntry model)
        {
            var temp = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (temp != null)
            {
                return null;
            }
            //if no user exists, we will create a the user

            var hashedPassword = _hasher.HashPassword(model.Password);
            var userCreated = new User
            {
                Username = model.Username,
                Password = hashedPassword
            };


            var token = _token.GenerateAuthToken(user: userCreated);
            userCreated.Token = token;

            await _context.Users.AddAsync(userCreated);
            await _context.SaveChangesAsync();

            var result = new AccessTokenResponse(token,
                (int)(DateTime.UtcNow.AddMinutes(60) - DateTime.UtcNow).TotalSeconds);

            return result;
        }
    }
}
