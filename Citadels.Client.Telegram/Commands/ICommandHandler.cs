using Telegram.Bot.Types;

namespace Citadels.Client.Telegram.CommandHandlers;

public interface ICommandHandler
{
    bool CanHandle(Update update);
    int Order { get; } //TODO temporary
    Task Handle(Update update, CancellationToken cancellationToken);
}
