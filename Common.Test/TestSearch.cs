using FluentAssertions;
using NUnit.Framework;
using static System.Math;

using matthiasffm.Common.Algorithms;

namespace matthiasffm.Common.Test;

internal class TestSearch
{
    #region breadth first

    [Test]
    public void TestBreadthFirstEnumerateEmpty()
    {
        // arrange

        // act

        var edgesNotFound = Search.BreadthFirstEnumerate("start", null, city => new List<string>{ });

        // assert

        edgesNotFound.Should().Equal("start");
    }

    [Test]
    public void TestBreadthFirstEnumerate()
    {
        // arrange

        // act

        var edgesFromKoeln      = Search.BreadthFirstEnumerate("Köln", null, city => SurroundingCities(city));
        var edgesFromHanau      = Search.BreadthFirstEnumerate("Hanau", null, city => SurroundingCities(city));
        var edgesFromIlbenstadt = Search.BreadthFirstEnumerate("Ilbenstadt", null, city => SurroundingCities(city));
        var edgesFromFrankfurt  = Search.BreadthFirstEnumerate("Frankfurt", null, city => SurroundingCities(city));

        // assert

        edgesFromKoeln.Should().Equal("Köln", "Bonn");
        edgesFromHanau.Should().Equal("Hanau", "Offenbach", "Frankfurt", "Erlensee", "Bad Vilbel", "Ilbenstadt", "Florstadt", "Friedberg");
        edgesFromIlbenstadt.Should().Equal("Ilbenstadt", "Bad Vilbel", "Florstadt", "Friedberg", "Frankfurt", "Hanau", "Offenbach", "Erlensee");
        edgesFromFrankfurt.Should().Equal("Frankfurt", "Bad Vilbel", "Hanau", "Offenbach", "Ilbenstadt", "Erlensee", "Florstadt", "Friedberg");
    }

    [Test]
    public void TestBreadthFirstSearchEmpty()
    {
        // arrange

        // act

        var pathNotFound = Search.BreadthFirstSearch("Hanau", "Bonn", city => SurroundingCities(city));

        // assert

        pathNotFound.Should().BeEmpty();
    }

    [Test]
    public void TestBreadthFirstSearch()
    {
        // arrange

        // act

        var pathFromKoelnToBonn          = Search.BreadthFirstSearch("Köln", "Bonn", city => SurroundingCities(city));
        var pathFromHanauToFriedberg     = Search.BreadthFirstSearch("Hanau", "Friedberg", city => SurroundingCities(city));
        var pathFromErlenseeToOffenbach  = Search.BreadthFirstSearch("Erlensee", "Offenbach", city => SurroundingCities(city));
        var pathFromFrankfurtToFrankfurt = Search.BreadthFirstSearch("Frankfurt", "Frankfurt", city => SurroundingCities(city));

        // assert

        pathFromKoelnToBonn.Should().Equal("Köln", "Bonn");
        pathFromHanauToFriedberg.Should().Equal("Hanau", "Frankfurt", "Bad Vilbel", "Ilbenstadt", "Friedberg");
        pathFromErlenseeToOffenbach.Should().Equal("Erlensee", "Hanau", "Offenbach");
        pathFromFrankfurtToFrankfurt.Should().Equal("Frankfurt");
    }

    #endregion

    #region depth first

    [Test]
    public void TestDepthFirstEnumerateEmpty()
    {
        // arrange

        // act

        var edgesNotFound = Search.DepthFirstEnumerate("start", null, city => new List<string> { });

        // assert

        edgesNotFound.Should().Equal("start");
    }

    [Test]
    public void TestDepthFirstEnumerate()
    {
        // arrange

        // act

        var edgesFromKoeln      = Search.DepthFirstEnumerate("Köln", null, city => SurroundingCities(city));
        var edgesFromHanau      = Search.DepthFirstEnumerate("Hanau", null, city => SurroundingCities(city));
        var edgesFromIlbenstadt = Search.DepthFirstEnumerate("Ilbenstadt", null, city => SurroundingCities(city));
        var edgesFromFrankfurt  = Search.DepthFirstEnumerate("Frankfurt", null, city => SurroundingCities(city));

        // assert

        edgesFromKoeln.Should().Equal("Köln", "Bonn");
        edgesFromHanau.Should().Equal("Hanau", "Erlensee", "Frankfurt", "Bad Vilbel", "Ilbenstadt", "Friedberg", "Florstadt", "Offenbach");
        edgesFromIlbenstadt.Should().Equal("Ilbenstadt", "Friedberg", "Florstadt", "Bad Vilbel", "Frankfurt", "Offenbach", "Hanau", "Erlensee");
        edgesFromFrankfurt.Should().Equal("Frankfurt", "Offenbach", "Hanau", "Erlensee", "Bad Vilbel", "Ilbenstadt", "Friedberg", "Florstadt");
    }

    [Test]
    public void TestDepthFirstSearchEmpty()
    {
        // arrange

        // act

        var pathNotFound = Search.DepthFirstSearch("Hanau", "Bonn", city => SurroundingCities(city));

        // assert

        pathNotFound.Should().BeEmpty();
    }

    [Test]
    public void TestDepthFirstSearch()
    {
        // arrange

        // act

        var pathFromKoelnToBonn             = Search.DepthFirstSearch("Köln", "Bonn", city => SurroundingCities(city));
        var pathFromHanauToFriedberg        = Search.DepthFirstSearch("Hanau", "Friedberg", city => SurroundingCities(city));
        var pathFromErlenseeToOffenbach     = Search.DepthFirstSearch("Erlensee", "Offenbach", city => SurroundingCities(city));
        var pathFromFrankfurtToFrankfurt    = Search.DepthFirstSearch("Frankfurt", "Frankfurt", city => SurroundingCities(city));

        // assert

        pathFromKoelnToBonn.Should().Equal("Köln", "Bonn");
        pathFromHanauToFriedberg.Should().Equal("Hanau", "Frankfurt", "Bad Vilbel", "Ilbenstadt", "Friedberg");
        pathFromErlenseeToOffenbach.Should().Equal("Erlensee", "Hanau", "Offenbach");
        pathFromFrankfurtToFrankfurt.Should().Equal("Frankfurt");
    }

    #endregion

    #region A star

    [Test]
    public void TestAStarEmpty()
    {
        // arrange

        // act

        var goalNotFound = Search.AStar("start",
                                        "goal",
                                        city => Array.Empty<string>(),
                                        (a, b) => 10L,
                                        city => 10L,
                                        1000L);

        // assert

        goalNotFound.Should().BeEmpty();
    }

    [Test]
    public void TestAStarCities()
    {
        // arrange

        // act

        var pathFromKoelnToBonn = Search.AStar("Köln",
                                               "Bonn",
                                               city => SurroundingCities(city),
                                               (a, b) => DistanceByStreet(a, b),
                                               city => DistanceByAir(city, "Bonn"),
                                               1000L);

        var pathFromErlenseeToFlorstadt = Search.AStar("Erlensee",
                                                       "Florstadt",
                                                       city => SurroundingCities(city),
                                                       (a, b) => DistanceByStreet(a, b),
                                                       city => DistanceByAir(city, "Florstadt"),
                                                       1000L);

        // assert

        pathFromKoelnToBonn.Should().Equal("Köln", "Bonn");
        pathFromErlenseeToFlorstadt.Should().Equal("Erlensee", "Hanau", "Frankfurt", "Bad Vilbel", "Ilbenstadt", "Florstadt");
    }

    [Test]
    public void TestAStarMazeManhattan()
    {
        // arrange

        var maze  = CreateMaze();
        var start = (x: 1, y: 0);
        var end   = (x: 7, y: 9);

        // act

        var pathFromTopToBottom = Search.AStar(start,
                                               pos => pos.x == end.x && pos.y == end.y,
                                               pos => MazeNeighbors4(maze, pos),
                                               (pos1, pos2) => 1,
                                               pos => MazeDistance(pos, end),
                                               1000);
        var pathBottomToTop = Search.AStar(end,
                                           pos => pos.x == start.x && pos.y == start.y,
                                           pos => MazeNeighbors4(maze, pos),
                                           (pos1, pos2) => 1,
                                           pos => MazeDistance(pos, start),
                                           1000);

        // assert

        var expectedPath = new[]
        {
            start,
            (1, 1),
            (1, 2),
            (2, 2),
            (2, 3),
            (2, 4),
            (1, 4),
            (0, 4),
            (0, 5),
            (0, 6),
            (1, 6),
            (2, 6),
            (3, 6),
            (3, 7),
            (4, 7),
            (5, 7),
            (6, 7),
            (7, 7),
            (8, 7),
            (8, 8),
            (8, 9),
            end,
        };

        pathFromTopToBottom.Should()
                           .HaveCount(expectedPath.Length).And
                           .Equal(expectedPath);

        pathBottomToTop.Should()
                       .HaveCount(expectedPath.Length).And
                       .Equal(expectedPath.Reverse());
    }

    [Test]
    public void TestAStarMazeDiag()
    {
        // arrange

        var maze  = CreateMaze();
        var start = (x: 1, y: 0);
        var end   = (x: 7, y: 9);

        // act

        var pathFromTopToBottom = Search.AStar(start,
                                               pos => pos.x == end.x && pos.y == end.y,
                                               pos => MazeNeighbors8(maze, pos),
                                               (pos1, pos2) => 1,
                                               pos => MazeDistanceDiag(pos, end),
                                               1000);
        var pathBottomToTop = Search.AStar(end,
                                           pos => pos.x == start.x && pos.y == start.y,
                                           pos => MazeNeighbors8(maze, pos),
                                           (pos1, pos2) => 1,
                                           pos => MazeDistanceDiag(pos, start),
                                           1000);

        // assert

        var expectedPath = new[]
        {
            start,
            (1, 1),
            (2, 2),
            (3, 2),
            (4, 2),
            (5, 3),
            (6, 3),
            (7, 3),
            (8, 4),
            (9, 5),
            (9, 6),
            (8, 7),
            (8, 8),
            end,
        };

        pathFromTopToBottom.Should()
                           .HaveCount(expectedPath.Length).And
                           .Equal(expectedPath);

        pathBottomToTop.Should()
                       .HaveCount(expectedPath.Length).And
                       .Equal(expectedPath.Reverse());
    }

    [Test]
    public void TestAStarMazeManhattanNoPathFound()
    {
        // arrange

        var maze  = CreateMaze();
        var start = (x: 9, y: 0);
        var end   = (x: 7, y: 9);

        // act

        var pathNotPossible = Search.AStar(start,
                                           pos => pos.x == end.x && pos.y == end.y,
                                           pos => MazeNeighbors4(maze, pos),
                                           (pos1, pos2) => 1,
                                           pos => MazeDistance(pos, end),
                                           1000);

        // assert

        pathNotPossible.Should().BeEmpty();
    }

    [Test]
    public void TestAStarMazeDiagNoPathFound()
    {
        // arrange

        var maze  = CreateMaze();
        var start = (x: 5, y: 5);
        var end   = (x: 7, y: 9);

        // act

        var pathNotPossible = Search.AStar(start,
                                           pos => pos.x == end.x && pos.y == end.y,
                                           pos => MazeNeighbors8(maze, pos),
                                           (pos1, pos2) => 1,
                                           pos => MazeDistanceDiag(pos, end),
                                           1000);

        // assert

        pathNotPossible.Should().BeEmpty();
    }

    #endregion

    // test data and helper functions

    private static IEnumerable<string> SurroundingCities(string city) => city switch {
        "Frankfurt"   => new[] { "Bad Vilbel", "Hanau", "Offenbach" },
        "Bad Vilbel"  => new[] { "Frankfurt", "Ilbenstadt" },
        "Hanau"       => new[] { "Offenbach", "Frankfurt", "Erlensee" },
        "Erlensee"    => new[] { "Hanau" },
        "Offenbach"   => new[] { "Hanau", "Frankfurt" },
        "Ilbenstadt"  => new[] { "Bad Vilbel", "Florstadt", "Friedberg" },
        "Florstadt"   => new[] { "Ilbenstadt" },
        "Friedberg"   => new[] { "Ilbenstadt" },
        "Köln"        => new[] { "Bonn" },
        "Bonn"        => new[] { "Köln" },
        _             => throw new InvalidOperationException(),
    };

    private static long DistanceByAir(string city, string finish) => (city, finish) switch
    {
        ("Köln", "Bonn")            => 25,
        ("Bonn", "Köln")            => 25,
        ("Köln", "Köln")            => 0,
        ("Bonn", "Bonn")            => 0,

        ("Florstadt", "Florstadt")  => 0,
        ("Friedberg", "Florstadt")  => 7,
        ("Ilbenstadt", "Florstadt") => 6,
        ("Bad Vilbel", "Florstadt") => 18,
        ("Erlensee", "Erlensee")    => 0,
        ("Erlensee", "Florstadt")   => 19,
        ("Hanau", "Florstadt")      => 21,
        ("Frankfurt", "Florstadt")  => 25,
        ("Offenbach", "Florstadt")  => 25,

        _                           => throw new InvalidOperationException("Aufruf der Testdaten mit unerwarteter Kombinaten der Städte"),
    };

    private static long DistanceByStreet(string a, string b) => (a, b) switch
    {
        ("Köln", "Köln")                => 0,
        ("Bonn", "Bonn")                => 0,
        ("Köln", "Bonn")                => 30,
        ("Bonn", "Köln")                => 30,

        ("Florstadt", "Florstadt")      => 0,
        ("Ilbenstadt", "Florstadt")     => 10,
        ("Florstadt", "Ilbenstadt")     => 10,
        ("Friedberg", "Ilbenstadt")     => 9,
        ("Ilbenstadt", "Friedberg")     => 9,
        ("Bad Vilbel", "Ilbenstadt")    => 15,
        ("Ilbenstadt", "Bad Vilbel")    => 15,
        ("Offenbach", "Hanau")          => 14,
        ("Offenbach", "Frankfurt")      => 8,
        ("Hanau", "Offenbach")          => 14,
        ("Frankfurt", "Offenbach")      => 8,
        ("Hanau", "Erlensee")           => 7,
        ("Erlensee", "Hanau")           => 7,
        ("Erlensee", "Erlensee")        => 0,
        ("Hanau", "Frankfurt")          => 22,
        ("Frankfurt", "Hanau")          => 22,
        ("Bad Vilbel", "Frankfurt")     => 9,
        ("Frankfurt", "Bad Vilbel")     => 9,

        _                               => throw new InvalidOperationException("Aufruf der Testdaten mit unerwarteter Kombinaten der Städte"),
    };

    private static HashSet<(int x, int y)> CreateMaze()
    {
        // create a maze where there is a path starting from top left to bottom right
        // depending if a 4-neighbor setup or a setup with 8 diagonal neighbors is used the paths differ
        //
        // for a negative test the bottom right position is not accessible from the spot at top right with
        // a 4-neighbor setup
        return new[] {
            "* ******  ",
            "* ** *   *",
            "*    **  *",
            "** **   **",
            "   ***** *",
            " ****  ** ",
            "    ***** ",
            "* *      *",
            "*   **** *",
            "******   *",
        }.SelectMany((l, y) => l.Select((c, x) => (c, x, y)))
         .Where(cxy => cxy.c == ' ')
         .Select(cxy => (cxy.x, cxy.y))
         .ToHashSet();
    }

    private static IEnumerable<(int x, int y)> MazeNeighbors4(HashSet<(int x, int y)> maze, (int x, int y) pos)
    {
        if(maze.Contains((pos.x - 1, pos.y)))
            yield return (pos.x - 1, pos.y);
        if(maze.Contains((pos.x + 1, pos.y)))
            yield return (pos.x + 1, pos.y);
        if(maze.Contains((pos.x, pos.y - 1)))
            yield return (pos.x, pos.y - 1);
        if(maze.Contains((pos.x, pos.y + 1)))
            yield return (pos.x, pos.y + 1);
    }

    private static IEnumerable<(int x, int y)> MazeNeighbors8(HashSet<(int x, int y)> maze, (int x, int y) pos)
    {
        if(maze.Contains((pos.x - 1, pos.y - 1)))
            yield return (pos.x - 1, pos.y - 1);
        if(maze.Contains((pos.x, pos.y - 1)))
            yield return (pos.x, pos.y - 1);
        if(maze.Contains((pos.x + 1, pos.y - 1)))
            yield return (pos.x + 1, pos.y - 1);

        if(maze.Contains((pos.x - 1, pos.y)))
            yield return (pos.x - 1, pos.y);
        if(maze.Contains((pos.x + 1, pos.y)))
            yield return (pos.x + 1, pos.y);

        if(maze.Contains((pos.x - 1, pos.y + 1)))
            yield return (pos.x - 1, pos.y + 1);
        if(maze.Contains((pos.x, pos.y + 1)))
            yield return (pos.x, pos.y + 1);
        if(maze.Contains((pos.x + 1, pos.y + 1)))
            yield return (pos.x + 1, pos.y + 1);
    }

    private static int MazeDistance((int x, int y) p1, (int x, int y) p2) => Abs(p1.x - p2.x) + Abs(p1.y - p2.y);

    private static int MazeDistanceDiag((int x, int y) p1, (int x, int y) p2) => Max(Abs(p1.x - p2.x), Abs(p1.y - p2.y));
}
