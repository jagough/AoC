var input = File.ReadAllLines("PuzzleInput.txt");

var part1 = input
    .Select(Extensions.Corrupted)
    .Where(x => x is not null)
    .Sum(x => Extensions.CorruptScore[Extensions.Close.IndexOf(x.Value.found)]);

Console.WriteLine($"Part 1: {part1}");

var part2 = input
    .NotCorrupted()
    .Select(Extensions.Complete)
    .Score();

Console.WriteLine($"Part 2: {part2}");
static class Extensions
{
    public static readonly List<char> Open = new List<char> { '(', '[', '{', '<' };
    public static readonly List<char> Close = new List<char> { ')', ']', '}', '>' };
    public static readonly int[] CorruptScore = new [] { 3, 57, 1197, 25137 };

    public static (char expected, char found)? Corrupted(string line)
    {
        var expected = new Stack<int>();
        foreach (var c in line)
        {
            var i = Open.IndexOf(c);
            if (i > -1) expected.Push(i);
            else
            {
                if (expected.Peek() == Close.IndexOf(c)) expected.Pop();
                else
                {
                    return (Close[expected.Peek()], c);
                }
            }
        }
        return null;
    }

    public static IEnumerable<string> NotCorrupted(this IEnumerable<string> lines)
    {
        return lines.Where(s => Corrupted(s) == null);
    }

    public static long Score(this IEnumerable<IEnumerable<long>> completionSuffix)
    {
        var suffixes = completionSuffix
            .Select(s => s
                .Select(x => x + 1)
                .Aggregate((x, y) => x * 5 + y))
            .OrderByDescending(x => x)
            .ToArray();

        return suffixes[suffixes.Length/2];
    }

    public static IEnumerable<long> Complete(string line)
    {
        var expected = new Stack<long>();
        foreach (var c in line)
        {
            var i = Open.IndexOf(c);
            if (i > -1) expected.Push(i);
            else expected.Pop();
        }

        return expected;
    }
}