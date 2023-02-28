using Citadels.Client.Telegram.CommandHandlers;
using Telegram.Bot.Types;

namespace Citadels.Client.Telegram.Commands;
public class Router
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<(Func<Update, bool> Condition, Type HandlerType)> _handlers;

    internal Router(IEnumerable<(Func<Update, bool> Condition, Type HandlerType)> handlers, IServiceProvider serviceProvider)
    {
        _handlers = new(handlers);
        _handlers.Add((_ => true, null)!);
        _serviceProvider = serviceProvider;
    }

    public ICommandHandler? GetHandler(Update update, IServiceProvider serviceProvider)
    {
        var handlerType = _handlers.FirstOrDefault(x => x.Condition(update)).HandlerType;
        if (handlerType is null)
        {
            return null;
        }
        return serviceProvider.GetService(handlerType) as ICommandHandler;
    }
}
