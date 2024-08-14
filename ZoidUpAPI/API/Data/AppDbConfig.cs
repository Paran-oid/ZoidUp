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

            //for friendship requests
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

            //for friendships
            builder.Entity<Friendship>()
                .HasKey(f => new { f.UserID, f.FriendID });

            builder.Entity<Friendship>()
                .HasIndex(f => new { f.UserID, f.FriendID })
                .IsUnique();

            builder.Entity<Friendship>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserID);

            builder.Entity<Friendship>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.FriendID);
        }
    }
}
