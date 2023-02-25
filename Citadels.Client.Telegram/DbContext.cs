using Citadels.Client.Telegram.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Citadels.Client.Telegram;

public class TelegramClientDbContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Game> Game { get; set; }
    public TelegramClientDbContext()
    {
    }
    public TelegramClientDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var userEntity = modelBuilder.Entity<User>();
        userEntity.HasKey(x => x.TelegramUserId);
        userEntity.Property(x => x.TelegramUserId).ValueGeneratedNever();
        userEntity.Property(x => x.LanguageCode).HasMaxLength(10);
        userEntity.Property(x => x.Name).HasMaxLength(100);
        userEntity
            .HasOne(x => x.CurrentGame)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.CurrentGameId)
            .OnDelete(DeleteBehavior.SetNull);
        userEntity.Navigation(x => x.CurrentGame).AutoInclude();

        var gameEntity = modelBuilder.Entity<Game>();
        gameEntity.HasKey(x => x.Id);
        gameEntity.Property(x => x.Id).ValueGeneratedOnAdd();
        gameEntity.HasOne(x => x.Host).WithOne().HasForeignKey<Game>(x => x.HostUserId);
        gameEntity.Navigation(x => x.Host).IsRequired();
    }
}
