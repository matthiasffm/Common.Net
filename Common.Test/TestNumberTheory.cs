using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common.Math;

namespace matthiasffm.Common.Test;

internal class TestNumberTheory
{
    [Test]
    public void TestChineseRemainder()
    {
        // arrange

        // act
        var chinRemainders1                     = NumberTheory.CalcSimultaneousCongruences((2, 3), (3, 4), (2, 5));
        var chinRemaindersClassic               = NumberTheory.CalcSimultaneousCongruences((1, 2), (2, 3), (3, 4), (4, 5), (5, 6), (0, 7));
        var chinRemaindersClassicRest1          = NumberTheory.CalcSimultaneousCongruences((1, 2), (1, 3), (1, 4), (1, 5), (1, 6), (0, 7));
        var chinRemaindersClassicBrahmagupta    = NumberTheory.CalcSimultaneousCongruences((2, 3), (3, 4), (4, 5), (5, 6));

        // assert
        chinRemainders1.Should().Be((47, 60));
        chinRemaindersClassic.Should().Be((119, 420));
        chinRemaindersClassicRest1.Should().Be((301, 420));
        chinRemaindersClassicBrahmagupta.Should().Be((59, 60));
    }
}
