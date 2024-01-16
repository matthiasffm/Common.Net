using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common.Text;

namespace matthiasffm.Common.Test;

public class TestStringExtensions
{
    [Test]
    public void TestRemoveAccents()
    {
        "dé".RemoveAccents().Should().Be("de");
        "Dončić".RemoveAccents().Should().Be("Doncic");
        "test".RemoveAccents().Should().Be("test");
        "".RemoveAccents().Should().Be("");
    }

    [Test]
    public void TestRemovePunctuation()
    {
        "d'aaron".RemovePunctuation().Should().Be("daaron");
        "t.j.".RemovePunctuation().Should().Be("tj");
    }

    [Test]
    public void TestLevenshteinWithEmptyStrings()
    {
        "".Levenshtein("").Should().Be(0);
        "kitten".Levenshtein("").Should().Be(6);
        "".Levenshtein("sitting").Should().Be(7);
    }

    [Test]
    public void TestLevenshtein()
    {
        "CA".Levenshtein("ABC").Should().Be(3);
        "ABC".Levenshtein("CA").Should().Be(3);

        // replace characters
        "kitten".Levenshtein("kitten").Should().Be(0);
        "kitten".Levenshtein("sitten").Should().Be(1);
        "kitten".Levenshtein("sitting").Should().Be(3);
        "sitting".Levenshtein("kitten").Should().Be(3);

        // insert/delete characters
        "sunday".Levenshtein("saturday").Should().Be(3);
        "saturday".Levenshtein("sunday").Should().Be(3);
        "flaw".Levenshtein("lawn").Should().Be(2);
        "lawn".Levenshtein("flaw").Should().Be(2);
        "embarking".Levenshtein("dark").Should().Be(6);
        "dark".Levenshtein("embarking").Should().Be(6);
        "execution".Levenshtein("intention").Should().Be(5);
        "intention".Levenshtein("execution").Should().Be(5);

        // transpose characters
        "computer".Levenshtein("comptuer").Should().Be(2);
        "comptuer".Levenshtein("computer").Should().Be(2);
    }

    [Test]
    public void TestOsaWithEmptyStrings()
    {
        "".OptimalStringAlignment("").Should().Be(0);
        "".OptimalStringAlignment("ABC").Should().Be(3);
        "CA".OptimalStringAlignment("").Should().Be(2);
    }

    [Test]
    public void TestOsa()
    {
        "CA".OptimalStringAlignment("ABC").Should().Be(3);

        // replace characters
        "kitten".OptimalStringAlignment("kitten").Should().Be(0);
        "kitten".OptimalStringAlignment("sitten").Should().Be(1);
        "sitten".OptimalStringAlignment("kitten").Should().Be(1);
        "kitten".OptimalStringAlignment("sitting").Should().Be(3);
        "sitting".OptimalStringAlignment("kitten").Should().Be(3);

        // insert/delete characters
        "sunday".OptimalStringAlignment("saturday").Should().Be(3);
        "saturday".OptimalStringAlignment("sunday").Should().Be(3);
        "flaw".OptimalStringAlignment("lawn").Should().Be(2);
        "lawn".OptimalStringAlignment("flaw").Should().Be(2);
        "embarking".OptimalStringAlignment("dark").Should().Be(6);
        "dark".OptimalStringAlignment("embarking").Should().Be(6);

        // transpose characters
        "computer".OptimalStringAlignment("comptuer").Should().Be(1);
        "comptuer".OptimalStringAlignment("computer").Should().Be(1);
    }
}
