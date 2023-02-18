﻿using Citadels.Core.Actions;
using Citadels.Core.Characters;
using Citadels.Core.Districts;

namespace Citadels.Core;

public class Turn
{
    private readonly List<District> _districtsForChoose = new();
    private Game Game { get; }
    private TurnActionPool TurnActionPool { get; }

    public Player Player { get; private set; }
    public bool GatherActionDone { get; private set; }
    public bool GatherActionInProgress { get; private set; }
    public int DistrictBuiltCount { get; private set; }

    public IReadOnlyList<District> DistrictsForChoose => _districtsForChoose;
    public bool GatherActionAvailable => !GatherActionDone && !GatherActionInProgress;
    public bool CanBuild => GatherActionDone && DistrictBuiltCount < Player.CurrentCharacter.DistrictMaxBuildCount;
    public bool CanEnd => GatherActionDone && TurnActionPool.AllObligatoryActionsDone;

    internal Turn(Game game, Player player)
    {
        Game = game;
        Player = player;

        foreach (var action in player.CurrentCharacter.AutomaticActions)
        {
            action.Execute(game);
        }
        TurnActionPool = new TurnActionPool(player.CurrentCharacter.AvailableActions);
    }

    internal void End()
    {
    }

    internal void GatherCoins()
    {
        Player.Coins += 2;
        GatherActionDone = true;
    }

    internal void GatherDistrict()
    {
        _districtsForChoose.AddRange(Game.DistrictDeck.Take(2));
        GatherActionInProgress = true;
    }

    internal void ChooseDistrict(District district)
    {
        if (!DistrictsForChoose.Contains(district))
        {
            throw new ArgumentException("District is not in the allowed list", nameof(district));
        }

        Player.AddDistricts(district);
        //TODO put the rest under the deck
        GatherActionInProgress = false;
        GatherActionDone = true;
    }

    internal void BuildDistrict(District district)
    {
        Player.BuildDistrict(district);
        DistrictBuiltCount++;
    }

    internal bool ActionAvaialble<TAction>() where TAction : IAction
        => TurnActionPool.IsActionAvailable<TAction>();

    internal void ExecuteAction(ISimpleAction simpleAction)
    {
        simpleAction.Execute(Game);
        MarkActionDone(simpleAction);
    }

    internal void ExecuteAction(ICharacterAction characterAction, Character character)
    {
        characterAction.Execute(Game, character);
        MarkActionDone(characterAction);
    }

    internal void ExecuteAction(IPlayerDistrictAction playerDistrictAction, Player player, District district)
    {
        playerDistrictAction.Execute(Game, player, district);
        MarkActionDone(playerDistrictAction);
    }

    private void MarkActionDone(IAction action)
        => TurnActionPool.MarkActionDone(action.GetType());
}
