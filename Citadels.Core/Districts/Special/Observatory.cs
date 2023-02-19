namespace Citadels.Core.Districts.Special;

[DistrictName("Observatory")]
internal class Observatory : District
{
    public Observatory(string name, DistrictKind kind, int buildPrice) : base(name, kind, buildPrice)
    {
    }
}
