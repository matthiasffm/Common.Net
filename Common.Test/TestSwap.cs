using FluentAssertions;
using NUnit.Framework;

using static matthiasffm.Common.Algorithms.Basics;

namespace matthiasffm.Common.Test;

internal class TestSwap
{
    [Test]
    public void TestSwapByRef()
    {
        // arrange
        var a = 4;
        var b = 5;
        var s1 = "s1";
        var s2 = "s2";

        // act
        Swap(ref a, ref b);
        Swap(ref s1, ref s2);

        // assert
        a.Should().Be(5);
        b.Should().Be(4);
        s1.Should().Be("s2");
        s2.Should().Be("s1");
    }

    [Test]
    public void TestSwapListElems()
    {
        // arrange
        var list = new List<int>() { 1, 2, 3, 4, 5, 6 };

        // act
        list.Swap(2, 5);
        list.Swap(1, 2);

        // assert
        list.Should().BeEquivalentTo(new List<int>() { 5, 1, 3, 4, 2, 6 });
    }

    [Test]
    public void TestSwapListOutOfRange()
    {
        // arrange
        var list = new List<int>() { 1, 2, 3, 4, 5, 6 };

        // act
        var act1 = () => list.Swap(12, 5);
        var act2 = () => list.Swap(1, 12);

        // assert
        act1.Should().Throw<ArgumentOutOfRangeException>();
        act2.Should().Throw<ArgumentOutOfRangeException>();
    }
}
