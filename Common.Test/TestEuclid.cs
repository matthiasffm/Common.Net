using FluentAssertions;
using NUnit.Framework;
using System.Numerics;

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

    [Test]
    public void TestEuclidWithBigInteger()
    {
        // arrange

        // 2 sehr große BigInteger:
        // 100F0E0D0C0B0A090807060504030201 == 21345817372864405881847059188222722561
        // 0102030405060708090A0B0C0D0E0F10 ==  1339673755198158349044581307228491536

        var bigA = new BigInteger(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
        var bigB = new BigInteger(new byte[] { 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 });

        // act

        var euclid0and1   = Euclid.Gcd(new BigInteger(0), new BigInteger(1));
        var euclidLarge   = Euclid.Gcd(new BigInteger(2855936073485L), new BigInteger(5739789L));
        var euclidExtLong = Euclid.GcdExt(new BigInteger(2855936073485L), new BigInteger(5739789L));
        var euclidXll     = Euclid.Gcd(bigA, bigB);
        var euclidExtXll  = Euclid.GcdExt(bigA, bigB);

        // assert

        euclid0and1.Should().Be(1);
        euclidLarge.Should().Be(11L);
        euclidExtLong.Should().Be((11L, 252127L, -125450359656L)); // 11 = 252127 * 2855936073485 - 125450359656 * 5739789

        // alles überprüft per Wolfram Alpha
        euclidXll.Should().Be(new BigInteger(17L));
        // == 17, 4925271158816758636193313629516529, -78477269753177962800908305839054382
        euclidExtXll.Should().Be((17L,
                                  new BigInteger(new byte[] { 0xF1, 0xB2, 0x75, 0x29, 0xCE, 0x63, 0xEA, 0x61, 0xCA, 0x23, 0x6E, 0xA9, 0xD5, 0xF2, 0x00 }),
                                  new BigInteger(new byte[] { 0xD2, 0xA1, 0x65, 0x19, 0xBE, 0x53, 0xDA, 0x51, 0xBA, 0x13, 0x5E, 0x99, 0xC5, 0xE2, 0xF0 })));
    }
}
