﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    [Table("Messages", Schema = "com")]
    public class Message
    {
        public int ID { get; set; }
        public string Body { get; set; } = string.Empty;
        public int SenderID { get; set; }
        [JsonIgnore]
        public User Sender { get; set; }
        public int ReceiverID { get; set; }
        [JsonIgnore]
        public User Receiver { get; set; }

    }
}
