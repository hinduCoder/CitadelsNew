using System.Reflection;

namespace Citadels.Core.Districts;

public class District : IEquatable<District>
{
    #region raw list
    private static readonly string RawList = 
@"5 Tavern 1 green 
4 Market 2 green
3 Trading Post 2 green
3 Docks 3 green
3 Harbor 4 green
2 Town Hall 5 green
3 Temple 1 blue
3 Church 2 blue
3 Monastery 3 blue
2 Cathedral 5 blue
3 Watchtower 1 red
3 Prison 2 red
3 Battlefield 3 red
2 Fortress 5 red
5 Manor 3 yellow
4 Castle 4 yellow
3 Palace 5 yellow
1 Haunted City 2 purple
2 Keep 3 purple
1 Laboratory 5 purple
1 Smithy 5 purple
1 Graveyard 5 purple
1 Observatory 4 purple
1 School of Magic 6 purple
1 Library 6 purple
1 Great Wall 6 purple
1 University 6 purple
1 Dragon Gate 6 purple
1 Imperial Treasure 5 purple
1 Map Room 5 purple";
    #endregion
    static District()
    {
        using var stringReader = new StringReader(RawList);
        var districts = new List<District>();
        while (true)
        {
            var line = stringReader.ReadLine()?.TrimEnd();
            if (line == null)
                break;
            var (count, name, price, color) = line.Split(' ') switch { var a => (int.Parse(a[0]), string.Join(' ', a[1..^2]), int.Parse(a[^2]), a[^1]) };

            for (var i = 0; i < count; i++)
            {
                var kind = color switch
                {
                    "yellow" => DistrictKind.Noble,
                    "red" => DistrictKind.Military,
                    "green" => DistrictKind.Trade,
                    "purple" => DistrictKind.Special,
                    "blue" => DistrictKind.Religious,
                    _ => throw new NotSupportedException()
                };
                var type = Assembly.GetExecutingAssembly().DefinedTypes
                    .Where(t => t.BaseType == typeof(District))
                    .Where(t => t.GetCustomAttribute<DistrictNameAttribute>()?.Name == name)
                    .FirstOrDefault();
                if (type is null)
                {
                    districts.Add(new(name, kind, price));
                } else {
                    districts.Add((District)Activator.CreateInstance(type, name, kind, price)!);
                }
            }
        }
        Pool = districts;
    }
    public static IReadOnlyList<District> Pool { get; private set; }

    public int BuildPrice { get; private set; }
    public string Name { get; private set; }
    public DistrictKind Kind { get; private set; }

    public virtual bool CanBeDestroyed => true;
    public virtual int Points => BuildPrice;

    public bool Equals(District? other) => other?.Name == Name && other?.Kind == Kind;

    public override bool Equals(object? obj) => Equals(obj as District);

    public override int GetHashCode() => HashCode.Combine(Name, Kind);


    protected District(string name, DistrictKind kind, int buildPrice)
    {
        Name = name;
        Kind = kind;
        BuildPrice = buildPrice;
    }
}

