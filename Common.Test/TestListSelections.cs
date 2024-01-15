using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common.Collections;

namespace matthiasffm.Common.Test;

internal class TestListSelections
{
    [Test]
    public void TestPartitionOnEmptyList()
    {
        // arrange

        var emptyList = new List<int> { };

        // act

        var partitionIdx = emptyList.Partition();

        // assert

        emptyList.Should().BeEmpty();
        partitionIdx.Should().Be(0);
    }

    [Test]
    public void TestPartition()
    {
        // arrange

        var listWithOneEntry = new List<int> { 2 };
        var listToPartition1 = new List<int> { 2, 1, 7, 8, 4, 3, 10, 5 };
        var listToPartition2 = new List<int> { 2, 7, 1, 8, 3, 4, 5, 10 };

        // act

        var idxForOneEntryList = listWithOneEntry.Partition();
        var partitionIdx1 = listToPartition1.Partition();
        var partitionIdx2 = listToPartition2.Partition();

        // assert

        idxForOneEntryList.Should().Be(0);
        listWithOneEntry.Should().Equal(2);

        partitionIdx1.Should().Be(4);
        listToPartition1[partitionIdx1].Should().Be(5);
        listToPartition1.GetRange(0, partitionIdx1).Should().OnlyContain(x => x <= listToPartition1[partitionIdx1]);
        listToPartition1.GetRange(partitionIdx1, listToPartition1.Count - partitionIdx1).Should().OnlyContain(x => x >= listToPartition1[partitionIdx1]);
        listToPartition1.Should().HaveCount(8).And.BeEquivalentTo(new[] { 1, 2, 3, 4, 5, 7, 8, 10 });

        partitionIdx2.Should().Be(7);
        listToPartition2[partitionIdx2].Should().Be(10);
        listToPartition2.GetRange(0, partitionIdx2).Should().OnlyContain(x => x <= 10);
        listToPartition2.GetRange(partitionIdx2, listToPartition2.Count - partitionIdx2).Should().OnlyContain(x => x >= 10);
        listToPartition2.Should().HaveCount(8).And.BeEquivalentTo(new[] { 1, 2, 3, 4, 5, 7, 8, 10 });
    }

    [Test]
    public void TestPartitionSublistErrors()
    {
        // arrange

        var listToPartition = new List<int> { 2, 1, 7, 8, 4, 3, 10, 5 };

        // act

        var negativeIndexPart = () => listToPartition.Partition(-4, 3);
        var negativeSlicePart = () => listToPartition.Partition(4, 3);
        var pTooBigPart = () => listToPartition.Partition(20, 21);
        var rTooBigPart = () => listToPartition.Partition(4, 30);

        // assert

        negativeIndexPart.Should().Throw<ArgumentOutOfRangeException>();
        negativeSlicePart.Should().Throw<ArgumentOutOfRangeException>();
        pTooBigPart.Should().Throw<ArgumentOutOfRangeException>();
        rTooBigPart.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void TestPartitionSublist()
    {
        // arrange

        var listWithOneEntry = new List<int> { 2 };
        var listToPartition1 = new List<int> { 2, 1, 7, 8, 4, 3, 10, 5 };
        var listToPartition2 = new List<int>(listToPartition1);

        // act

        var idxForOneEntryList = listWithOneEntry.Partition(0, 0);
        var partition2To4Idx = listToPartition1.Partition(2, 4);
        var partition1To5Idx = listToPartition2.Partition(1, 5);

        // assert

        idxForOneEntryList.Should().Be(0);
        listWithOneEntry.Should().Equal(2);

        // after partition of sublist { 7, 8, 4 }, full list looks like = { 2, 1, <=4, >=4, >=4, 3, 10, 5)
        partition2To4Idx.Should().Be(2);
        listToPartition1.GetRange(0, 2).Should().Equal(2, 1);
        listToPartition1.GetRange(5, 3).Should().Equal(3, 10, 5);
        listToPartition1.GetRange(2, partition2To4Idx - 1).Should().OnlyContain(x => x <= 4);
        listToPartition1.GetRange(partition2To4Idx, 3).Should().OnlyContain(x => x >= 4);
        listToPartition1.Should().HaveCount(8).And.BeEquivalentTo(new[] { 2, 1, 4, 7, 8, 3, 10, 5 });

        // after partition of sublist { 1, 7, 8, 4, 3 }, full list looks like = { 2, <=3, <=3, >=3, >=3, =>3, 10, 5)
        partition1To5Idx.Should().Be(2);
        listToPartition1.GetRange(0, 1).Should().Equal(2);
        listToPartition1.GetRange(6, 2).Should().Equal(10, 5);
        listToPartition2.GetRange(1, partition1To5Idx).Should().OnlyContain(x => x <= 3);
        listToPartition2.GetRange(partition1To5Idx, 4).Should().OnlyContain(x => x >= 3);
        listToPartition2.Should().HaveCount(8).And.BeEquivalentTo(new[] { 2, 1, 4, 7, 8, 3, 10, 5 });
    }

    [Test]
    public void TestMedian()
    {
        // arrange

        var emptyList        = new List<int> { };
        var listWithOneEntry = new List<int> { 2 };
        var listEven         = new List<int> { 2, 1, 7, 4, 10, 11, 8, 5 };
        var listOdd          = new List<int> { 2, 1, 7, 8, 4, 3, 5 };

        // act

        var medianForEmptyList          = () => emptyList.Median();
        var medianForListWithOneEntry   = listWithOneEntry.Median();
        var medianForEvenList           = listEven.Median();
        var medianForOddList            = listOdd.Median();

        // assert

        medianForEmptyList.Should().Throw<ArgumentOutOfRangeException>("list");
        medianForListWithOneEntry.Should().Be(2);
        medianForEvenList.Should().Be(5);
        medianForOddList.Should().Be(4);
    }

    [Test]
    public void TestNthSmallest()
    {
        // arrange

        var emptyList        = new List<int> { };
        var listWithOneEntry = new List<int> { 2 };
        var list             = new List<int> { 2, 1, 7, 4, 10, 11, 8, 5 };

        // act

        var nthSmallestForEmptyList         = () => emptyList.NthSmallest(0);
        var nthSmallestForListWithOneEntry  = listWithOneEntry.NthSmallest(1);
        var nthSmallest0                    = () => list.NthSmallest(0);
        var nthSmallest9                    = () => list.NthSmallest(9);
        var nthSmallest1                    = list.NthSmallest(1);
        var nthSmallest2                    = list.NthSmallest(2);
        var nthSmallest4                    = list.NthSmallest(4);
        var nthSmallest8                    = list.NthSmallest(8);

        // assert

        nthSmallestForEmptyList.Should().Throw<ArgumentOutOfRangeException>("list");
        nthSmallestForListWithOneEntry.Should().Be(2);
        nthSmallest0.Should().Throw<ArgumentOutOfRangeException>("nth");
        nthSmallest9.Should().Throw<ArgumentOutOfRangeException>("nth");
        nthSmallest1.Should().Be(1);
        nthSmallest2.Should().Be(2);
        nthSmallest4.Should().Be(5);
        nthSmallest8.Should().Be(11);
    }
}
