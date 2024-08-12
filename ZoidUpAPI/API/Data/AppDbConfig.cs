using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDbConfig
    {
        public static void Configure(ModelBuilder builder)
        {
            //for messages and users
            builder.Entity<Message>()
                    .HasOne(m => m.Sender)
                    .WithMany(u => u.SentMessages)
                    .HasForeignKey(m => m.SenderID);

            builder.Entity<Message>()
           .HasOne(m => m.Receiver)
           .WithMany(u => u.ReceivedMessages)
           .HasForeignKey(m => m.ReceiverID);
        }
    }
}
