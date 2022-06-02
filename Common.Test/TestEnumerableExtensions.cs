using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common;

namespace matthiasffm.Common.Test;

internal class TestEnumerableExtensions
{
    [Test]
    public void TestIn()
    {
        // arrange

        int[]               testArray   = new[] { 1, 2, 3 };
        IEnumerable<string> testColl    = new List<string> { "", "eins", "zwei" };

        // act

        var inNullArray  = () => 1.In(null);
        var inEmptyArray = 1.In(Array.Empty<int>());

        var in1Array     = 1.In(testArray);
        var in2Array     = 2.In(testArray);
        var notIn5Array  = 5.In(testArray);

        var inNullEnum  = () => 1.In((IEnumerable<int>)null);
        var inEmptyEnum = "eins".In(new List<string>());

        var in1Enum      = "eins".In(testColl);
        var in2Enum      = "zwei".In(testColl);
        var notIn5Enum   = "fuenf".In(testColl);

        // assert

        inNullArray.Should().Throw<ArgumentNullException>();
        inEmptyArray.Should().BeFalse();

        in1Array.Should().BeTrue();
        in2Array.Should().BeTrue();
        notIn5Array.Should().BeFalse();

        inNullEnum.Should().Throw<ArgumentNullException>();
        inEmptyEnum.Should().BeFalse();

        in1Enum.Should().BeTrue();
        in2Enum.Should().BeTrue();
        notIn5Enum.Should().BeFalse();
    }

    [Test]
    public void TestAddRange()
    {
        // arrange

        var testColl = new HashSet<string> { "", "eins", "zwei" };

        // act

        testColl.AddRange(new[] { "drei", "vier" });

        // assert

        testColl.Should().HaveCount(5).And.Equal("", "eins", "zwei", "drei", "vier");
    }

    [Test]
    public void TestToInt()
    {
        // arrange

        // act

        var from2to2 = 2.To(2);
        var from0to3 = 0.To(3);
        var from2to5 = 2.To(5);
        var fromMinus5to2 = (-5).To(2);
        var fromMinus5toMinus3 = (-5).To(-3);
        var fromMinus5toMinus5 = (-5).To(-5);

        var from2to1 = 2.To(1);
        var from2toMinus1 = 2.To(-1);
        var fromMinus1toMinus2 = (-1).To(-2);
        var from5to2 = 5.To(2);

        // assert

        from2to2.Should().HaveCount(1).And.Equal(2);
        from0to3.Should().HaveCount(4).And.Equal(0, 1, 2, 3);
        from2to5.Should().HaveCount(4).And.Equal(2, 3, 4, 5);
        fromMinus5to2.Should().HaveCount(8).And.Equal(-5, -4, -3, -2, -1, 0, 1, 2);
        fromMinus5toMinus3.Should().HaveCount(3).And.Equal(-5, -4, -3);
        fromMinus5toMinus5.Should().HaveCount(1).And.Equal(-5);

        from2to1.Should().HaveCount(2).And.Equal(2, 1);
        from2toMinus1.Should().HaveCount(4).And.Equal(2, 1, 0, -1);
        fromMinus1toMinus2.Should().HaveCount(2).And.Equal(-1, -2);
        from5to2.Should().HaveCount(4).And.Equal(5, 4, 3, 2);
    }

    [Test]
    public void TestToLong()
    {
        // arrange

        // act

        var from2to2 = 2L.To(2L);
        var from0to3 = 0L.To(3L);
        var from2to5 = 2L.To(5L);
        var fromMinus5to2 = (-5L).To(2L);
        var fromMinus5toMinus3 = (-5L).To(-3L);
        var fromMinus5toMinus5 = (-5L).To(-5L);

        var from2to1 = 2L.To(1L);
        var from2toMinus1 = 2L.To(-1L);
        var fromMinus1toMinus2 = (-1L).To(-2L);
        var from5to2 = 5L.To(2L);

        // assert

        from2to2.Should().HaveCount(1).And.Equal(2L);
        from0to3.Should().HaveCount(4).And.Equal(0L, 1L, 2L, 3L);
        from2to5.Should().HaveCount(4).And.Equal(2L, 3L, 4L, 5L);
        fromMinus5to2.Should().HaveCount(8).And.Equal(-5L, -4L, -3L, -2L, -1L, 0L, 1L, 2L);
        fromMinus5toMinus3.Should().HaveCount(3).And.Equal(-5L, -4L, -3L);
        fromMinus5toMinus5.Should().HaveCount(1).And.Equal(-5L);

        from2to1.Should().HaveCount(2).And.Equal(2L, 1L);
        from2toMinus1.Should().HaveCount(4).And.Equal(2L, 1L, 0L, -1L);
        fromMinus1toMinus2.Should().HaveCount(2).And.Equal(-1L, -2L);
        from5to2.Should().HaveCount(4).And.Equal(5L, 4L, 3L, 2L);

    }

    [Test]
    public void TestRotateLeft()
    {
        // arrange

        var empty = Array.Empty<int>();
        var coll  = new int[] { 1, 2, 3, 4, 5 };

        // act

        var rotateNull = () => ((int[])null).RotateLeft(1);
        var rotateCollMinus = () => coll.RotateLeft(-2);

        var rotateEmpty = empty.RotateLeft(1);
        var rotateColl0 = coll.RotateLeft(0);
        var rotateColl1 = coll.RotateLeft(1);
        var rotateColl2 = coll.RotateLeft(2);
        var rotateColl5 = coll.RotateLeft(5);
        var rotateColl7 = coll.RotateLeft(7);

        // assert

        rotateNull.Should().Throw<ArgumentNullException>();
        rotateCollMinus.Should().Throw<ArgumentOutOfRangeException>();

        rotateEmpty.Should().HaveCount(0);
        rotateColl0.Should().HaveCount(5).And.Equal(1, 2, 3, 4, 5);
        rotateColl1.Should().HaveCount(5).And.Equal(2, 3, 4, 5, 1);
        rotateColl2.Should().HaveCount(5).And.Equal(3, 4, 5, 1, 2);
        rotateColl5.Should().HaveCount(5).And.Equal(1, 2, 3, 4, 5);
        rotateColl7.Should().HaveCount(5).And.Equal(3, 4, 5, 1, 2);
    }

    [Test]
    public void TestRotateRight()
    {
        // arrange

        var empty = Array.Empty<int>();
        var coll = new int[] { 1, 2, 3, 4, 5 };

        // act

        var rotateNull = () => ((int[])null).RotateRight(1);
        var rotateCollMinus = () => coll.RotateRight(-2);

        var rotateEmpty = empty.RotateRight(1);
        var rotateColl0 = coll.RotateRight(0);
        var rotateColl1 = coll.RotateRight(1);
        var rotateColl2 = coll.RotateRight(2);
        var rotateColl5 = coll.RotateRight(5);
        var rotateColl7 = coll.RotateRight(7);

        // assert

        rotateNull.Should().Throw<ArgumentNullException>();
        rotateCollMinus.Should().Throw<ArgumentOutOfRangeException>();

        rotateEmpty.Should().HaveCount(0);
        rotateColl0.Should().HaveCount(5).And.Equal(1, 2, 3, 4, 5);
        rotateColl1.Should().HaveCount(5).And.Equal(5, 1, 2, 3, 4);
        rotateColl2.Should().HaveCount(5).And.Equal(4, 5, 1, 2, 3);
        rotateColl5.Should().HaveCount(5).And.Equal(1, 2, 3, 4, 5);
        rotateColl7.Should().HaveCount(5).And.Equal(4, 5, 1, 2, 3);
    }


    [Test]
    public void TestPairs()
    {
        // arrange

        var empty = Array.Empty<int>();
        var coll  = new int[] { 1, 2, 3, 5 };

        // act

        var nullPairs  = () => ((IEnumerable<int>)null).Pairs();

        var emptyPairs = empty.Pairs();
        var coll1Pairs = coll.Pairs();
        var coll2Pairs = coll.Pairs(2);

        // assert

        nullPairs.Should().Throw<ArgumentNullException>();

        emptyPairs.Should().HaveCount(0);
        coll1Pairs.Should().HaveCount(3).And.Equal((1, 2), (2, 3), (3, 5));
        coll2Pairs.Should().HaveCount(2).And.Equal((1, 3), (2, 5));
    }

    [Test]
    public void TestVariations()
    {
        // arrange

        var empty = Array.Empty<int>();
        var coll = new int[] { 1, 2, 3, 5 };

        // act

        var nullVariations = () => ((IEnumerable<int>)null).Variations();

        var emptyVariations = empty.Variations();
        var collVariations  = coll.Variations();

        // assert

        nullVariations.Should().Throw<ArgumentNullException>();

        emptyVariations.Should().HaveCount(0);
        collVariations.Should().HaveCount(4 * 3).And.Equal((1, 2), (1, 3), (1, 5),
                                                           (2, 1), (2, 3), (2, 5),
                                                           (3, 1), (3, 2), (3, 5),
                                                           (5, 1), (5, 2), (5, 3));
    }
}
