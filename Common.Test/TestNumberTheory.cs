using FluentAssertions;
using NUnit.Framework;

using System.Numerics;
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

        var chinRemaindersAocSample1 = NumberTheory.CalcSimultaneousCongruences((7, 7), (12, 13), (55, 59), (25, 31), (12, 19));
        var chinRemaindersAocSample2 = NumberTheory.CalcSimultaneousCongruences((17, 17), (11, 13), (16, 19));
        var chinRemaindersAocSample3 = NumberTheory.CalcSimultaneousCongruences((67, 67), (6, 7), (57, 59), (58, 61));
        var chinRemaindersAocSample4 = NumberTheory.CalcSimultaneousCongruences((67, 67), (5, 7), (56, 59), (57, 61));
        var chinRemaindersAocSample5 = NumberTheory.CalcSimultaneousCongruences((67, 67), (6, 7), (56, 59), (57, 61));
        var chinRemaindersAocSample6 = NumberTheory.CalcSimultaneousCongruences((1789, 1789), (36, 37), (45, 47), (1886, 1889));

        // assert

        chinRemainders1.Should().Be((47, 60));
        chinRemaindersClassic.Should().Be((119, 420));
        chinRemaindersClassicRest1.Should().Be((301, 420));
        chinRemaindersClassicBrahmagupta.Should().Be((59, 60));

        chinRemaindersAocSample1.Should().Be((1068781L, 3162341L));
        chinRemaindersAocSample2.Should().Be((3417L, 4199L));
        chinRemaindersAocSample3.Should().Be((754018L, 1687931L));
        chinRemaindersAocSample4.Should().Be((779210L, 1687931L));
        chinRemaindersAocSample5.Should().Be((1261476L, 1687931L));
        chinRemaindersAocSample6.Should().Be((1202161486L, 5876813119L));
    }

    [Test]
    public void TestChineseRemainderBigInt()
    {
        // arrange

        var input = new[] {
            (new BigInteger(13),    new BigInteger(13)),
            (new BigInteger(30),    new BigInteger(37)),
            (new BigInteger(448),   new BigInteger(461)),
            (new BigInteger(7),     new BigInteger(17)),
            (new BigInteger(6),     new BigInteger(19)),
            (new BigInteger(16),    new BigInteger(29)),
            (new BigInteger(695),   new BigInteger(739)),
            (new BigInteger(28),    new BigInteger(41)),
            (new BigInteger(2),     new BigInteger(23)),
        };

        // act

        var chinRemaindersAoc1 = NumberTheory.CalcSimultaneousCongruences(input.Take(2));
        var chinRemaindersAoc2 = NumberTheory.CalcSimultaneousCongruences(input.Take(3));
        var chinRemaindersAoc3 = NumberTheory.CalcSimultaneousCongruences(input.Take(4));
        var chinRemaindersAoc4 = NumberTheory.CalcSimultaneousCongruences(input.Take(5));
        var chinRemaindersAoc5 = NumberTheory.CalcSimultaneousCongruences(input.Take(6));
        var chinRemaindersAoc6 = NumberTheory.CalcSimultaneousCongruences(input.Take(7));
        var chinRemaindersAoc7 = NumberTheory.CalcSimultaneousCongruences(input.Take(8));
        var chinRemaindersAoc8 = NumberTheory.CalcSimultaneousCongruences(input.Take(9));

        // assert

        chinRemaindersAoc1.Should().Be((new BigInteger(104), new BigInteger(481)));
        chinRemaindersAoc2.Should().Be((new BigInteger(185770), new BigInteger(221741)));
        chinRemaindersAoc3.Should().Be((new BigInteger(850993), new BigInteger(3769597)));
        chinRemaindersAoc4.Should().Be((new BigInteger(19698978), new BigInteger(71622343)));
        chinRemaindersAoc5.Should().Be((new BigInteger(1165656466), new BigInteger(2077047947)));
        chinRemaindersAoc6.Should().Be((new BigInteger(34398423618), new BigInteger(1534938432833)));    // ab hier overflow mit long
        chinRemaindersAoc7.Should().Be((new BigInteger(49152428274274), new BigInteger(62932475746153)));
        chinRemaindersAoc8.Should().Be((new BigInteger(552612234243498), new BigInteger(1447446942161519)));
    }
}
