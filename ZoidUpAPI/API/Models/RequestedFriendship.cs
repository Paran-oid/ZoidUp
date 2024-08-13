using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    [Table("RequestedFriendships", Schema = "com")]
    public class RequestedFriendship
    {
        public int SenderID { get; set; }
        [JsonIgnore]
        public User Sender { get; set; }
        public int ReceiverID { get; set; }
        [JsonIgnore]
        public User Receiver { get; set; }
        public DateTime RequestedOn { get; set; } = DateTime.Now;
    }
}
