using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common.Algorithms;

namespace matthiasffm.Common.Test;

internal class TestSearch
{
    [Test]
    public void TestBreadthFirstSearchEmpty()
    {
        // arrange

        var cities = new[] { "Frankfurt", "Bad Vilbel", "Hanau", "Offenbach", "Nidda", "Florstadt", "Friedberg", "Köln", "Bonn" };

        // act

        var pathNotFound = cities.BreadthFirstSearch("Hanau", "Bonn", (map, city) => SurroundingCities(city));

        // assert

        pathNotFound.Should().BeEmpty();
    }

    [Test]
    public void TestBreadthFirstSearch()
    {
        // arrange

        var cities = new[] { "Frankfurt", "Bad Vilbel", "Hanau", "Offenbach", "Nidda", "Florstadt", "Friedberg" };

        // act

        var pathFromKoelnToBonn          = cities.BreadthFirstSearch("Köln", "Bonn", (map, city) => SurroundingCities(city));
        var pathFromHanauToFriedberg     = cities.BreadthFirstSearch("Hanau", "Friedberg", (map, city) => SurroundingCities(city));
        var pathFromErlenseeToOffenbach  = cities.BreadthFirstSearch("Erlensee", "Offenbach", (map, city) => SurroundingCities(city));
        var pathFromFrankfurtToFrankfurt = cities.BreadthFirstSearch("Frankfurt", "Frankfurt", (map, city) => SurroundingCities(city));

        // assert

        pathFromKoelnToBonn.Should().BeEquivalentTo(new[] { "Köln", "Bonn" });
        pathFromHanauToFriedberg.Should().BeEquivalentTo(new[] { "Hanau", "Frankfurt", "Bad Vilbel", "Nidda", "Friedberg" });
        pathFromErlenseeToOffenbach.Should().BeEquivalentTo(new[] { "Erlensee", "Hanau", "Offenbach" });
        pathFromFrankfurtToFrankfurt.Should().BeEquivalentTo(new[] { "Frankfurt" });
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
