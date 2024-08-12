namespace API.Models
{
    public class Message
    {
        public int ID { get; set; }
        public string Body { get; set; } = string.Empty;
        public int SenderID { get; set; }
        public required User Sender { get; set; }
        public int ReceiverID { get; set; }
        public required User Receiver { get; set; }

    }
}
