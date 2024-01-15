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
        var gcd0and1         = Euclid.Gcd(0, 1);
        var gcd1and0         = Euclid.Gcd(1, 0);
        var gcd1and1         = Euclid.Gcd(1, 1);
        var gcd3and71        = Euclid.Gcd(3, 71);
        var gcd2and8         = Euclid.Gcd(2, 8);
        var gcd12and128      = Euclid.Gcd(12, 128);
        var gcd1071and462    = Euclid.Gcd(1071, 462);
        var gcdNegA          = Euclid.Gcd(-1071, 462);
        var gcdNegB          = Euclid.Gcd(1071, -462);
        var gcdNegBoth       = Euclid.Gcd(-1071, -462);
        var gcdLarge         = Euclid.Gcd(2855936073485L, 5739789L);

        // assert
        gcd0and1.Should().Be(1);
        gcd1and0.Should().Be(1);
        gcd1and1.Should().Be(1);
        gcd3and71.Should().Be(1);
        gcd2and8.Should().Be(2);
        gcd12and128.Should().Be(4);
        gcd1071and462.Should().Be(21);
        gcdNegA.Should().Be(21);
        gcdNegB.Should().Be(21);
        gcdNegBoth.Should().Be(21);
        gcdLarge.Should().Be(11L);
    }

    [Test]
    public void TestGcdMultiple()
    {
        // arrange

        // act
        var gcdInt1 = Euclid.Gcd(-1071, 462, -42, 21, 147);
        var gcdInt2 = new[] { 7, 3, 21, 5375}.Gcd();
        var gcdLongs1 = Euclid.Gcd(2855936073485L, 5739789L, -132L);
        var gcdLongs2 = new[] { -23L, 391L, 2737L}.Gcd();

        // assert
        gcdInt1.Should().Be(21);
        gcdInt2.Should().Be(1);
        gcdLongs1.Should().Be(11L);
        gcdLongs2.Should().Be(23L);
    }

    [Test]
    public void TestGcdExt()
    {
        // arrange

        // act
        var gcdExtInt  = Euclid.GcdExt(240, 46);
        var gcdExtLong = Euclid.GcdExt(2855936073485L, 5739789L);

        // assert
        gcdExtInt.Should().Be((2, -9, 47));                     // 2 = -9 * 240 + 47 * 46
        gcdExtLong.Should().Be((11L, 252127L, -125450359656L)); // 11 = 252127 * 2855936073485 - 125450359656 * 5739789
    }

    [Test]
    public void TestGcdWithBigInteger()
    {
        // arrange

        // test this with two very big integers:
        // 100F0E0D0C0B0A090807060504030201 == 21345817372864405881847059188222722561
        // 0102030405060708090A0B0C0D0E0F10 ==  1339673755198158349044581307228491536

        var bigA = new BigInteger(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
        var bigB = new BigInteger(new byte[] { 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 });

        // act

        var gcd0and1   = Euclid.Gcd(new BigInteger(0), new BigInteger(1));
        var gcdLarge   = Euclid.Gcd(new BigInteger(2855936073485L), new BigInteger(5739789L));
        var gcdExtLong = Euclid.GcdExt(new BigInteger(2855936073485L), new BigInteger(5739789L));
        var gcdXll     = Euclid.Gcd(bigA, bigB);
        var gcdExtXll  = Euclid.GcdExt(bigA, bigB);

        // assert

        gcd0and1.Should().Be(1);
        gcdLarge.Should().Be(11L);
        gcdExtLong.Should().Be((11L, 252127L, -125450359656L)); // 11 = 252127 * 2855936073485 - 125450359656 * 5739789

        // results checked with wolfram alpha
        gcdXll.Should().Be(new BigInteger(17L));
        // == 17, 4925271158816758636193313629516529, -78477269753177962800908305839054382
        gcdExtXll.Should().Be((17L,
                                  new BigInteger(new byte[] { 0xF1, 0xB2, 0x75, 0x29, 0xCE, 0x63, 0xEA, 0x61, 0xCA, 0x23, 0x6E, 0xA9, 0xD5, 0xF2, 0x00 }),
                                  new BigInteger(new byte[] { 0xD2, 0xA1, 0x65, 0x19, 0xBE, 0x53, 0xDA, 0x51, 0xBA, 0x13, 0x5E, 0x99, 0xC5, 0xE2, 0xF0 })));
    }

    [Test]
    public void TestLcm()
    {
        // arrange

        // act
        var lcm1and1        = Euclid.Lcm(1, 1);
        var lcm3and7        = Euclid.Lcm(3, 7);
        var lcm2and8        = Euclid.Lcm(2, 8);
        var lcm12and128     = Euclid.Lcm(12, 128);
        var lcm1071and462   = Euclid.Lcm(1071, 462);

        // assert
        lcm1and1.Should().Be(1);
        lcm3and7.Should().Be(21);
        lcm2and8.Should().Be(8);
        lcm12and128.Should().Be(384);
        lcm1071and462.Should().Be(23562);
    }

    [Test]
    public void TestLcmMultiple()
    {
        // arrange

        // act
        var lcmInt1     = Euclid.Lcm(2, 3, 7, 5, 4);
        var lcmInt2     = new[] { 7, 3, 12, 573 }.Lcm();
        var lcmLongs1   = Euclid.Lcm(285593607L, 5739789L, 132L);
        var lcmLongs2   = new[] { 23L, 391L, 2737L }.Lcm();

        // assert
        lcmInt1.Should().Be(420);
        lcmInt2.Should().Be(16044);
        lcmLongs1.Should().Be(2185662725238564L);
        lcmLongs2.Should().Be(2737L);
    }
}
