var input = File.ReadAllLines("PuzzleInput.txt").Displays();

var part1 = input
    .SelectMany(x => x.output)
    .Where(x => new int[] {2, 3, 4, 7 }.Contains(x.Length))
    .Count();

Console.WriteLine($"Part 1: {part1}");

var part2 = input
    .DecodeDisplays()
    .Sum();

Console.WriteLine($"Part 2: {part2}");
static class Extensions
{
    public static IEnumerable<(IEnumerable<string> patterns, IEnumerable<string> output)> Displays(this IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var l = line.Split(new[] { " | " }, StringSplitOptions.None);
            yield return (l[0].Split(' '), l[1].Split(' '));
        }
    }

    public static IEnumerable<int> DecodeDisplays(this IEnumerable<(IEnumerable<string> patterns, IEnumerable<string> output)> displays)
    {
        foreach (var display in displays)
        {
            var solutions = display.patterns.FindSolutionsByCount();
            var decoded = display.output.Select(x => x.DecodeValue(solutions)).ToArray();
            yield return decoded.ReadDisplay();
        }
    }

    public static (char coded, char actual)[] FindSolutionsByCount(this IEnumerable<string> patterns)
    {
        //var remaining = patterns.Select(x => solutions.SelectMany(s => x.Replace(s.error, 'x')));
        var totals = patterns.SelectMany(x => x).GroupBy(x => x).Select(gp => (gp.Key, gp.Count()));
        var b = (totals.First(x => x.Item2 == 6).Key, 'b');
        var e = (totals.First(x => x.Item2 == 4).Key, 'e');
        var f = (totals.First(x => x.Item2 == 9).Key, 'f');
        var c = (patterns.Where(x => x.Length == 2).First().Where(x => x != f.Item1).First(), 'c');
        var a = (totals.Where(x => x.Item2 == 8).First(x => x.Key != c.Item1).Key, 'a');
        var d = (patterns.Where(x => x.Length == 4).First().Where(x => x != f.Item1 && x != b.Item1 && x != c.Item1).First(), 'd');
        var g = (totals.Where(x => x.Item2 == 7).First(x => x.Key != d.Item1).Key, 'g');
        return new [] { a, b, c, d, e, f, g };
    }
    public static char[] DecodeValue(this string s, (char coded, char actual)[] solutions)
    {
        return s.Select(x => solutions.Where(s => s.coded == x).First().actual).ToArray();
    }
    public static int ReadDisplay(this char[][] decoded)
    {
        int value = 0;
        value += decoded[0].ReadValue() * 1000;
        value += decoded[1].ReadValue() * 100;
        value += decoded[2].ReadValue() * 10;
        value += decoded[3].ReadValue() * 1;
        return value;
    }

    public static int ReadValue(this char[] str)
    {
        return TrueSegments.Select((s, i) => (s.SetEquals(str), i)).First(x => x.Item1).i;
    }


    public static HashSet<char>[] TrueSegments = new HashSet<char>[]
    {
        new HashSet<char> { 'a', 'b', 'c', 'e', 'f', 'g' },
        new HashSet<char> { 'c', 'f' },
        new HashSet<char> { 'a', 'c', 'd', 'e', 'g' },
        new HashSet<char> { 'a', 'c', 'd', 'f', 'g' },
        new HashSet<char> { 'b', 'c', 'd', 'f' },
        new HashSet<char> { 'a', 'b', 'd', 'f', 'g' },
        new HashSet<char> { 'a', 'b', 'd', 'e', 'f', 'g' },
        new HashSet<char> { 'a', 'c', 'f' },
        new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' },
        new HashSet<char> { 'a', 'b', 'c', 'd', 'f', 'g' },
    };
}