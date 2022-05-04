using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common.Math;

namespace matthiasffm.Common.Test;

internal class TestEuclid
{
    [Test]
    public void TestGcd()
    {
        // arrange

        // act
        var euclid0and1         = Euclid.Gcd(0, 1);
        var euclid1and0         = Euclid.Gcd(1, 0);
        var euclid1and1         = Euclid.Gcd(1, 1);
        var euclid3and71        = Euclid.Gcd(3, 71);
        var euclid2and8         = Euclid.Gcd(2, 8);
        var euclid12and128      = Euclid.Gcd(12, 128);
        var euclid1071and462    = Euclid.Gcd(1071, 462);
        var euclidNegA          = Euclid.Gcd(-1071, 462);
        var euclidNegB          = Euclid.Gcd(1071, -462);
        var euclidNegBoth       = Euclid.Gcd(-1071, -462);
        var euclidLarge         = Euclid.Gcd(2855936073485L, 5739789L);

        // assert
        euclid0and1.Should().Be(1);
        euclid1and0.Should().Be(1);
        euclid1and1.Should().Be(1);
        euclid3and71.Should().Be(1);
        euclid2and8.Should().Be(2);
        euclid12and128.Should().Be(4);
        euclid1071and462.Should().Be(21);
        euclidNegA.Should().Be(21);
        euclidNegB.Should().Be(21);
        euclidNegBoth.Should().Be(21);
        euclidLarge.Should().Be(11L);
    }

    [Test]
    public void TestGcdMultiple()
    {
        // arrange

        // act
        var euclidInt1 = Euclid.Gcd(-1071, 462, -42, 21, 147);
        var euclidInt2 = new[] { 7, 3, 21, 5375}.Gcd();
        var euclidLongs1 = Euclid.Gcd(2855936073485L, 5739789L, -132L);
        var euclidLongs2 = new[] { -23L, 391L, 2737L}.Gcd();

        // assert
        euclidInt1.Should().Be(21);
        euclidInt2.Should().Be(1);
        euclidLongs1.Should().Be(11L);
        euclidLongs2.Should().Be(23L);
    }

    [Test]
    public void TestGcdExt()
    {
        // arrange

        // act
        var euclidExtInt  = Euclid.GcdExt(240, 46);
        var euclidExtLong = Euclid.GcdExt(2855936073485L, 5739789L);

        // assert
        euclidExtInt.Should().Be((2, -9, 47));                     // 2 = -9 * 240 + 47 * 46
        euclidExtLong.Should().Be((11L, 252127L, -125450359656L)); // 11 = 252127 * 2855936073485 - 125450359656 * 5739789
    }
}
