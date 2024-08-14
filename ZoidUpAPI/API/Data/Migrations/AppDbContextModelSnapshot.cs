﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("API.Models.Friendship", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("integer");

                    b.Property<int>("FriendID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Since")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UserID", "FriendID");

                    b.HasIndex("FriendID");

                    b.HasIndex("UserID", "FriendID")
                        .IsUnique();

                    b.ToTable("Friends", "com");
                });

            modelBuilder.Entity("API.Models.Message", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ReceiverID")
                        .HasColumnType("integer");

                    b.Property<int>("SenderID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("ReceiverID");

                    b.HasIndex("SenderID");

                    b.ToTable("Messages", "com");
                });

            modelBuilder.Entity("API.Models.RequestedFriendship", b =>
                {
                    b.Property<int>("SenderID")
                        .HasColumnType("integer");

                    b.Property<int>("ReceiverID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("RequestedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("SenderID", "ReceiverID");

                    b.HasIndex("ReceiverID");

                    b.ToTable("RequestedFriendships", "com");
                });

            modelBuilder.Entity("API.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProfilePicturePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Users", "ath");
                });

            modelBuilder.Entity("API.Models.Friendship", b =>
                {
                    b.HasOne("API.Models.User", "Friend")
                        .WithMany()
                        .HasForeignKey("FriendID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Models.Message", b =>
                {
                    b.HasOne("API.Models.User", "Receiver")
                        .WithMany("ReceivedMessages")
                        .HasForeignKey("ReceiverID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Models.User", "Sender")
                        .WithMany("SentMessages")
                        .HasForeignKey("SenderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("API.Models.RequestedFriendship", b =>
                {
                    b.HasOne("API.Models.User", "Receiver")
                        .WithMany("ReceivedFriendship")
                        .HasForeignKey("ReceiverID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Models.User", "Sender")
                        .WithMany("SentFriendship")
                        .HasForeignKey("SenderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("API.Models.User", b =>
                {
                    b.Navigation("ReceivedFriendship");

                    b.Navigation("ReceivedMessages");

                    b.Navigation("SentFriendship");

                    b.Navigation("SentMessages");
                });
#pragma warning restore 612, 618
        }
    }
}