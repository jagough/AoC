var heights = File
    .ReadAllLines("PuzzleInput.txt")
    .Select(line => line.Select(x => (int) char.GetNumericValue(x)).ToArray()).ToArray();

var part1 = heights
    .RiskLevel();

Console.WriteLine($"Part 1: {part1}");

var part2 = heights
     .Basins()
     .OrderByDescending(x => x)
     .Take(3)
     .Aggregate((x, y) => x * y);

Console.WriteLine($"Part 1: {part2}");
static class Extensions
{
    public static int RiskLevel(this int[][] heightMap)
    {
        return heightMap
            .SelectMany((row, i) =>
                row.Select((col, j) =>
                    heightMap.LowestConnectedPoint((j, i))))
            .Where(x => x != (-1, -1))
            .Distinct()
            .Select(i => 1 + heightMap[i.y][i.x])
            .Sum();
    }
    public static IEnumerable<int> Basins(this int[][] height)
    {
        return height
            .SelectMany((row, i) =>
                row.Select((col, j) =>
                    height.LowestConnectedPoint((j, i))))
            .Where(x => x != (-1, -1))
            .GroupBy(x => x)
            .Select(gp => gp.Count());
    }
    public static (int x, int y) LowestConnectedPoint(this int[][] height, (int x, int y) i)
    {
        if (height[i.y][i.x] == 9) return (-1, -1);

        while (height.GetLowerPoint(i, out var lowerPoint))
        {
            i = lowerPoint;
        }
        return i;
    }
    public static bool GetLowerPoint(this int[][] height, (int x, int y) i, out (int x, int y) lower)
    {
        var value = height[i.y][i.x];
        var row = height.Skip(i.y).First();
        var col = height.Select(row => row[i.x]).ToArray();
        lower = (-1, -1);

        if (i.x != 0 && row[i.x - 1] < row[i.x]) lower = (i.x - 1, i.y);
        if (i.x != row.Length - 1 && row[i.x + 1] < row[i.x]) lower = (i.x + 1, i.y);
        if (i.y != 0 && col[i.y - 1] < col[i.y]) lower = (i.x, i.y - 1);
        if (i.y != col.Length - 1 && col[i.y + 1] < col[i.y]) lower = (i.x, i.y + 1);

        return lower != (-1, -1);
    }
}