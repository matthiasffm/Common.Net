using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common.Math;

namespace matthiasffm.Common.Test;

internal class TestMath
{
    [Test]
    public void TestSumNumbers()
    {
        // arrange

        // act
        var sum0 = MathExtensions.SumNumbers(0);
        var sum1 = MathExtensions.SumNumbers(1);
        var sum2 = MathExtensions.SumNumbers(2);
        var sum5 = MathExtensions.SumNumbers(5);
        var sum9 = MathExtensions.SumNumbers(9);

        // assert
        sum0.Should().Be(0);
        sum1.Should().Be(1);
        sum2.Should().Be(1 + 2);
        sum5.Should().Be(1 + 2 + 3 + 4 + 5);
        sum9.Should().Be(1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9);
    }

    [Test]
    public void TestBetween()
    {
        // arrange

        // act
        var between0 = 1.Between(0, 2);
        var between1 = (-1).Between(-2, 0);
        var between2 = 1.Between(1, 2);
        var between3 = 3.Between(0, 2);
        var between4 = 0f.Between(-0.001f, 0.0001f);
        var between5 = 0f.Between(0.001f, 0.0003f);

        // assert
        between0.Should().Be(true);
        between1.Should().Be(true);
        between2.Should().Be(true);
        between3.Should().Be(false);
        between4.Should().Be(true);
        between5.Should().Be(false);
    }

    [Test]
    public void TestAbs()
    {
        // arrange

        // act
        var abs1 = 1.Abs();
        var abs2 = (-1).Abs();
        var abs3 = 0.Abs();
        var abs4 = 3.6f.Abs();
        var abs5 = (-0.6f).Abs();

        // assert
        abs1.Should().Be(1);
        abs2.Should().Be(1);
        abs3.Should().Be(0);
        abs4.Should().Be(3.6f);
        abs5.Should().Be(0.6f);
    }

    [Test]
    public void TestClamp()
    {
        // arrange

        // act
        var clamp1      = 1.Clamp(5);
        var clamp2      = 7.Clamp(2);
        var clamp3      = (-1).Clamp(-5);
        var clamp4      = (-0.876f).Clamp(-0.781f);
        var clampRange1 = 2.Clamp(1, 11);
        var clampRange2 = 2.Clamp(4, 11);
        var clampRange3 = (-0.324f).Clamp(-32.1f, -0.23f);
        var clampRange4 = 13.43f.Clamp(12.213f, 23.3f);
        var clampRange5 = 23.43f.Clamp(12.213f, 23.3f);

        // assert
        clamp1.Should().Be(5);
        clamp2.Should().Be(7);
        clamp3.Should().Be(-1);
        clamp4.Should().Be(-0.781f);
        clampRange1.Should().Be(2);
        clampRange2.Should().Be(4);
        clampRange3.Should().Be(-0.324f);
        clampRange4.Should().Be(13.43f);
        clampRange5.Should().Be(23.3f);
    }
}
