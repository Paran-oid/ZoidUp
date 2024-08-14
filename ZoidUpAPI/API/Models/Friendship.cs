using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    [Table("Friends", Schema = "com")]
    public class Friendship
    {
        public int UserID { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int FriendID { get; set; }
        [JsonIgnore]
        public User Friend { get; set; }
        public DateTime Since { get; set; } = DateTime.UtcNow;
    }
}
