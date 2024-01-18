using NUnit.Framework;
using FluentAssertions;

using matthiasffm.Common.Collections;

namespace matthiasffm.Common.Test;

[TestFixture]
public partial class TestBinaryHeap
{
    [Test]
    public void TestCreateHeapWithNegativeCapacity()
    {
        // arrange

        // act
        var tryCreate0 = () => new BinaryHeap<int>(0);
        var tryCreateMinus = () => new BinaryHeap<int>(-4);

        // assert
        tryCreate0.Should().Throw<ArgumentOutOfRangeException>();
        tryCreateMinus.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void TestEmptyHeap()
    {
        // arrange
        var heap = new BinaryHeap<int>();

        // act
        var count = heap.Count;
        var contains1 = heap.Contains(1, out _);
        var min = () => heap.Min;
        var extract = () => heap.ExtractMin();
        var tryExtract = heap.TryExtractMin(out var tryMin);

        // assert
        count.Should().Be(0);
        contains1.Should().BeFalse();
        min.Should().Throw<InvalidOperationException>();
        extract.Should().Throw<InvalidOperationException>();
        tryExtract.Should().BeFalse();
    }

    [Test]
    public void TestCountSingleElementHeap()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 1 });

        // act
        var count = heap.Count;

        // assert
        count.Should().Be(1);
    }

    [Test]
    public void TestCountMultipleElementHeap([Values(new int[] { 3, 5, 1, 4, 2 }, new int[] { 1, 2, 3, 2, 5, 7 })] int[] elements)
    {
        // arrange
        var heap = new BinaryHeap<int>(elements);

        // act
        var count = heap.Count;

        // assert
        count.Should().Be(elements.Length);
    }

    [Test]
    public void TestContainsSingleElementHeap()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 1 });

        // act
        var contains1 = heap.Contains(1, out var dummy);
        var contains0 = heap.Contains(0, out _);

        // assert
        contains1.Should().BeTrue();
        dummy.Should().NotBeNull();
        contains0.Should().BeFalse();
    }

    [Test]
    public void TestContainsMultipleElementHeap()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 3, 5, 1, 4, 2 });

        // act
        var contains1 = heap.Contains(1, out var dummy1);
        var contains2 = heap.Contains(2, out var dummy2);
        var contains3 = heap.Contains(3, out var dummy3);
        var contains4 = heap.Contains(4, out var dummy4);
        var contains5 = heap.Contains(5, out var dummy5);
        var contains6 = heap.Contains(6, out _);

        // assert
        contains1.Should().BeTrue();
        dummy1.Should().NotBeNull();
        contains2.Should().BeTrue();
        dummy2.Should().NotBeNull();
        contains3.Should().BeTrue();
        dummy3.Should().NotBeNull();
        contains4.Should().BeTrue();
        dummy4.Should().NotBeNull();
        contains5.Should().BeTrue();
        dummy5.Should().NotBeNull();
        contains6.Should().BeFalse();
    }

    [Test]
    public void TestMinSingleElementHeap()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 1 });

        // act
        var min = heap.Min;

        // assert
        min.Should().Be(1);
    }

    [Test]
    public void TestMinMultipleElementHeap([Values(new int[] { 3, 5, 1, 4, 2 }, new int[] { 1, 2, 3, 2, 5, 7 })] int[] elements)
    {
        // arrange
        var heap = new BinaryHeap<int>(elements);

        // act
        var min = heap.Min;

        // assert
        min.Should().Be(elements.Min());
    }

    [Test]
    public void TestExtractMinSingleElementHeap()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 4 });

        // act
        var initialCount = heap.Count;
        var min = heap.Min;
        var minExtracted = heap.ExtractMin();

        // assert
        min.Should().Be(minExtracted);
        min.Should().Be(4);
        initialCount.Should().Be(1);
        heap.Count.Should().Be(0);
    }

    [Test]
    public void TestExtractMinMultipleElementHeap()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 3, 5, 1, 4, 2 });
        // heap tree:
        //             1
        //          2     3
        //         4 5      

        // act
        var initialCount = heap.Count;
        var min1 = heap.Min;
        var minExtracted1 = heap.ExtractMin();
        var min2 = heap.Min;
        var minExtracted2 = heap.ExtractMin();
        var min3 = heap.Min;
        var minExtracted3 = heap.ExtractMin();
        var min4 = heap.Min;
        var minExtracted4 = heap.ExtractMin();
        var min5 = heap.Min;
        var minExtracted5 = heap.ExtractMin();
        var count = heap.Count;

        // assert
        min1.Should().Be(minExtracted1);
        min1.Should().Be(1);
        min2.Should().Be(minExtracted2);
        min2.Should().Be(2);
        min3.Should().Be(minExtracted3);
        min3.Should().Be(3);
        min4.Should().Be(minExtracted4);
        min4.Should().Be(4);
        min5.Should().Be(minExtracted5);
        min5.Should().Be(5);
        initialCount.Should().Be(5);
        count.Should().Be(0);
    }

    [Test]
    public void TestTryExtractMin()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 1, 3, 1 });

        // act
        var initialCount = heap.Count;
        var tryExtract1 = heap.TryExtractMin(out var min1);
        var tryExtract2 = heap.TryExtractMin(out var min2);
        var tryExtract3 = heap.TryExtractMin(out var min3);
        var tryExtract4 = heap.TryExtractMin(out var min4);
        var count = heap.Count;

        // assert
        initialCount.Should().Be(3);
        tryExtract1.Should().BeTrue();
        tryExtract2.Should().BeTrue();
        tryExtract3.Should().BeTrue();
        tryExtract4.Should().BeFalse();
        min1.Should().Be(1);
        min2.Should().Be(1);
        min3.Should().Be(3);
        min4.Should().Be(default);
        count.Should().Be(0);
    }

    [Test]
    public void TestDecreaseElementWithIllegalHandleFails()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 1, 2, 4, 3 });
        heap.Contains(1, out var handle1);

        // act, decrease with handle not in heap fails
        heap.ExtractMin();
        var handleNull      = () => heap.DecreaseElement(null, 7);
        var handleNotInHeap = () => heap.DecreaseElement(handle1, 7);

        // assert
        handleNull.Should().ThrowExactly<ArgumentOutOfRangeException>("pos");
        handleNotInHeap.Should().ThrowExactly<ArgumentOutOfRangeException>("pos");
    }

    [Test]
    public void TestDecreaseElementToGreaterValueFails()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 1, 2, 4, 3 });
        heap.Contains(3, out var handle3);

        // act, decrease with higher value fails
        var valueHigher = () => heap.DecreaseElement(handle3, 7);

        // assert
        valueHigher.Should().ThrowExactly<ArgumentOutOfRangeException>("newElement");
    }

    [Test]
    public void TestDecreaseSingleElementHeap()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 4 });
        heap.Contains(4, out var handle4);

        // act, decrease single item 4 to 2
        var initialCount = heap.Count;
        var initialMin = heap.Min;
        heap.DecreaseElement(handle4, 2);
        var newMin = heap.Min;
        var newCount = heap.Count;

        // assert
        initialCount.Should().Be(1);
        initialMin.Should().Be(4);
        newMin.Should().Be(2);
        newCount.Should().Be(1);
    }

    [Test]
    public void TestDecreaseMultiElementHeap()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 4, 2, 3, 5, 7, 9, 1 });
        // heap tree:
        //             1
        //          2     3
        //         5 7   9 4
        heap.Contains(4, out var handle4);
        heap.Contains(3, out var handle3);

        // act
        var initialCount = heap.Count;
        var initialMin = heap.Min;
        heap.DecreaseElement(handle4, 2);
        heap.DecreaseElement(handle3, 1);
        var newMin = heap.Min;
        var newCount = heap.Count;
        var contains4AfterDecrease = heap.Contains(4, out _);
        var contains3AfterDecrease = heap.Contains(3, out _);

        // assert
        // heap tree:
        //             1
        //          2     1
        //         5 7   9 2
        initialCount.Should().Be(7);
        initialMin.Should().Be(1);
        newMin.Should().Be(1);
        newCount.Should().Be(7);
        contains4AfterDecrease.Should().BeFalse();
        contains3AfterDecrease.Should().BeFalse();
    }

    [Test]
    public void TestHandlePersistence()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 4, 2, 3, 5, 7, 9, 1 });
        // heap tree:
        //             1
        //          2     3
        //         5 7   9 4
        heap.Contains(4, out var handle4);
        heap.Contains(3, out var handle3);

        // act
        heap.ExtractMin();
        heap.ExtractMin();
        //             3
        //           5   4
        //          7 9
        var contains4AfterExtractMin = heap.Contains(4, out var handle4AfterExtractMin);
        var contains3AfterExtractMin = heap.Contains(3, out var handle3AfterExtractMin);

        // assert
        handle4AfterExtractMin.Should().Be(handle4);
        contains4AfterExtractMin.Should().BeTrue();
        handle3AfterExtractMin.Should().Be(handle3);
        contains3AfterExtractMin.Should().BeTrue();
    }

    [Test]
    public void TestDecreaseToNewMin()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 4, 2, 3, 5, 7, 9, 2 });
        // heap tree:
        //             2
        //          4     2
        //         5 7   9 3
        heap.Contains(4, out var handle4);

        // act
        var initialCount = heap.Count;
        var initialMin = heap.Min;
        heap.DecreaseElement(handle4, 1);
        var newMin = heap.Min;
        var newCount = heap.Count;

        // assert
        // heap tree:
        //             1
        //          2     2
        //         5 7   9 3
        initialCount.Should().Be(7);
        initialMin.Should().Be(2);
        newMin.Should().Be(1);
        newCount.Should().Be(7);
    }

    [Test]
    public void TestDecreaseExistingMin()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 6, 2, 3, 5, 4, 6 });
        // heap tree:
        //             2
        //          4     3
        //         5 6   6
        heap.Contains(2, out var handle2);

        // act
        var initialCount = heap.Count;
        var initialMin = heap.Min;
        heap.DecreaseElement(handle2, 1);
        var newMin = heap.Min;
        var newCount = heap.Count;

        // assert
        // heap tree:
        //             1
        //          4      3
        //         5 6    6
        initialCount.Should().Be(6);
        initialMin.Should().Be(2);
        newMin.Should().Be(1);
        newCount.Should().Be(6);
    }

    [Test]
    public void TestInsertOnEmptyHeap()
    {
        // arrange
        var heap = new BinaryHeap<int>(10);

        // act
        var initialCount = heap.Count;
        var handle3 = heap.Insert(3);
        var handle4 = heap.Insert(4);
        var handle1 = heap.Insert(1);
        var handle2 = heap.Insert(2);
        var min = heap.Min;
        var count = heap.Count;
        var contains1 = heap.Contains(1, out var handleContains1);
        var contains2 = heap.Contains(2, out var handleContains2);
        var contains3 = heap.Contains(3, out var handleContains3);
        var contains4 = heap.Contains(4, out var handleContains4);

        // assert
        // heap tree:
        //            1
        //          2   3
        //         4      
        initialCount.Should().Be(0);
        count.Should().Be(4);
        min.Should().Be(1);
        handle1.Should().Be(handleContains1);
        handle2.Should().Be(handleContains2);
        handle3.Should().Be(handleContains3);
        handle4.Should().Be(handleContains4);
        contains1.Should().BeTrue();
        contains2.Should().BeTrue();
        contains3.Should().BeTrue();
        contains4.Should().BeTrue();
    }

    [Test]
    public void TestInsertWithResize()
    {
        // arrange
        var heap = new BinaryHeap<int>(new[] { 6, 5, 4, 2 });
        // heap tree:
        //            2
        //          5   4
        //         6      

        // act
        var initialCount = heap.Count;
        var initialMin = heap.Min;
        var handle3 = heap.Insert(3);
        var handle1 = heap.Insert(1);
        var handle9 = heap.Insert(9);
        var handle8 = heap.Insert(8);
        var handle7 = heap.Insert(7);
        var count = heap.Count;
        var min = heap.Min;
        var contains1 = heap.Contains(1, out var handleContains1);
        var contains2 = heap.Contains(2, out var handleContains2);
        var contains3 = heap.Contains(3, out var handleContains3);
        var contains4 = heap.Contains(4, out var handleContains4);
        var contains5 = heap.Contains(5, out var handleContains5);
        var contains7 = heap.Contains(7, out var handleContains7);
        var contains8 = heap.Contains(8, out var handleContains8);
        var contains9 = heap.Contains(9, out var handleContains9);

        // assert
        // heap tree:
        //             1
        //          3      2
        //        6   5   4 9
        //       8 7         
        initialCount.Should().Be(4);
        initialMin.Should().Be(2);
        count.Should().Be(4 + 5);
        min.Should().Be(1);
        handleContains2.Should().NotBeNull();
        handleContains4.Should().NotBeNull();
        handleContains5.Should().NotBeNull();
        handle1.Should().Be(handleContains1);
        handle3.Should().Be(handleContains3);
        handle7.Should().Be(handleContains7);
        handle8.Should().Be(handleContains8);
        handle9.Should().Be(handleContains9);
        contains1.Should().BeTrue();
        contains2.Should().BeTrue();
        contains3.Should().BeTrue();
        contains4.Should().BeTrue();
        contains5.Should().BeTrue();
        contains7.Should().BeTrue();
        contains8.Should().BeTrue();
        contains9.Should().BeTrue();
    }

    [Test]
    public void TestInsertAndExtractEmptyHeap()
    {
        // arrange
        var heap = new BinaryHeap<int>();

        // act
        var min = heap.InsertAndExtractMin(1);

        // assert
        min.Should().Be(1);
    }

    [Test]
    public void TestInsertAndExtractSingleElementHeap()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 1 });

        // act, first replace a min with a greater value, then with a lesser one
        var min1 = heap.InsertAndExtractMin(2);
        var min2 = heap.InsertAndExtractMin(5);

        // assert
        min1.Should().Be(1);
        min2.Should().Be(2);
    }

    [Test]
    public void TestInsertAndExtractMultiElementHeap()
    {
        // arrange
        var heap = new BinaryHeap<int>(new int[] { 1, 2, 3, 4, 5 });

        // act
        var min1 = heap.InsertAndExtractMin(1);
        var min2 = heap.InsertAndExtractMin(7);
        var min3 = heap.InsertAndExtractMin(6);
        var min4 = heap.InsertAndExtractMin(8);
        var min5 = heap.InsertAndExtractMin(2);

        // assert
        min1.Should().Be(1);
        min2.Should().Be(1);
        min3.Should().Be(2);
        min4.Should().Be(3);
        min5.Should().Be(2);
    }
}
