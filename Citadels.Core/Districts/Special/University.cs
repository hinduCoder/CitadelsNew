namespace Citadels.Core.Districts.Special;

[DistrictName("University")]
internal class University : District
{
    public override int Points => BuildPrice + 2;
    public University(string name, DistrictKind kind, int buildPrice) 
        : base(name, kind, buildPrice)
    {
    }
}
