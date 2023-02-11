namespace Citadels.Core.Characters;

public class Bishop : Character
{
    public override int Rank => 5;
    public override bool CanDistrictsBeDesctroyed => !IsAlive;
}
