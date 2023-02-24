﻿// <auto-generated />
using System;
using Citadels.Client.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Citadels.Client.Telegram.Migrations
{
    [DbContext(typeof(TelegramClientDbContext))]
    [Migration("20230222184236_UserChatId")]
    partial class UserChatId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Citadels.Client.Telegram.Entities.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("HostUserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("HostUserId")
                        .IsUnique();

                    b.ToTable("Game");
                });

            modelBuilder.Entity("Citadels.Client.Telegram.Entities.User", b =>
                {
                    b.Property<long>("TelegramUserId")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("CurrentGameId")
                        .HasColumnType("uuid");

                    b.Property<string>("LanguageCode")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<long>("PrivateChatId")
                        .HasColumnType("bigint");

                    b.Property<int?>("UpdatingTelegramMessageId")
                        .HasColumnType("integer");

                    b.HasKey("TelegramUserId");

                    b.HasIndex("CurrentGameId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Citadels.Client.Telegram.Entities.Game", b =>
                {
                    b.HasOne("Citadels.Client.Telegram.Entities.User", "Host")
                        .WithOne()
                        .HasForeignKey("Citadels.Client.Telegram.Entities.Game", "HostUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Host");
                });

            modelBuilder.Entity("Citadels.Client.Telegram.Entities.User", b =>
                {
                    b.HasOne("Citadels.Client.Telegram.Entities.Game", "CurrentGame")
                        .WithMany("Users")
                        .HasForeignKey("CurrentGameId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("CurrentGame");
                });

            modelBuilder.Entity("Citadels.Client.Telegram.Entities.Game", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}