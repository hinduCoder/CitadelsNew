using Citadels.Client.Telegram.CommandHandlers;
using Citadels.Client.Telegram.Templates.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Citadels.Client.Telegram.Commands.Handlers;

public class CustomInGameChatHandler : ICommandHandler
{
    private readonly ILogger _logger;
    private readonly TelegramClientDbContext _dbContext;
    private readonly ITelegramBotClient _botClient;

    public CustomInGameChatHandler(ILogger logger,
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
            var messageText = Templates.Templates.InGameChatMessageTemplate(new ChatMessage(fromUserModel, message.Text), targetUser.LanguageCode);
            var chatId = targetUser.PrivateChatId;
            await _botClient.SendTextMessageAsync(
                chatId,
                messageText,
                ParseMode.Html, cancellationToken: cancellationToken);
            if (message.Animation is Animation animation)
            {
                await _botClient.SendAnimationAsync(chatId, animation.FileId, caption: message.Caption);
            }
            else if (message.Sticker is Sticker sticker)
            {
                await _botClient.SendStickerAsync(chatId, sticker.FileId);
            }
            else if (message.Photo is [var photo, ..])
            {
                await _botClient.SendPhotoAsync(chatId, photo.FileId, message.Caption);
            }
        }));
    }
}
