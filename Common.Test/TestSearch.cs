using FluentAssertions;
using NUnit.Framework;

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

        var goalNotFound = Search.AStar(Array.Empty<string>(),
                                        "Start",
                                        "Ziel",
                                        (city) => Array.Empty<string>(),
                                        (a, b) => 10L,
                                        (city) => 10L,
                                        1000L);

        // assert

        goalNotFound.Should().BeEmpty();
    }

    [Test]
    public void TestAStar()
    {
        // arrange

        var cities = new[] { "Frankfurt", "Bad Vilbel", "Hanau", "Offenbach", "Erlensee", "Ilbenstadt", "Florstadt", "Friedberg", "Köln", "Bonn" };

        // act

        var pathFromKoelnToBonn = Search.AStar(cities,
                                               "Köln",
                                               "Bonn",
                                               (city) => SurroundingCities(city),
                                               (a, b) => DistanceByStreet(a, b),
                                               (city) => DistanceByAir(city, "Bonn"),
                                               1000L);

        var pathFromErlenseeToFlorstadt = Search.AStar(cities,
                                                       "Erlensee",
                                                       "Florstadt",
                                                       (city) => SurroundingCities(city),
                                                       (a, b) => DistanceByStreet(a, b),
                                                       (city) => DistanceByAir(city, "Florstadt"),
                                                       1000L);

        // assert

        pathFromKoelnToBonn.Should().Equal("Köln", "Bonn");
        pathFromErlenseeToFlorstadt.Should().Equal("Erlensee", "Hanau", "Frankfurt", "Bad Vilbel", "Ilbenstadt", "Florstadt");
    }

    #endregion

    // Testdaten

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
}
