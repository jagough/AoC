using FluentAssertions;
using Xunit;

namespace Day13;


public class Tests
{
    [Fact]
    public void Assert_Dots_ReadOk()
    {
        var dots = File
           .ReadAllLines("TestInput.txt")
           .Dots()
           .Print();

@"...#..#..#.
....#......
...........
#..........
...#....#.#
...........
...........
...........
...........
...........
.#....#.##.
....#......
......#...#
#..........
#.#........
".Should().BeEquivalentTo(dots);
    }

    [Fact]
    public void Assert_Dots_FoldsOk()
    {
        var dots = File
           .ReadAllLines("TestInput.txt")
           .Dots();

        var folds = File
           .ReadAllLines("TestInput.txt")
           .Folds();

        var result = dots
            .Fold(folds)
            .Print();

        @"#####
#...#
#...#
#...#
#####
".Should().BeEquivalentTo(result);
    }

    [Theory]
    [InlineData(0, 3, 0)]
    [InlineData(1, 3, 1)]
    [InlineData(2, 3, 2)]
    [InlineData(6, 3, 0)]
    [InlineData(5, 3, 1)]
    [InlineData(4, 3, 2)]
    [InlineData(6, 5, 4)]
    [InlineData(4, 5, 4)]
    public void Assert_FoldY_Ok(int y, int fold, int result)
    {
        (0, y).FoldY(fold).Should().Contain((0, result));
    }

    [Theory]
    [InlineData(6, 3, 2)]
    [InlineData(5, 3, 1)]
    [InlineData(4, 3, 0)]
    [InlineData(2, 3, 0)]
    [InlineData(1, 3, 1)]
    [InlineData(0, 3, 2)]
    [InlineData(0, 2, 1)]
    [InlineData(1, 2, 0)]
    [InlineData(3, 2, 0)]
    public void Assert_FoldXEven_Ok(int x, int fold, int result)
    {
        (x, 0).FoldX(fold).Should().Contain((result, 0));
    }
}