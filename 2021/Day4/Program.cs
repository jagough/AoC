var input = File.ReadAllLines("PuzzleInput.txt").ParseInput();

var part1 = input.FindWinningCards().First().CalculateScore();

Console.WriteLine($"Part 1: {part1}");

var part2 = input.FindWinningCards().Reverse().First().CalculateScore();

Console.WriteLine($"Part 2: {part2}");
static class Extensions
{
    public static IEnumerable<(int[] values, bool[] hits, int winningDraw)> FindWinningCards(this (IEnumerable<int> draws, IEnumerable<int[]> cardValues) input)
    {
        var cards = input.cardValues.Select(GetCard);
        foreach (var draw in input.draws)
        {
            cards = cards.MarkDraws(draw);
            
            foreach (var c in cards.Where(Bingo).Select(x => (x.values, x.hits, draw)))
            {
                yield return c;
            };

            cards = cards.Where(x => !Bingo(x));
        }
    }

    public static (int[] values, bool[] hits) GetCard(int[] values)
    {
        return (values, new bool[values.Length]);
    }

    public static (IEnumerable<int> draws, IEnumerable<int[]> cardValues) ParseInput(this IEnumerable<string> lines)
    {
        var draws = lines.First().Split(',').Select(x => Convert.ToInt32(x));
        return (draws, lines.Skip(1).ParseCardsValues());
    }

    public static IEnumerable<int[]> ParseCardsValues(this IEnumerable<string> lines)
    {
        return lines.
            Where(l => l != string.Empty)
            .SelectMany(l => l
                .Split(' ')
                .Where(s => s != string.Empty)
                .Select(v => Convert.ToInt32(v)))
            .Select((v, i) => (i/25, v))
            .GroupBy(g => g.Item1)
            .Select(gr => gr.Select(x => x.Item2).ToArray());
    }

    public static IEnumerable<int[]> BingoIndexes()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new int[] { 0 + 5 * i, 1 + 5 * i, 2 + 5 * i, 3 + 5 * i, 4 + 5 * i };
            yield return new int[] { 0 + i, 5 + i, 10 + i, 15 + i, 20 + i };
        }
    }

    public static IEnumerable<(int[] values, bool[] hits)> MarkDraws(this IEnumerable<(int[] values, bool[] hits)> cards, int draw)
    {
        foreach (var card in cards)
        {
            yield return card.MarkDraw(draw);
        }
    }

    public static (int[] values, bool[] hits) MarkDraw(this (int[] values, bool[] hits) card, int draw)
    {
        var i = Array.FindIndex(card.values, 0, card.values.Length, v => v == draw);
        if (i > -1) card.hits[i] = true;
        return card;
    }

    public static bool Bingo((int[] values, bool[] hits) card)
    {
        return BingoIndexes().Any(indices => indices.All(index => card.hits[index]));
    }

    public static int CalculateScore(this (int[] values, bool[] hits, int winningDraw) card)
    {
        return card.values.Select((v, i) => card.hits[i] ? 0 : v).Sum() * card.winningDraw;
    }
}