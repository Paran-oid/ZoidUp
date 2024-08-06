using System.ComponentModel.DataAnnotations.Schema;

namespace ZoidUpAPI.Models
{
    [Table(name: "Users", Schema = "ath")]
    public class User
    {
        public int ID { get; set; }
        public string ChatID { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        //users can have connections
        public ICollection<Connection>? Connections { get; set; }
    }
}
