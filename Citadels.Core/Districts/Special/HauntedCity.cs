namespace Citadels.Core.Districts.Special;

[DistrictName("Haunted City")]
internal class HauntedCity : District
{
    public override bool CountAsAnyTypeAtTheEnd => true;
    public HauntedCity(string name, DistrictKind kind, int buildPrice)
        : base(name, kind, buildPrice)
    {
    }
}
