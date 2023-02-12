using Citadels.Core;
using Citadels.Core.Events;
using System.Diagnostics;

var eventHander = new GameEventHandler();
var events = GetEvents().ToList();

var game = new Game();
ExecuteEvents(game, events);

var game2 = new Game();
ExecuteEvents(game2, events);

Debugger.Break();

static IEnumerable<IGameEvent> GetEvents()
{
    var playersCount = 7;
    var startGameEvent = new StartGame(Enumerable.Range(1, playersCount).Select(i => i.ToString()));
    yield return startGameEvent;
    yield return new DiscardDraftFirstCards();
    foreach (var @char in startGameEvent.RandomizedCharacters.Skip(Math.Max(7 - playersCount, 1)).Take(playersCount))
    {
        yield return new ChooseCharacter(@char.Rank);
    }
}

void ExecuteEvents(Game game, IEnumerable<IGameEvent> events)
{
    foreach (var e in events)
    {
        var result = eventHander!.HandleEvent(game, e);
        if (!result)
        {
            throw new InvalidOperationException();
        }
    }
}