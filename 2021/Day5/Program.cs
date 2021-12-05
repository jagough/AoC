var vents = File
    .ReadAllLines("PuzzleInput.txt")
    .ParseVents();

var part1 = vents
    .Where(Extensions.HorizontalOrVertical)
    .SelectMany(Extensions.GetIntermediateIndexes)
    .BuildMap()
    .Where(x => x > 1)
    .Count();

Console.WriteLine($"Part 1: {part1}");

var part2 = vents
    .SelectMany(Extensions.GetIntermediateIndexes)
    .BuildMap()
    .Where(x => x > 1)
    .Count();

Console.WriteLine($"Part 2: {part2}");
static class Extensions
{
    public static IEnumerable<((int x, int y) from, (int x, int y) to)> ParseVents(this IEnumerable<string> lines)
    {
        return lines.Select(ParseVent);
    }
    public static ((int x, int y) from, (int x, int y) to) ParseVent(string line)
    {
        var input = line
            .Split(new[] { " -> " }, StringSplitOptions.None)
            .SelectMany(x => x.Split(','))
            .Select(x => Convert.ToInt32(x))
            .ToArray();
        return ((input[0], input[1]), (input[2], input[3]));
    }

    public static bool HorizontalOrVertical(((int x, int y) from, (int x, int y) to) vent)
    {
        return vent.IsHorizontal() || vent.IsVertical();
    }

    public static bool IsHorizontal(this ((int x, int y) from, (int x, int y) to) vent)
    {
        return vent.from.x == vent.to.x;
    }
    public static bool IsVertical(this ((int x, int y) from, (int x, int y) to) vent)
    {
        return vent.from.y == vent.to.y;
    }
    public static bool IsAscendingDiagonal(this ((int x, int y) from, (int x, int y) to) vent, out ((int x, int y) from, (int x, int y) to) outVent)
    {
        outVent = vent;

        if (vent.from.x < vent.to.x && vent.from.y < vent.to.y) return true;

        if (vent.to.x < vent.from.x && vent.to.y < vent.from.y)
        {
            outVent = (vent.to, vent.from);
            return true;
        }
        return false;
    }
    public static bool IsDescendingDiagonal(this ((int x, int y) from, (int x, int y) to) vent, out ((int x, int y) from, (int x, int y) to) outVent)
    {
        outVent = vent;

        if (vent.from.x < vent.to.x && vent.from.y > vent.to.y) return true;

        if (vent.to.x < vent.from.x && vent.to.y > vent.from.y)
        {
            outVent = (vent.to, vent.from);
            return true;
        }
        return false;
    }
    public static int[] BuildMap(this IEnumerable<(int x, int y)> vents)
    {
        var max = vents.Aggregate((a, b) => (Math.Max(a.x, b.x), Math.Max(a.y, b.y)));
        var map = new int[(max.x + 1) * (max.y + 1)];
        foreach (var vent in vents)
        {
            map[vent.x + vent.y * (max.x + 1)]++;
        }
        return map;
    }

    public static IEnumerable<(int x, int y)> GetIntermediateIndexes(((int x, int y) from, (int x, int y) to) vent)
    {
        if (vent.IsHorizontal())
        {
            for (var i = Math.Min(vent.from.y, vent.to.y); i <= Math.Max(vent.from.y, vent.to.y); i++)
            {
                yield return (vent.from.x, i);
            }
        }
        if (vent.IsVertical())
        {
            for (var i = Math.Min(vent.from.x, vent.to.x); i <= Math.Max(vent.from.x, vent.to.x); i++)
            {
                yield return (i, vent.from.y);
            }
        }
        if (vent.IsAscendingDiagonal(out var ascVent))
        {
            for (var i = ascVent.from; i.x <= ascVent.to.x; i.x++, i.y++)
            {
                yield return i;
            }
        }
        if (vent.IsDescendingDiagonal(out var descVent))
        {
            for (var i = descVent.from; i.x <= descVent.to.x; i.x++, i.y--)
            {
                yield return i;
            }
        }
    }
}