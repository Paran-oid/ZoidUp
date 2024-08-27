using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Connections", Schema = "ath")]
    public class Connection
    {
        public int Id { get; set; }
        public string SignalId { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime Time { get; set; }
    }
}
