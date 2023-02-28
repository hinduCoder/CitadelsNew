using Citadels.Client.Telegram.CommandHandlers;
using Telegram.Bot.Types;

namespace Citadels.Client.Telegram.Commands;

public class RouterBuilder
{
    private readonly List<(Func<Update, bool>, Type)> _handlers = new();
    public IReadOnlyList<(Func<Update, bool>, Type)> Handlers => _handlers;
    public RouterBuilder Map<T>(Func<Update, bool> condition) where T : ICommandHandler
    {
        _handlers.Add((condition, typeof(T)));
        return this;
    }
    public Router Build() => new Router(_handlers);
}
