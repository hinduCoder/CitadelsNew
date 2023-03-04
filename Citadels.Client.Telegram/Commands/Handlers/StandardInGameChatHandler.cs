using Citadels.Client.Telegram.CommandHandlers;
using Citadels.Client.Telegram.Templates.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Citadels.Client.Telegram.Commands.Handlers;

public class StandardInGameChatHandler : ICommandHandler
{
    private readonly ILogger _logger;
    private readonly TelegramClientDbContext _dbContext;
    private readonly ITelegramBotClient _botClient;

    public StandardInGameChatHandler(ILogger logger,
        TelegramClientDbContext dbContext,
        ITelegramBotClient telegramBotClient)
    {
        _logger = logger;
        _dbContext = dbContext;
        _botClient = telegramBotClient;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken)
    {
        var message = update.Message!;
        var sender = message.From!;

        var user = await _dbContext.User.FindAsync(sender.Id);
        if (user?.CurrentGame is null)
        {
            return;
        }

        var fromUserModel = new UserModel(user.TelegramUserId, user.Name!, false);
        var targetUsers = await _dbContext.User
            .Where(x => x.CurrentGameId == user.CurrentGameId && x.TelegramUserId != user.TelegramUserId)
            .ToListAsync(cancellationToken: cancellationToken);
        await Task.WhenAll(targetUsers.Select(async targetUser =>
        {
            await _botClient.ForwardMessageAsync(targetUser.PrivateChatId, message.Chat.Id, message.MessageId);
        }));
    }
}
