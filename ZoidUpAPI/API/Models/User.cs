using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table(name: "Users", Schema = "ath")]
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;


        public ICollection<Message>? SentMessages { get; set; }
        public ICollection<Message>? ReceivedMessages { get; set; }


    }
}
