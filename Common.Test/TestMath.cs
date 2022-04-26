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
}
