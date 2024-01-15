using NUnit.Framework;
using FluentAssertions;

using matthiasffm.Common.Algorithms;

namespace matthiasffm.Common.Test;

[TestFixture]
public partial class TestHeapSort
{
    [Test]
    public void TestSortEmptyCollection()
    {
        // arrange
        var unsorted = Array.Empty<int>();

        // act
        var sorted = HeapSort.Sort(unsorted).ToArray();

        // assert
        sorted.Length.Should().Be(0);
    }

    [Test]
    public void TestSortSingleElementCollection()
    {
        // arrange
        var unsorted = new[] { 1 };

        // act
        var sorted = HeapSort.Sort(unsorted).ToArray();

        // assert
        sorted.Should()
              .HaveCount(1)
              .And.Contain(1);
    }

    [Test]
    public void TestSortMultipleElementCollection()
    {
        // arrange
        var unsorted = new[] { 7, 11, 532, 3, 9, 1, 44 };

        // act
        var sorted = HeapSort.Sort(unsorted).ToArray();

        // assert
        sorted.Should()
              .HaveCount(7)
              .And.BeInAscendingOrder()
              .And.Contain(1)
              .And.Contain(3)
              .And.Contain(7)
              .And.Contain(9)
              .And.Contain(11)
              .And.Contain(44)
              .And.Contain(532);
    }

    [Test]
    public void TestSortBigCollection([Values(13, 101)] int size, [Random(1, 100, 5)] int seed)
    {
        var rand = new Random(seed);
        var toSort = Enumerable.Range(0, size).Select(r => rand.Next()).ToArray();

        var sorted = HeapSort.Sort(toSort).ToArray();

        sorted.Should()
              .NotBeEmpty()
              .And.HaveCount(toSort.Length)
              .And.BeInAscendingOrder()
              .And.Contain(toSort);
    }
}
