using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZoidUpAPI.Models;
using ZoidUpAPI.Models.Others.AuthRelated;
using ZoidUpAPI.Utilities.Tokens_Hashers;

namespace ZoidUpAPI.Data.Services.UserService
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

        public async Task<User?> GetUser(string token)
        {
            var claims = _token.ReadAuthToken(token);

            //we get the claim whose type is name
            var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            string name = nameClaim.Value;

            if (name == null)
            {
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == name);

            if (user == null)
            {
                return null;
            }

            return user;
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

            await _context.Users.AddAsync(userCreated);
            await _context.SaveChangesAsync();

            var token = _token.GenerateAuthToken(user: userCreated);

            var result = new AccessTokenResponse(token,
                (int)(DateTime.UtcNow.AddMinutes(60) - DateTime.UtcNow).TotalSeconds);
            return result;


        }
    }
}
