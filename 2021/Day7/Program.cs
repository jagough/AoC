var crabs = File
    .ReadAllLines("PuzzleInput.txt")
    .SelectMany(x => x.Split(','))
    .Select(x => Convert.ToInt32(x));

var part1 = Enumerable
            .Range(crabs.Min(), crabs.Max())
            .Select(x => crabs.BasicFuelCost(x))
            .Min();
Console.WriteLine($"Part 1: {part1}"); 

var part2 = Enumerable
             .Range(crabs.Min(), crabs.Max())
             .Select(x => crabs.AdvancedFuelCost(x))
             .Min();
Console.WriteLine($"Part 2: {part2}");
static class Extensions
{
    public static int BasicFuelCost(this IEnumerable<int> crabs, int position)
    {
        return crabs.Sum(x => Math.Abs(x - position));
    }
    public static int AdvancedFuelCost(this IEnumerable<int> crabs, int position)
    {
        return crabs.Sum(x => SumSeries(Math.Abs(x - position)));
    }
    public static int SumSeries(int i)
    {
        return i * (i + 1) / 2;
    }
}