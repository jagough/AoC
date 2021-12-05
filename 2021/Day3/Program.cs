var lines = File.ReadAllLines("PuzzleInput.txt");
var part1 = lines.Readings()
    .MostCommonValues()
    .PowerRates()
    .Multiply();

Console.WriteLine($"Part 1: {part1}");

var part2 = lines.Readings()
    .LifeRates()
    .Multiply();
Console.WriteLine($"Part 2: {part2}");

public static class DiagnosticExtensions
{
    public static uint Multiply(this (uint rate1, uint rate2) rates)
    {        
        return rates.rate1 * rates.rate2;
    }
    public static IEnumerable<bool[]> Readings(this IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            yield return line.Select(x => x == '1').ToArray();
        }
    }

    public static IEnumerable<T> MostCommonValues<T>(this IEnumerable<T[]> readings)
    {
        for (var i = 0; i < readings.First().Length; i++)
        {
            yield return readings
                .Select(x => x[i])
                .MostCommonValue();
        }
    }

    public static T MostCommonValue<T>(this IEnumerable<T> values)
    {
        return values
            .GroupBy(x => x)
            .OrderByDescending(gp => gp.Count())
            .ThenByDescending(gp => gp.Key)
            .Select(gp => gp.Key)
            .First();
    }

    public static (uint gamma, uint epsilon) PowerRates(this IEnumerable<bool> mostCommonValues)
    {
        var gamma = new System.Collections.BitArray(mostCommonValues.Reverse().ToArray());
        var epsilon = new System.Collections.BitArray(mostCommonValues.Reverse().Select(x => !x).ToArray());

        var result = new uint[2];

        gamma.CopyTo(result, 0);
        epsilon.CopyTo(result, 1);
        return (result[0], result[1]);
    }

    public static (uint oxy, uint co2) LifeRates(this IEnumerable<bool[]> readings)
    {
        List<bool[]> oxyReads = readings.ToList();
        List<bool[]> co2Reads = readings.ToList();
        int index = 0;
        do
        {
            var mcv = oxyReads.Select(x => x[index]).MostCommonValue();
            oxyReads = oxyReads.Where(x => x[index] == mcv).ToList();
            index++;
        }
        while (oxyReads.Count() > 1);

        index = 0;

        do
        {
            var mcv = co2Reads.Select(x => x[index]).MostCommonValue();
            co2Reads = co2Reads.Where(x => x[index] == !mcv).ToList();
            index++;
        }
        while (co2Reads.Count() > 1);

        var oxy = new System.Collections.BitArray(oxyReads.First().Reverse().ToArray());
        var co2 = new System.Collections.BitArray(co2Reads.First().Reverse().ToArray());

        var result = new uint[2];

        oxy.CopyTo(result, 0);
        co2.CopyTo(result, 1);
        return (result[0], result[1]);
    }
}