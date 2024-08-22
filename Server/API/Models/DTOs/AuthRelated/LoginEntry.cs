using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs.AuthRelated
{
    public class LoginEntry
    {
        [MaxLength(20)]
        public string Username { get; set; } = string.Empty;
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}
