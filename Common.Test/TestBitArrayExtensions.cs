using FluentAssertions;
using NUnit.Framework;
using System.Collections;

using matthiasffm.Common.Collections;

namespace matthiasffm.Common.Test;

internal class TestBitArrayExtensions
{
    [Test]
    public void TestEqualsAll()
    {
        // arrange

        BitArray nullbits = null;

        var bitsEmpty = CreateBitArray();
        var bits      = CreateBitArray(1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1);

        // act

        var equalsEmptyTrue       = bitsEmpty.EqualsAll(CreateBitArray());
        var equalsEmptyFalse      = bitsEmpty.EqualsAll(CreateBitArray(0, 1 ,1));
        var equalsTrue            = bits.EqualsAll(CreateBitArray(1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1));
        var equalsTrueRepeat      = bits.EqualsAll(CreateBitArray(1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1));
        var equalsDiffLength      = bits.EqualsAll(CreateBitArray(1, 0, 1));
        var equalsDiffContent     = bits.EqualsAll(CreateBitArray(1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0));

        var equalsNull            = nullbits.EqualsAll(null);
        var equalsNonNull1        = nullbits.EqualsAll(bitsEmpty);
        var equalsNonNull2        = bitsEmpty.EqualsAll(nullbits);

        // assert

        equalsEmptyTrue.Should().BeTrue();
        equalsEmptyFalse.Should().BeFalse();
        equalsTrue.Should().BeTrue();
        equalsTrueRepeat.Should().BeTrue();
        equalsDiffLength.Should().BeFalse();
        equalsDiffContent.Should().BeFalse();

        equalsNull.Should().BeTrue();
        equalsNonNull1.Should().BeFalse();
        equalsNonNull2.Should().BeFalse();
    }

    [Test]
    public void TestSlice()
    {
        // arrange

        var bits      = CreateBitArray(1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1);
        var bitsEmpty = CreateBitArray();

        // act

        var sliceEmpty = bitsEmpty.Slice(0, 0);
        var slice0     = bits.Slice(0, 0);
        var slice1     = bits.Slice(0, 5);
        var slice2     = bits.Slice(2, 2);
        var slice3     = bits.Slice(6, 7);
        var slice4     = bits.Slice(4, 1);

        // assert

        sliceEmpty.EqualsAll(CreateBitArray()).Should().BeTrue();
        slice0.EqualsAll(CreateBitArray()).Should().BeTrue();
        slice1.EqualsAll(CreateBitArray(1, 0, 0, 1, 1)).Should().BeTrue();
        slice2.EqualsAll(CreateBitArray(0, 1)).Should().BeTrue();
        slice3.EqualsAll(CreateBitArray(0, 0, 1, 0, 0, 0, 0)).Should().BeTrue();
        slice4.EqualsAll(CreateBitArray(1)).Should().BeTrue();
    }

    [Test]
    public void TestConvertToInt()
    {
        // arrange

        var bits15    = CreateBitArray(1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1); // 6119H
        var bits29    = CreateBitArray(1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 0, 0, 0, 0, 1); // 10DD 6119H
        var bitsEmpty = CreateBitArray();

        // act

        var intEmpty  = bitsEmpty.ConvertToInt();

        var intBits15 = bits15.ConvertToInt();
        var intBits29 = bits29.ConvertToInt();

        var intBits2To7  = bits15.ConvertToInt(2, 6);
        var intBits9To15 = bits15.ConvertToInt(9, 5);

        // assert

        intEmpty.Should().Be(0);

        intBits15.Should().Be(24857);
        intBits29.Should().Be(282943769);

        intBits2To7.Should().Be(6);
        intBits9To15.Should().Be(16);
    }

    [Test]
    public void TestReverse()
    {
        // arrange

        var bitsEmpty  = CreateBitArray();
        var bitsSingle = CreateBitArray(1);
        var bits       = CreateBitArray(1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1);

        // act

        var emptyReverse    = bitsEmpty.Reverse();
        var singleReverse   = bitsSingle.Reverse();
        var bitsReverse     = bits.Reverse();

        // assert

        emptyReverse.EqualsAll(CreateBitArray()).Should().BeTrue();
        singleReverse.EqualsAll(CreateBitArray(1)).Should().BeTrue();
        bitsReverse.EqualsAll(CreateBitArray(1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 1)).Should().BeTrue();
    }

    private static BitArray CreateBitArray(params byte[] bits) => new(bits.Select(b => b == 1).ToArray());
}
