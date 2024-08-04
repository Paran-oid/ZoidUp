using Microsoft.AspNetCore.Identity;

namespace ZoidUpAPI.Utilities.Tokens_Hashers
{
    public class Hash
    {
        private readonly PasswordHasher<object> _passwordHasher;
        public Hash()
        {
            _passwordHasher = new PasswordHasher<object>();
        }

        public string HashPassword(string password)
        {
            var hashedPassword = _passwordHasher.HashPassword(null, password);
            return hashedPassword;
        }
        public bool VerifyPassword(string hashedPassword, string password)
        {
            return _passwordHasher.VerifyHashedPassword(null, hashedPassword, password) == PasswordVerificationResult.Success;
        }
    }
}
