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
    var startGameEvent = new StartGame(Enumerable.Range(1, 4).Select(i => i.ToString()));
    yield return startGameEvent;
    yield return new DiscardDraftFirstCards();
    foreach (var @char in startGameEvent.RandomizedCharacters.Skip(3).Take(4))
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