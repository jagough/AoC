var lines = File.ReadAllLines("PuzzleInput.txt");

var part1 = lines
    .Instructions()
    .MovesUnaimed()
    .Sum()
    .Multiply();

Console.WriteLine($"Part 1: {part1}"); 

var part2 = lines
     .Instructions()
     .MovesWithAim()
     .Sum()
     .Multiply();

Console.WriteLine($"Part 2: {part2}");

public static class IEnumberableExtensions
{
    public static (int, int) Sum(this IEnumerable<(int x, int y)> moves)
    {
        return (moves.Select(m => m.x).Sum(),
            moves.Select(m =>m.y).Sum());
    }

    public static (int, int) Sum(this IEnumerable<(int x, int y, int aim)> moves)
    {
        return moves.Select(m => (m.x, m.y)).Sum();
    }
    public static int Multiply(this (int x, int y) sum)
    {
        return sum.x * sum.y;
    }
    public static IEnumerable<(string, int)> Instructions(this IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            string[] s = line.Split(' ');
            yield return (s[0], Convert.ToInt32(s[1]));
        }
    }
    public static IEnumerable<(int, int)> MovesUnaimed(this IEnumerable<(string cmd, int value)> instructions)
    {
        foreach (var ins in instructions)
        {
            yield return ins.cmd switch
            {
                "forward" => (ins.value, 0),
                "up" => (0, -ins.value),
                "down" => (0, ins.value),
                _ => throw new Exception()
            };
        }
    }
    public static IEnumerable<(int, int, int)> MovesWithAim(this IEnumerable<(string cmd, int value)> instructions)
    {
        int aim = 0;
        foreach (var ins in instructions)
        {
            var @new = ins.cmd switch
            {
                "forward" => (ins.value, ins.value * aim, 0),
                "up" => (0, 0, -ins.value),
                "down" => (0, 0, ins.value),
                _ => throw new Exception()
            };
            aim += @new.Item3;
            yield return @new;
        }
    }
}

public class Move
{
    public int X { get; set; }
    public int Y { get; set; }
    public (int X, int Y) GetValues()
    {
        return (X, Y);
    }
}