namespace Citadels.Core.Districts.Special;

[DistrictName("Map Room")]
internal class MapRoom : District
{
    public MapRoom(string name, DistrictKind kind, int buildPrice)
        : base(name, kind, buildPrice)
    {
    }
}
