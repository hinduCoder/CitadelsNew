namespace Citadels.Core.Districts;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal class DistrictNameAttribute : Attribute
{
    public string Name { get; }

    public DistrictNameAttribute(string name) => Name = name;
}
