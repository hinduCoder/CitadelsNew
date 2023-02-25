using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Citadels.Client.Telegram;

public class TelegramClientDbContextFactory : IDesignTimeDbContextFactory<TelegramClientDbContext>
{
    public TelegramClientDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TelegramClientDbContext>();
        optionsBuilder.UseNpgsql();

        return new TelegramClientDbContext(optionsBuilder.Options);
    }
}
