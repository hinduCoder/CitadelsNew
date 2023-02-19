namespace Citadels.Core.Districts.Special;

[DistrictName("Dragon Gate")]
internal class DragonGate : District
{
    public override int Points => BuildPrice + 2;
    public DragonGate(string name, DistrictKind kind, int buildPrice)
        : base(name, kind, buildPrice)
    {
    }
}