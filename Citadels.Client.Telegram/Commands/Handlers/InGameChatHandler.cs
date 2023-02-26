using Citadels.Client.Telegram.CommandHandlers;
using Citadels.Client.Telegram.Entities;
using Citadels.Client.Telegram.Resources;
using Citadels.Client.Telegram.Templates.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Citadels.Client.Telegram.Commands.Handlers;

public class InGameChatHandler : ICommandHandler
{
    private readonly ILogger _logger;
    private readonly IDbContextFactory<TelegramClientDbContext> _dbContextFactory;
    private readonly ITelegramBotClient _botClient;

    public InGameChatHandler(ILogger logger,
        IDbContextFactory<TelegramClientDbContext> dbContextFactory,
        ITelegramBotClient telegramBotClient)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
        _botClient = telegramBotClient;
    }

    public int Order => 0;
    public bool CanHandle(Update update) => update.Message is not null;
    public async Task Handle(Update update, CancellationToken cancellationToken)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var message = update.Message!;
        var sender = message.From!;

        var user = await dbContext.User.FindAsync(sender.Id);
        if (user?.CurrentGame is null)
        {
            return;
        }

        var fromUserModel = new UserModel(user.TelegramUserId, user.Name!, false);
        var targetUsers = await dbContext.User
            .Where(x => x.CurrentGameId == user.CurrentGameId && x.TelegramUserId != user.TelegramUserId)
            .ToListAsync(cancellationToken: cancellationToken);
        foreach (var targetUser in targetUsers)
        {
            await _botClient.SendTextMessageAsync(
                targetUser.PrivateChatId, 
                Templates.Templates.InGameChatMessageTemplate(new ChatMessage(fromUserModel, message.Text), targetUser.LanguageCode), 
                ParseMode.Html, cancellationToken: cancellationToken);
        }

        return;
    }
}
