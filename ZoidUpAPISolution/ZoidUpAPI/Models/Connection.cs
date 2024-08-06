using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZoidUpAPI.Models
{
    //We will use this connection class to store user connections
    [Table(name: "Connections", Schema = "ath")]
    public class Connection
    {
        [Key]
        public string ID { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public bool Connected { get; set; }
    }
}
