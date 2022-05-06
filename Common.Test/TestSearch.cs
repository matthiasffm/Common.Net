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
        var edgesFromNidda      = Search.BreadthFirstEnumerate("Nidda", null, city => SurroundingCities(city));
        var edgesFromFrankfurt  = Search.BreadthFirstEnumerate("Frankfurt", null, city => SurroundingCities(city));

        // assert

        edgesFromKoeln.Should().Equal("Köln", "Bonn");
        edgesFromHanau.Should().Equal("Hanau", "Offenbach", "Frankfurt", "Erlensee", "Bad Vilbel", "Nidda", "Florstadt", "Friedberg");
        edgesFromNidda.Should().Equal("Nidda", "Bad Vilbel", "Florstadt", "Friedberg", "Frankfurt", "Hanau", "Offenbach", "Erlensee");
        edgesFromFrankfurt.Should().Equal("Frankfurt", "Bad Vilbel", "Hanau", "Offenbach", "Nidda", "Erlensee", "Florstadt", "Friedberg");
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
        pathFromHanauToFriedberg.Should().Equal("Hanau", "Frankfurt", "Bad Vilbel", "Nidda", "Friedberg");
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
        var edgesFromNidda      = Search.DepthFirstEnumerate("Nidda", null, city => SurroundingCities(city));
        var edgesFromFrankfurt  = Search.DepthFirstEnumerate("Frankfurt", null, city => SurroundingCities(city));

        // assert

        edgesFromKoeln.Should().Equal("Köln", "Bonn");
        edgesFromHanau.Should().Equal("Hanau", "Erlensee", "Frankfurt", "Bad Vilbel", "Nidda", "Friedberg", "Florstadt", "Offenbach");
        edgesFromNidda.Should().Equal("Nidda", "Friedberg", "Florstadt", "Bad Vilbel", "Frankfurt", "Offenbach", "Hanau", "Erlensee");
        edgesFromFrankfurt.Should().Equal("Frankfurt", "Offenbach", "Hanau", "Erlensee", "Bad Vilbel", "Nidda", "Friedberg", "Florstadt");
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
        pathFromHanauToFriedberg.Should().Equal("Hanau", "Frankfurt", "Bad Vilbel", "Nidda", "Friedberg");
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

        // assert
    }

    [Test]
    public void TestAStar()
    {
        // arrange

        // act

        // assert
    }

    #endregion

    // Testdaten

    private static IEnumerable<string> SurroundingCities(string city) => city switch {
        "Frankfurt"   => new[] { "Bad Vilbel", "Hanau", "Offenbach" },
        "Bad Vilbel"  => new[] { "Frankfurt", "Nidda" },
        "Hanau"       => new[] { "Offenbach", "Frankfurt", "Erlensee" },
        "Erlensee"    => new[] { "Hanau" },
        "Offenbach"   => new[] { "Hanau", "Frankfurt" },
        "Nidda"       => new[] { "Bad Vilbel", "Florstadt", "Friedberg" },
        "Florstadt"   => new[] { "Nidda" },
        "Friedberg"   => new[] { "Nidda" },
        "Köln"        => new[] { "Bonn" },
        "Bonn"        => new[] { "Köln" },
        _             => throw new InvalidOperationException(),
    };
}
