namespace Citadels.Core.Districts.Special;

[DistrictName("School of Magic")]
internal class MagicScool : District
{
    public override bool CountAsAnyTypeDuringGatheringCoins => true;
    public MagicScool(string name, DistrictKind kind, int buildPrice) 
        : base(name, kind, buildPrice)
    {
    }
}
