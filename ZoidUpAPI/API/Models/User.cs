using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    [Table(name: "Users", Schema = "ath")]
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;


        //friends section
        [JsonIgnore]
        public ICollection<RequestedFriendship>? ReceivedFriendship { get; set; }
        [JsonIgnore]
        public ICollection<RequestedFriendship>? SentFriendship { get; set; }



        //messages section
        [JsonIgnore]
        public ICollection<Message>? SentMessages { get; set; }
        [JsonIgnore]
        public ICollection<Message>? ReceivedMessages { get; set; }


    }
}
