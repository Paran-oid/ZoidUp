using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    [Table(name: "Users", Schema = "ath")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(20)]
        [MinLength(3)]
        public string Username { get; set; } = string.Empty;
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string ProfilePicturePath { get; set; } = "default";
        public DateTime LoggedOn { get; set; }


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
