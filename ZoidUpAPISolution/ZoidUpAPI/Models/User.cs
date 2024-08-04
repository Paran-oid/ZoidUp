using System.ComponentModel.DataAnnotations.Schema;

namespace ZoidUpAPI.Models
{
    [Table(name: "Users", Schema = "ath")]
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
