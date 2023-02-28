using CitadelsApi = Citadels.Api.Citadels;
using Citadels.Client.Telegram.CommandHandlers;
using Citadels.Client.Telegram.Templates.Models;
using Telegram.Bot;
using TelegramTypes = Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.EntityFrameworkCore;
using Citadels.Client.Telegram.Entities;
using Citadels.Client.Telegram.TelegramExnteions;
using Citadels.Client.Telegram.Resources;

namespace Citadels.Client.Telegram.Commands.Handlers;
public class DraftHandler : ICommandHandler
{
    private readonly ITelegramBotClient _telegram;
    private readonly TelegramClientDbContext _dbContext;
    private readonly IStringsProvider _stringsProvider;
    private readonly IKeyboardLocalizator _keyboardLocalizator;
    private readonly CitadelsApi.CitadelsClient _api;

    public DraftHandler(ITelegramBotClient telegramBotClient,
        TelegramClientDbContext dbContext,
        IKeyboardLocalizator keyboardLocalizator,
        CitadelsApi.CitadelsClient citadelsClient,
        IStringsProvider stringsProvider)
    {
        _telegram = telegramBotClient;
        _dbContext = dbContext;
        _keyboardLocalizator = keyboardLocalizator;
        _api = citadelsClient;
        _stringsProvider = stringsProvider;
    }

    public async Task Handle(TelegramTypes.Update update, CancellationToken cancellationToken)
    {
        var callback = update.CallbackQuery!;
        if (callback.Data is "start")
        {
            var game = await _dbContext.Game
                .Include(x => x.Users)
                .SingleOrDefaultAsync(x => x.HostUserId == callback.From.Id);
            if (game is null)
            {
                await _telegram.AnswerCallbackQueryAsync(callback.Id, _stringsProvider.Get("OnlyHostCanStart", callback.From.LanguageCode), showAlert: true);
                return;
            }
            await _telegram.AnswerCallbackQueryAsync(callback.Id);

            var gameUsers = game.Users.ToList();

            var request = new Api.NewGameRequest { GameId = game.Id.ToString() };
            request.PlayerNames.AddRange(gameUsers.Select(x => x.Name));
            await _api.StartNewGameAsync(request);

            foreach (var user in gameUsers)
            {
                await _telegram.EditMessageReplyMarkupAsync(user.PrivateChatId, user.UpdatingTelegramMessageId!.Value, replyMarkup: null);
                await _telegram.SendTextMessageAsync(user.PrivateChatId, _stringsProvider.Get("DraftStarted", user.LanguageCode)!);
            }

            var state = await _api.StartDraftAsync(new Api.StartDraftRequest { GameId = game.Id.ToString() });
            if (state.InProgress)
            {
                var targetUser = gameUsers.Find(x => x.Name == state.PlayerName)!;
                await SendMessages(targetUser, gameUsers, state.AvailableRanks, state.DiscardedRanks);
            }
        }
        else if (int.TryParse(callback.Data, out var rank))
        {
            await _telegram.EditMessageReplyMarkupAsync(callback.Message!.Chat.Id, callback.Message.MessageId, cancellationToken: cancellationToken);
            await _telegram.AnswerCallbackQueryAsync(callback.Id);

            var callingUser = await _dbContext.User.FindAsync(callback.From.Id);
            var state = await _api.ChooseCharacterAsync(new Api.ChooseCharacterRequest { GameId = callingUser!.CurrentGameId.ToString(), Rank = rank });

            var game = callingUser.CurrentGame!;
            await _dbContext.Entry(game).Collection(x => x.Users).LoadAsync();
            var gameUsers = game.Users.ToList();
            if (state.InProgress)
            {
                var targetUser = await _dbContext.User.Where(x => x.Name == state.PlayerName && x.CurrentGameId == callingUser.CurrentGameId).SingleAsync();
                await SendMessages(targetUser, gameUsers, state.AvailableRanks, state.DiscardedRanks);
            }
            else
            {
                foreach (var user in gameUsers)
                {
                    await _telegram.SendTextMessageAsync(user.PrivateChatId, _stringsProvider.Get("DraftEnded", user.LanguageCode)!);
                }
            }
        }
    }

    private async Task SendMessages(User targetUser, IEnumerable<User> gameUsers,
        IEnumerable<int> availableRanks, IEnumerable<int> discardedRanks)
    {
        var text = Templates.Templates.DraftCharacterCards(new DraftModel(
            availableRanks.Select(i => new CharacterCardModel(i)), discardedRanks.Select(i => new CharacterCardModel(i))), targetUser.LanguageCode);

        var keyboardButtons = 
                 availableRanks
                    .Select(i => InlineKeyboardButton.WithCallbackData($"CharacterName{i}", i.ToString()))
                    .Chunk(3);
        await _telegram.SendTextMessageAsync(targetUser.PrivateChatId, text, ParseMode.Html,
            replyMarkup: new InlineKeyboardMarkup(_keyboardLocalizator.Localize(keyboardButtons, targetUser.LanguageCode)));

        foreach (var user in gameUsers.Where(u => u.TelegramUserId != targetUser.TelegramUserId))
        {
            await _telegram.SendTextMessageAsync(user.PrivateChatId, $"{targetUser.TelegramLinkMarkdown} выбирает героя", ParseMode.MarkdownV2);
        }
    }
}
