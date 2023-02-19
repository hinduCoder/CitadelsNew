namespace Citadels.Core.Districts.Special;

[DistrictName("Keep")]
internal class Keep : District
{
    public override bool CanBeDestroyed => true;
    public Keep(string name, DistrictKind kind, int buildPrice)
        : base(name, kind, buildPrice)
    {
    }
}
