using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common.Text;

namespace matthiasffm.Common.Test;

public class TestStringExtensions
{
    /// <summary>
    /// Prüft das Entfernen von Akzenten
    /// </summary>
    [Test]
    public void TestRemoveAccents()
    {
        "dé".RemoveAccents().Should().Be("de");
        "Dončić".RemoveAccents().Should().Be("Doncic");
        "test".RemoveAccents().Should().Be("test");
        "".RemoveAccents().Should().Be("");
    }

    /// <summary>
    /// Prüft das Entfernen von Punkt- und Stric
    /// </summary>
    [Test]
    public void TestRemovePunctuation()
    {
        "d'aaron".RemovePunctuation().Should().Be("daaron");
        "t.j.".RemovePunctuation().Should().Be("tj");
    }

    /// <summary>
    /// Prüft die Levenshtein-Entferung zwischen 2 Zeichenketten
    /// </summary>
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
        "CA".OptimalStringAlignment("ABC").Should().Be(3);
        "".OptimalStringAlignment("ABC").Should().Be(3);
        "CA".OptimalStringAlignment("").Should().Be(2);
    }
}
