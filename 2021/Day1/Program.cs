var lines = File.ReadAllLines("PuzzleInput.txt");

var values = ConvertLines(lines).ToArray();
Console.WriteLine($"Part 1: {CountIncreases(values)}");

var windows = CalculateWindows(values).ToArray();
Console.WriteLine($"Part 2: {CountIncreases(windows)}");

IEnumerable<int> ConvertLines(IEnumerable<string> lines)
{
    return lines.Select(x => Convert.ToInt32(x));
}

IEnumerable<int> CalculateWindows(int[] lines)
{
    for (var i = 0; i < lines.Length - 2; i++)
    {
        yield return lines[i] + lines[i + 1] + lines[i + 2];
    }
}

IEnumerable<bool> DetermineIncreases(int[] values)
{
    for (var i = 0; i < values.Length - 1; i++)
    {
        yield return values[i] < values[i + 1];
    }
}
int CountIncreases(int[] values)
{
    return DetermineIncreases(values).Count(x => x);
}