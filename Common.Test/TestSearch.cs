using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common.Algorithms;

namespace matthiasffm.Common.Test;

internal class TestSearch
{
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
