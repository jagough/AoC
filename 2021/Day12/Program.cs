IEnumerable<(string orig, string dest)> connections = File
    .ReadAllLines("PuzzleInput.txt")
    .Select(s => s.Split('-'))
    .SelectMany(c => new[] { c, c.Reverse().ToArray() })
    .Select(c => (c[0], c[1]))
    .Where(c => c.Item1 != "end" && c.Item2 != "start")
    .ToList();


var part1 = connections
    .Initial()
    .Paths()
    .Count(x => x.Last() == "end");

Console.WriteLine($"Part 1: {part1}");

var part2 = connections
    .Initial(true)
    .Paths()
    .Count(x => x.Last() == "end"); ;

Console.WriteLine($"Part 2: {part2}");


static class Extensions
{
    public static IEnumerable<(IEnumerable<string> caves, IEnumerable<(string orig, string dest)> connections, bool canRevisit)> Initial(this IEnumerable<(string orig, string dest)> connections, bool canRevisit = false)
    {
        return new List<(IEnumerable<string>, IEnumerable<(string, string)>, bool)> { (new[] { "start" }, connections, canRevisit) };
    }
    public static IEnumerable<IEnumerable<string>> Paths(this IEnumerable<(IEnumerable<string> caves, IEnumerable<(string orig, string dest)> connections, bool canRevisit)> paths)
    {
        do paths = paths.SelectMany(BranchingPaths).ToList();
        while (paths.Any(CanContinue));
        return paths.Select(path => path.caves);
    }

    public static IEnumerable<(IEnumerable<string> caves, IEnumerable<(string orig, string dest)> connections, bool canRevisit)> BranchingPaths((IEnumerable<string> caves, IEnumerable<(string orig, string dest)> connections, bool canRevisit) path)
    {
        if (!CanContinue(path))
        {
            yield return path;
            yield break;
        }
        foreach (var conn in path.connections.Where(c => c.orig == path.caves.Last()))
        {
            var caves = path.caves.Append(conn.dest).ToArray();
            if (!conn.dest.IsSmallCave())
            {
                yield return (caves, path.connections.ToArray(), path.canRevisit);
            }
            else
            {
                var canRevisit = path.canRevisit && caves
                    .Where(x => x.IsSmallCave())
                    .GroupBy(x => x)
                    .Select(gp => gp.Count())
                    .Max() < 2;
                var connections = canRevisit ? 
                    path.connections : 
                    path.connections
                        .Where(con => !caves
                            .Where(x => x.IsSmallCave())
                            .Contains(con.dest));

                yield return (caves, connections.ToArray(), canRevisit);
            }
        }
    }

    public static bool CanContinue((IEnumerable<string> caves, IEnumerable<(string orig, string dest)> connections, bool canRevisit) path)
    {
        var last = path.caves.Last();
        return path.connections.Any(c => c.orig == last);
    }

    public static bool IsSmallCave(this string s)
    {
        return char.IsLower(s[0]);
    }
}