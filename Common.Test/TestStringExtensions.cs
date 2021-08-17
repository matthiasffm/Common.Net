using FluentAssertions;
using NUnit.Framework;

namespace Common.Test;

public class TestStringExtensions
{
    [Test]
    public void TestLevenshtein()
    {
        "kitten".Levenshtein("").Should().Be(6);
        "".Levenshtein("sitting").Should().Be(7);
        "kitten".Levenshtein("kitten").Should().Be(0);
        "kitten".Levenshtein("sitten").Should().Be(1);
        "kitten".Levenshtein("sitting").Should().Be(3);
        "flaw".Levenshtein("lawn").Should().Be(2);
    }

    [Test]
    public void TestOsa()
    {
        "CA".OptimalStringAlignemnt("ABC").Should().Be(3);
        "".OptimalStringAlignemnt("ABC").Should().Be(3);
        "CA".OptimalStringAlignemnt("").Should().Be(2);
    }
}
