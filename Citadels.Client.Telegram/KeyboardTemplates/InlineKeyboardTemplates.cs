using Citadels.Client.Telegram.Commands;

namespace Citadels.Client.Telegram.KeyboardTemplates;

public static class InlineKeyboardTemplates
{
    public static readonly InlineKeyboardTemplate<AssasinKeyboardState> AssasinKeyboard
        = new InlineKeyboardTemplate<AssasinKeyboardState>()
            .AddRow(x => x
                .Button("GatherCoins", "coins", static s => s.GatherActionAvailable)
                .Button("GatherDistrict", "district", static s => s.GatherActionAvailable))
            .AddRow(x => x.Button("Убить кого-то", "kill", static s => s.KillActionAvailable))
            .AddRow(x => x.Button("Закончить ход", "end_turn", static s => !s.GatherActionAvailable));
    public static readonly InlineKeyboardTemplate<RegistrationKeyboardState> RegistrationKeyboard
        = new InlineKeyboardTemplate<RegistrationKeyboardState>()
        .AddRow(x => x
            .Button("Start", "start", static s => s.IsHost)
            .Button("CancelGame", CallbackData.CancelRegistration, static s => s.IsHost)
            .Button("CancelRegistration", CallbackData.CancelRegistration, static s => !s.IsHost))
        .AddRow(x => x.Link("Rules", "RulesLink"));
}

public record struct AssasinKeyboardState(bool GatherActionAvailable, bool KillActionAvailable);
public record struct RegistrationKeyboardState(bool IsHost);
