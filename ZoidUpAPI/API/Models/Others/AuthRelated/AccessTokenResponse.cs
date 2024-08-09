namespace API.Models.Others.AuthRelated
{
    public class AccessTokenResponse
    {
        public AccessTokenResponse(string token, int expiresIn)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }
        public string Token { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
