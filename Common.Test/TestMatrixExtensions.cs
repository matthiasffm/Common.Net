using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common.Math;

namespace matthiasffm.Common.Test;

internal class TestMatrixExtensions
{
    #region Testdaten

    private static int[,] CreateNullMatix() => null;
    private static int[,] CreateEmptyMatix() => new int[0,0];
    private static int[,] CreateSingleMatix() => new int[,] { { 1 } };
    private static int[,] CreateHorizontalMatix() => new int[,] { { 11, 12, 13 } };
    private static int[,] CreateVerticalMatix() => new int[,] { { 11 }, { 21 }, { 31 } };
    private static int[,] CreateSimpleMatix() => new int[,] { { 11, 12, 13 }, { 21, 22, 23 }, { 31, 32, 33 } };

    #endregion

    [Test]
    public void TestSelect()
    {
        // arrange

        var nullMatrix      = CreateNullMatix();
        var emptyMatrix     = CreateEmptyMatix();
        var singleMatrix    = CreateSingleMatix();
        var horMatrix       = CreateHorizontalMatix();
        var vertMatrix      = CreateVerticalMatix();
        var simpleMatrix    = CreateSimpleMatix();

        // act

        var emptyElems      = emptyMatrix.Select((item, r, c) => r + c + item);
        var singleElems     = singleMatrix.Select((item, r, c) => r + c + item);
        var horElems        = horMatrix.Select((item, r, c) => r + c + item);
        var vertElems       = vertMatrix.Select((item, r, c) => r + c + item);
        var simpleElems     = simpleMatrix.Select((item, r, c) => r + c + item);

        // assert

        emptyElems.Should().HaveCount(0);
        singleElems.Should().HaveCount(1).And.Equal(1);
        horElems.Should().HaveCount(3).And.Equal(0 + 11, 1 + 12, 2 + 13);
        vertElems.Should().HaveCount(3).And.Equal(0 + 11, 1 + 21, 2 + 31);
        simpleElems.Should().HaveCount(9).And.Equal(0 + 11, 1 + 12, 2 + 13, 1 + 21, 2 + 22, 3 + 23, 2 + 31, 3 + 32, 4 + 33);
    }

    [Test]
    public void TestForEach()
    {
        // arrange

        var nullMatrix      = CreateNullMatix();
        var emptyMatrix     = CreateEmptyMatix();
        var singleMatrix    = CreateSingleMatix();
        var horMatrix       = CreateHorizontalMatix();
        var vertMatrix      = CreateVerticalMatix();
        var simpleMatrix    = CreateSimpleMatix();

        // act

        var nullForEach = () => nullMatrix.ForEach((item, r, c) => nullMatrix[r, c] = item + r + c);
        emptyMatrix.ForEach((item, r, c) => emptyMatrix[r, c] = item + r + c);
        singleMatrix.ForEach((item, r, c) => singleMatrix[r, c] = item + r + c);
        horMatrix.ForEach((item, r, c) => horMatrix[r, c] = item + r + c);
        vertMatrix.ForEach((item, r, c) => vertMatrix[r, c] = item + r + c);
        simpleMatrix.ForEach((item, r, c) => simpleMatrix[r, c] = item + r + c);

        // assert

        nullForEach.Should().Throw<ArgumentNullException>("matrix");
        emptyMatrix.Should().BeEquivalentTo(new int[0, 0]);
        singleMatrix.Should().BeEquivalentTo(new int[,] { { 0 + 1 } });
        horMatrix.Should().BeEquivalentTo(new int[,] { { 0 + 11, 1 + 12, 2 + 13 } });
        vertMatrix.Should().BeEquivalentTo(new int[,] { { 0 + 11 }, { 1 + 21 }, { 2 + 31 } });
        simpleMatrix.Should().BeEquivalentTo(new int[,] { { 0 + 11, 1 + 12, 2 + 13 }, { 1 + 21, 2 + 22, 3 + 23 }, { 2 + 31, 3 + 32, 4 + 33 } });
    }

    [Test]
    public void TestPopulate()
    {
        // arrange

        var nullMatrix      = CreateNullMatix();
        var emptyMatrix     = CreateEmptyMatix();
        var singleMatrix    = CreateSingleMatix();
        var horMatrix       = CreateHorizontalMatix();
        var vertMatrix      = CreateVerticalMatix();
        var simpleMatrix    = CreateSimpleMatix();

        // act

        var nullPopulate = () => nullMatrix.Populate((r, c) => r * 10 + c);
        emptyMatrix.Populate((r, c) => r * 10 + c);
        singleMatrix.Populate((r, c) => r * 10 + c);
        horMatrix.Populate((r, c) => r * 10 + c);
        vertMatrix.Populate((r, c) => r * 10 + c);
        simpleMatrix.Populate((r, c) => r * 10 + c);

        // assert

        nullPopulate.Should().Throw<ArgumentNullException>("matrix");
        emptyMatrix.Should().BeEquivalentTo(new int[0, 0]);
        singleMatrix.Should().BeEquivalentTo(new int[,] { { 0 } });
        horMatrix.Should().BeEquivalentTo(new int[,] { { 0, 1, 2 } });
        vertMatrix.Should().BeEquivalentTo(new int[,] { { 0 }, { 10 }, { 20 } });
        simpleMatrix.Should().BeEquivalentTo(new int[,] { { 0, 1, 2 }, { 10, 11, 12 }, { 20, 21, 22 } });
    }

    [Test]
    public void TestCount()
    {
        // arrange

        var nullMatrix      = CreateNullMatix();
        var emptyMatrix     = CreateEmptyMatix();
        var singleMatrix    = CreateSingleMatix();
        var horMatrix       = CreateHorizontalMatix();
        var vertMatrix      = CreateVerticalMatix();
        var simpleMatrix    = CreateSimpleMatix();

        // act

        var nullCount = () => nullMatrix.Count((item) => item > 11);
        var emptyCount  = emptyMatrix.Count((item) => item > 11);
        var singleCount = singleMatrix.Count((item) => item > 11);
        var horCount    = horMatrix.Count((item) => item > 11);
        var vertCount   = vertMatrix.Count((item) => item > 11);
        var simpleCount = simpleMatrix.Count((item) => item > 11);

        // assert

        nullCount.Should().Throw<ArgumentNullException>("matrix");
        emptyCount.Should().Be(0);
        singleCount.Should().Be(0);
        horCount.Should().Be(2);
        vertCount.Should().Be(2);
        simpleCount.Should().Be(8);
    }

    [Test]
    public void TestConvert()
    {
        // arrange

        int[][] nullMatrix  = null;
        var emptyArray     = Array.Empty<int[]>();
        var singleArray    = new int[][] { new int[] { 1 } };
        var horArray       = new int[][] { new int[] { 11, 12, 13 } };
        var vertArray      = new int[][] { new int[] { 11 }, new int[] { 21 }, new int[] { 31 } };
        var simpleArray    = new int[][] { new int[] { 11, 12, 13 }, new int[] { 21, 22, 23 }, new int[] { 31, 32, 33 } };
        var irregularArray = new int[][] { new int[] { 11, 12, 13 }, new int[] { 21, 22 }, new int[] { 31, 32, 33 } };

        // act

        var nullConvert     = () => nullMatrix.ConvertToMatrix();
        var emptyMatrix     = emptyArray.ConvertToMatrix();
        var singleMatrix    = singleArray.ConvertToMatrix();
        var horMatrix       = horArray.ConvertToMatrix();
        var vertMatrix      = vertArray.ConvertToMatrix();
        var simpleMatrix    = simpleArray.ConvertToMatrix();
        var irregularMatrix = () => irregularArray.ConvertToMatrix();

        // assert

        nullConvert.Should().Throw<ArgumentNullException>("matrix");
        emptyMatrix.SequenceEquals(CreateEmptyMatix()).Should().BeTrue();
        singleMatrix.Should().BeEquivalentTo(CreateSingleMatix());
        horMatrix.Should().BeEquivalentTo(CreateHorizontalMatix());
        vertMatrix.Should().BeEquivalentTo(CreateVerticalMatix());
        simpleMatrix.Should().BeEquivalentTo(CreateSimpleMatix());
        irregularMatrix.Should().Throw<ArgumentOutOfRangeException>("input");

    }

    [Test]
    public void TestSequenceEquals()
    {
        // arrange

        var nullMatrix      = CreateNullMatix();
        var emptyMatrix     = CreateEmptyMatix();
        var singleMatrix    = CreateSingleMatix();
        var horMatrix       = CreateHorizontalMatix();
        var vertMatrix      = CreateVerticalMatix();
        var simpleMatrix    = CreateSimpleMatix();

        // act

        var nullEqualsNull = () => nullMatrix.SequenceEquals(CreateNullMatix());
        var nullNotEqualsSimpleMatrix = () => nullMatrix.SequenceEquals(CreateSimpleMatix());
        var simpleMatrixEqualsNull = () => simpleMatrix.SequenceEquals(CreateNullMatix());

        var emptyMatrixEqualsEmptyMatrix = emptyMatrix.SequenceEquals(CreateEmptyMatix());
        var singleMatrixEqualsSingleMatrix = singleMatrix.SequenceEquals(CreateSingleMatix());
        var horMatrixEqualsHorMatrix = horMatrix.SequenceEquals(CreateHorizontalMatix());
        var vertMatrixEqualsVertMatrix = vertMatrix.SequenceEquals(CreateVerticalMatix());
        var simpleMatrixEqualsSimpleMatrix = simpleMatrix.SequenceEquals(CreateSimpleMatix());

        var emptyNotEqualsSimpleMatrix = emptyMatrix.SequenceEquals(CreateSimpleMatix());
        var simpleMatrixNotEqualsHorMatrix = simpleMatrix.SequenceEquals(CreateHorizontalMatix());
        var horMatrixNotEqualsVertMatrix = horMatrix.SequenceEquals(CreateVerticalMatix());

        // assert

        nullEqualsNull.Should().Throw<ArgumentNullException>("matrix");
        nullNotEqualsSimpleMatrix.Should().Throw<ArgumentNullException>("matrix");
        simpleMatrixEqualsNull.Should().Throw<ArgumentNullException>("right");

        emptyMatrixEqualsEmptyMatrix.Should().BeTrue();
        singleMatrixEqualsSingleMatrix.Should().BeTrue();
        horMatrixEqualsHorMatrix.Should().BeTrue();
        vertMatrixEqualsVertMatrix.Should().BeTrue();
        simpleMatrixEqualsSimpleMatrix.Should().BeTrue();

        emptyNotEqualsSimpleMatrix.Should().BeFalse();
        simpleMatrixNotEqualsHorMatrix.Should().BeFalse();
        horMatrixNotEqualsVertMatrix.Should().BeFalse();
    }

    [Test]
    public void TestAggregate()
    {
        // arrange

        var nullMatrix      = CreateNullMatix();
        var emptyMatrix     = CreateEmptyMatix();
        var singleMatrix    = CreateSingleMatix();
        var horMatrix       = CreateHorizontalMatix();
        var vertMatrix      = CreateVerticalMatix();
        var simpleMatrix    = CreateSimpleMatix();

        // act

        var nullAggr    = () => nullMatrix.Aggregate(0, (sum, item) => sum + item);
        var emptyAggr   = emptyMatrix.Aggregate(0, (sum, item) => sum + item);
        var singleAggr  = singleMatrix.Aggregate(0, (sum, item) => sum + item);
        var horAggr     = horMatrix.Aggregate(0, (sum, item) => sum + item);
        var vertAggr    = vertMatrix.Aggregate(0, (sum, item) => sum + item);
        var simpleAggr  = simpleMatrix.Aggregate(0, (sum, item) => sum + item);

        // assert

        nullAggr.Should().Throw<ArgumentNullException>("matrix");
        emptyAggr.Should().Be(0);
        singleAggr.Should().Be(1);
        horAggr.Should().Be(11 + 12 + 13);
        vertAggr.Should().Be(11 + 21 + 31);
        simpleAggr.Should().Be(11 + 12 + 13 + 21 + 22 + 23 + 31 + 32 + 33);
    }
}
