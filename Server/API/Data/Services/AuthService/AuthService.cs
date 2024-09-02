using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using API.Models;
using API.Utilities.Tokens_Hashers;
using API.Models.DTOs.AuthRelated;

namespace API.Data.Services.AuthService
{
    public class AuthService : IAuthService

    {
        private readonly AppDbContext _context;
        private readonly Hash _hasher;
        private readonly TokenAuth _token;

        public AuthService(AppDbContext context, Hash hasher, TokenAuth token)
        {
            _context = context;
            _hasher = hasher;
            _token = token;
        }


        public async Task<object> GetUser(string token)
        {
            token = token.Replace("Bearer", "");
            token = token.Trim();

            var claims = _token.ReadAuthToken(token);

            if (claims.Count == 0)
            {
                throw new Exception("invalid token");
            }


            var result = new
            {
                Id = claims.FirstOrDefault(c => c.Type == "Id")!.Value,
                username = claims.FirstOrDefault(c => c.Type == "username")!.Value,
                date = claims.FirstOrDefault(c => c.Type == "date")!.Value,
                profilePicturePath = claims.FirstOrDefault(c => c.Type == "profile-picture-path")!.Value
            };

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == result.username
            );

            if (user == null)
            {
                throw new Exception("user not found");
            }

            user.LoggedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<AccessTokenResponse> Login(LoginEntry model)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null)
            {
                throw new Exception("user not found");
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

            throw new Exception("not same password");

        }

        public async Task<AccessTokenResponse> Register(RegisterEntry model)
        {
            var temp = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (temp != null)
            {
                throw new Exception("user with username already exists");
            }

            var hashedPassword = _hasher.HashPassword(model.Password);
            var userCreated = new User
            {
                Username = model.Username,
                Password = hashedPassword,
            };

            await _context.Users.AddAsync(userCreated);
            await _context.SaveChangesAsync();


            var token = _token.GenerateAuthToken(user: userCreated);
            userCreated.Token = token;


            var result = new AccessTokenResponse(token,
                (int)(DateTime.UtcNow.AddMinutes(60) - DateTime.UtcNow).TotalSeconds);

            return result;
        }
    }
}
