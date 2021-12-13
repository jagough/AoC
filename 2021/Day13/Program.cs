var dots = File
    .ReadAllLines("PuzzleInput.txt")
    .Dots();
var folds = File
    .ReadAllLines("PuzzleInput.txt")
    .Folds();

var part1 = dots.Fold(folds.First()).Distinct();

Console.WriteLine($"{part1.Count()}");

var part2 = dots.Fold(folds).Distinct().Print();

Console.WriteLine("Part 2:");
Console.WriteLine(part2); // For some reason this is mirrored
static class Extensions
{
    public static string Print(this IEnumerable<(int x, int y)> dots)
    {
        var sb = new System.Text.StringBuilder();

        for (int y = 0; y <= dots.Max(d => d.y); y++)
        {
            for (int x = 0; x <= dots.Max(d => d.x); x++)
            {
                sb.Append(dots.Any(d => d.x==x && d.y==y) ? '#' : '.');
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
    public static IEnumerable<(int x, int y)> Dots(this IEnumerable<string> lines)
    {
        return lines
            .Where(line => !line.StartsWith("fold") && line != string.Empty)
            .Select(line => line
                .Split(new[] { ',' })
                .Select(x => Convert.ToInt32(x))
                .ToArray())
            .Select(d => (d[0], d[1]))
            .ToArray();
    }

    public static IEnumerable<(bool axis, int fold)> Folds(this IEnumerable<string> lines)
    {
        return lines
            .Where(line => line.StartsWith("fold"))
            .Select(line => line.Split(' ').Last().Split('='))
            .Select(x => (x[0] == "x", Convert.ToInt32(x[1])));
    }

    public static IEnumerable<(int x, int y)> Fold(this IEnumerable<(int x, int y)> dots, IEnumerable<(bool axis, int fold)> folds)
    {
        foreach (var fold in folds)
        {
            dots = dots
                .Fold(fold)
                .Distinct()
                .ToArray();
        }
        return dots;
    }
    public static IEnumerable<(int x, int y)> Fold(this IEnumerable<(int x, int y)> dots, (bool axis, int fold) fold)
    {
        return dots.SelectMany(d => fold.axis ? d.FoldX(fold.fold) : d.FoldY(fold.fold));
    }

    public static IEnumerable<(int x, int y)> FoldY(this (int x, int y) dot, int fold)
    {
        if (fold == dot.y ) yield break;
        if (fold > dot.y ) yield return dot;
        if (fold < dot.y) yield return (dot.x, 2 * fold - dot.y);
    }

    public static IEnumerable<(int x, int y)> FoldX(this (int x, int y) dot, int fold)
    {
        if (fold == dot.x) yield break;
        if (fold < dot.x) yield return (dot.x - (fold + 1), dot.y);
        if (fold > dot.x) yield return (2 * fold - dot.x - (fold + 1), dot.y);
    }
}