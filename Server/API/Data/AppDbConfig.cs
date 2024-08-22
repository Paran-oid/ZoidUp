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
                    .HasForeignKey(m => m.SenderId);

            builder.Entity<Message>()
           .HasOne(m => m.Receiver)
           .WithMany(u => u.ReceivedMessages)
           .HasForeignKey(m => m.ReceiverId);

            //for friendship requests
            builder.Entity<RequestedFriendship>()
                .HasKey(f => new { f.SenderId, f.ReceiverId });


            builder.Entity<RequestedFriendship>()
               .HasOne(f => f.Sender)
               .WithMany(u => u.SentFriendship)
               .HasForeignKey(f => f.SenderId);

            builder.Entity<RequestedFriendship>()
                .HasOne(f => f.Receiver)
                .WithMany(u => u.ReceivedFriendship)
                .HasForeignKey(f => f.ReceiverId);

            //for friendships
            builder.Entity<Friendship>()
                .HasKey(f => new { f.UserId, f.FriendId });

            builder.Entity<Friendship>()
                .HasIndex(f => new { f.UserId, f.FriendId })
                .IsUnique();

            builder.Entity<Friendship>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId);

            builder.Entity<Friendship>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.FriendId);
        }
    }
}
