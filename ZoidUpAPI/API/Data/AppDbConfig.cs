using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDbConfig
    {
        public static void Configure(ModelBuilder builder)
        {
            //for messages
            builder.Entity<Message>()
                    .HasOne(m => m.Sender)
                    .WithMany(u => u.SentMessages)
                    .HasForeignKey(m => m.SenderID);

            builder.Entity<Message>()
           .HasOne(m => m.Receiver)
           .WithMany(u => u.ReceivedMessages)
           .HasForeignKey(m => m.ReceiverID);

            //for friendships
            builder.Entity<RequestedFriendship>()
                .HasKey(f => new { f.SenderID, f.ReceiverID });

            builder.Entity<RequestedFriendship>()
               .HasOne(f => f.Sender)
               .WithMany(u => u.SentFriendship)
               .HasForeignKey(f => f.SenderID);

            builder.Entity<RequestedFriendship>()
                .HasOne(f => f.Receiver)
                .WithMany(u => u.ReceivedFriendship)
                .HasForeignKey(f => f.ReceiverID);
        }
    }
}
