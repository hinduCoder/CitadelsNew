using Citadels.Client.Telegram.CommandHandlers;
using Citadels.Client.Telegram.Entities;
using Citadels.Client.Telegram.KeyboardTemplates;
using Citadels.Client.Telegram.Resources;
using Citadels.Client.Telegram.TelegramExnteions;
using Citadels.Client.Telegram.Templates.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramTypes = Telegram.Bot.Types;

namespace Citadels.Client.Telegram.Commands.Handlers;

public class RegistrationHandler : ICommandHandler
{
    private readonly IStringsProvider _stringsProvider;
    private readonly ILogger _logger;
    private readonly TelegramClientDbContext _dbContext;
    private readonly ITelegramBotClient _botClient;
    private readonly IKeyboardLocalizator _keyboardLocalizator;

    public RegistrationHandler(IStringsProvider stringsProvider,
        ILogger logger,
        TelegramClientDbContext dbContext,
        ITelegramBotClient telegramBotClient,
        IKeyboardLocalizator keyboardLocalizator)
    {
        _stringsProvider = stringsProvider;
        _logger = logger;
        _dbContext = dbContext;
        _botClient = telegramBotClient;
        _keyboardLocalizator = keyboardLocalizator;
    }

    public async Task Handle(TelegramTypes.Update update, CancellationToken cancellationToken)
    {
        if (update.Message is TelegramTypes.Message message)
        {
            var command = message.ParseCommand();
            if (command is not { Name: "start", Parameter: var param })
            {
                return;
            }
            if (string.IsNullOrEmpty(param))
            {
                await HandleGameCreation(message, cancellationToken);
            } 
            else
            {
                if (!Guid.TryParse(param, out var gameId))
                {
                    return;
                }
                await HandlerGameJoin(message, gameId, cancellationToken);
            }
        } else
        {
            await HandleRegistrationCancel(update.CallbackQuery!, cancellationToken);
        }
    }

    private async Task HandleGameCreation(TelegramTypes.Message message, CancellationToken cancellationToken)
    {
        var user = await GetUpdatedUser(_dbContext, message.From!, message.Chat.Id);
        if (await CheckUserInGame(user, cancellationToken))
        {
            return;
        }
        var game = new Game(user);
        user.CurrentGame = game;
        _dbContext.Add(game);

        var myUsername = (await _botClient.GetMeAsync(cancellationToken: cancellationToken)).Username;
        var link = $"https://t.me/{myUsername}?start={game.Id}";

        await _botClient.SendTextMessageAsync(message.Chat.Id,
            Templates.Templates.GameInvitation(new(link), user.LanguageCode),
            ParseMode.Html, cancellationToken: cancellationToken);

        await Print(new[] { user }, user.TelegramUserId, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task HandlerGameJoin(TelegramTypes.Message message, Guid gameId, CancellationToken cancellationToken)
    {
        var user = await GetUpdatedUser(_dbContext, message.From!, message.Chat.Id);
        if (await CheckUserInGame(user, cancellationToken))
        {
            return;
        }
        var game = await _dbContext.Game.FindAsync(gameId);
        if (game is null)
        {
            await _botClient.SendTextMessageAsync(user.TelegramUserId, _stringsProvider.Get("GameDoesNotExist", user.LanguageCode)!, cancellationToken: cancellationToken);
            return;
        }

        await _dbContext.Entry(game).Collection(x => x.Users).LoadAsync(cancellationToken);
        await Print(game.Users.Concat(new[] { user }), game.HostUserId, cancellationToken);
        user.CurrentGameId = gameId;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task<User> GetUpdatedUser(TelegramClientDbContext dbContext, TelegramTypes.User sender, long chatId)
    {
        var user = await dbContext.User.FindAsync(sender.Id);
        if (user is null)
        {
            user = new User(sender.Id, chatId);
            dbContext.Add(user);
        }

        user.Name = $"{sender.FirstName} {sender.LastName}".Trim();
        user.LanguageCode = sender.LanguageCode;
        
        return user;
    }

    private async Task<bool> CheckUserInGame(User user, CancellationToken cancellationToken)
    {
        if (user.CurrentGameId.HasValue)
        {
            await _botClient.SendTextMessageAsync(user.PrivateChatId, _stringsProvider.Get("GameHasAlreadyStarted", user.LanguageCode)!, replyToMessageId: user.UpdatingTelegramMessageId!.Value, cancellationToken: cancellationToken);
            return true;
        }
        return false;
    }

    private async Task HandleRegistrationCancel(TelegramTypes.CallbackQuery callback, CancellationToken cancellationToken)
    {
        await _botClient.AnswerCallbackQueryAsync(callback.Id);

        var callingUser = await _dbContext.User.FindAsync(new object?[] { callback.From.Id }, cancellationToken: cancellationToken);
        var game = callingUser!.CurrentGame;

        if (game is null)
        {
            return;
        }

        if (game.HostUserId == callingUser.TelegramUserId)
        {
            await _dbContext.Entry(game).Collection(x => x.Users).LoadAsync(cancellationToken);
            await Task.WhenAll(game.Users.Select(async user =>
            {
                await _botClient.DeleteMessageAsync(user.PrivateChatId, user.UpdatingTelegramMessageId!.Value, cancellationToken);
                await _botClient.SendTextMessageAsync(user.PrivateChatId, _stringsProvider.Get("GameCancelled", user.LanguageCode)!, cancellationToken: cancellationToken);
                user.UpdatingTelegramMessageId = null;
            }));

            _dbContext.Remove(game);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return;
        }

        var userGameId = callingUser!.CurrentGameId;
        callingUser.CurrentGameId = null;

        await _botClient.DeleteMessageAsync(callingUser.PrivateChatId,
            callingUser.UpdatingTelegramMessageId!.Value,
            cancellationToken: cancellationToken);
        callingUser.UpdatingTelegramMessageId = null;
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _dbContext.Entry(game).Collection(x => x.Users).LoadAsync(cancellationToken);
        await Print(game.Users, game.HostUserId, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

    }

    private async Task Print(IEnumerable<User> users, long hostId, CancellationToken cancellationToken)
    {
        
        var userModels = users.Select(x => new UserModel(x.TelegramUserId, x.Name ?? x.TelegramUserId.ToString(), x.TelegramUserId == hostId)).ToList();
        await Task.WhenAll(users.Select(async user =>
        {
            var languageCode = user.LanguageCode;
            var messageText = Templates.Templates.RegistrationTemplate(new(userModels), languageCode);

            var keyboard = InlineKeyboardTemplates.RegistrationKeyboard.GetKeyboard(
                new RegistrationKeyboardState(user.TelegramUserId == hostId), user.LanguageCode, _keyboardLocalizator);
            var message = await _botClient.SendOrEditMessageAsync(user.PrivateChatId, user.UpdatingTelegramMessageId,
                    messageText, ParseMode.Html, keyboard, cancellationToken);
            user.UpdatingTelegramMessageId = message.MessageId;
        }));
    }
}
