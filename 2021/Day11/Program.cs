var energyLevels = File
    .ReadAllLines("PuzzleInput.txt")
    .Select(l => l
        .Select(c => (int) char
            .GetNumericValue(c))
        .ToArray())
    .ToArray();

//var part1 = energyLevels
//    .ProcessSteps()
//    .Take(100)
//    .Sum();

//Console.WriteLine($"Part 1: {part1}");

var part2 = energyLevels
    .ProcessSteps()
    .Select((f, i) => (f, i))
    .First( x => x.f == 100).i + 1;

Console.WriteLine($"Part 2: {part2}");

static class Extensions
{
    public static IEnumerable<int> ProcessSteps(this int[][] energyLevels)
    {
        do
        {
            var flashed = new List<(int x, int y)>();
            energyLevels.Increment();
            do flashed.AddRange(energyLevels.Flash(energyLevels.FindFlashPoints()));
            while (energyLevels.FindFlashPoints().Any());
            yield return flashed.Count;
        } while (true);
    }

    public static IEnumerable<(int x, int y)> FindFlashPoints(this int[][] energyLevels)
    {
        return energyLevels
            .SelectMany((row, y) => row
                .Select((e, x) => (x, y, e)))
            .Where(p => p.e > 9)
            .Select(p => (p.x, p.y));
    }
    public static IEnumerable<(int x, int y)> Flash(this int[][] energyLevels, IEnumerable<(int x, int y)> points)
    {
        return points.Select(energyLevels.Flash);
    }    
    
    public static (int x, int y) Flash(this int[][] energyLevels, (int x, int y) p)
    {
        energyLevels[p.y][p.x] = 0;
        for (var y = Math.Max(p.y - 1, 0) ; y <= Math.Min(p.y + 1, energyLevels.Length - 1); y++)
        {
            for (var x = Math.Max(p.x - 1, 0); x <= Math.Min(p.x + 1, energyLevels[0].Length - 1); x++)
            {
                if (energyLevels[y][x] != 0) energyLevels[y][x]++;
            }
        }
        return p;
    }

    public static void Increment(this int[][] grid)
    {
        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[i].Length; j++)
            {
                grid[j][i]++;
            }
        }
    }
}