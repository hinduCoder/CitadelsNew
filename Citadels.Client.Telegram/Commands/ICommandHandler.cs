using Telegram.Bot.Types;

namespace Citadels.Client.Telegram.CommandHandlers;

public interface ICommandHandler
{
    Task Handle(Update update, CancellationToken cancellationToken);
}
