using System.Diagnostics.CodeAnalysis;

namespace Citadels.Client.Telegram.Entities;

public class Game
{
    public Guid Id { get; }
    public long HostUserId { get; private set;  }
    public User Host { get; }
    [AllowNull]
    public ICollection<User> Users { get; private set; }

    public Game(User host)
    {
        Host = host;
    }

    private Game()
    {
        Host = null!;
    }
}