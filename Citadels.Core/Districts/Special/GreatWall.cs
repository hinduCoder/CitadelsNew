namespace Citadels.Core.Districts.Special;

[DistrictName("Great Wall")]
internal class GreatWall : District
{
    public GreatWall(string name, DistrictKind kind, int buildPrice) 
        : base(name, kind, buildPrice)
    {
    }
}
