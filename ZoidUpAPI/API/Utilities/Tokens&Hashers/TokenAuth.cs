using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;

namespace API.Utilities.Tokens_Hashers

{
    public class TokenAuth
    {
        private readonly IConfiguration _configuration;
        public TokenAuth(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Claim> ReadAuthToken(string token)
        {
            //we create a handler
            //we get the jwt using the handler
            //we turn the claims into a list and return them
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var claims = jwt.Claims.ToList();

            return claims;
        }
        public string GenerateAuthToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.ID.ToString()),
                new Claim("username", user.Username),
                new Claim("date", DateTime.Now.ToString()),
                new Claim("profilePicturePath", user.ProfilePicturePath)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:SecretKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(3),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }


    }
}
