var lanternFish = File
    .ReadAllLines("PuzzleInput.txt")
    .SelectMany(line => line.Split(','))
    .Select(x => Convert.ToInt32(x))
    .InitialDayState()
    .AddDailyStates();

var part1 = lanternFish
    .SumAtDay(80);

Console.WriteLine($"Part 1: {part1}"); 

var part2 = lanternFish
     .SumAtDay(256);

Console.WriteLine($"Part 2: {part2}");

static class Extensions
{
    public static long SumAtDay(this long[,] days, int day)
    {
        long total = 0;
        for (int k = 0; k < 9; k++) {
            total += days[day, k];
        }
        return total;
    }
    public static long[,] InitialDayState(this IEnumerable<int> lanternFish)
    {
        var days = new long[270, 9];

        foreach (var fish in lanternFish)
        {
            days[0, fish]++;
        }

        return days;
    }

    public static long[,] AddDailyStates(this long[,] days)
    {
        for (int i = 0; i < 256; i++)
        {
            days[i+1, 8] += days[i, 0];
            days[i+1, 6] += days[i, 0];

            for (int j = 1; j < 9; j++)
            {
                days[i+1, j-1] += days[i, j];
            }
        }

        return days;
    }
}