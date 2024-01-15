using System.Collections;

namespace matthiasffm.Common.Collections;

/// <summary>
/// Provides utility functions for BitArray objects.
/// </summary>
public static class BitArrayExtensions
{
    /// <summary>
    /// Compares two BitArrays for equality.
    /// </summary>
    public static bool EqualsAll(this BitArray left, BitArray right)
        => left?.Length == right?.Length && 
           ((left == null && right == null) || ((BitArray)left!.Clone()).Xor(right!).OfType<bool>().All(b => b == false));

    /// <summary>
    /// Copies a slice of <i>length</i> of continuous bits starting at <i>start</i> to a new BitArray object.
    /// </summary>
    /// <param name="bits">Source data where bits are copied from (stays unchanged).</param>
    /// <param name="start">Start index where bits are copied from</param>
    /// <param name="length">Length of continuous slice where bits are copied from</param>
    public static BitArray Slice(this BitArray bits, int start, int length)
    {
        ArgumentNullException.ThrowIfNull(bits);
        if(start < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(start), "Start of slice is outside of the source BitArray.");
        }
        if(length < 0 || length > bits.Length - start)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Range of slice is outside of the source BitArray.");
        }

        var slice = new BitArray(length);

        for(int i = 0; i < length; i++)
        {
            slice[i] = bits[i + start];
        }

        return slice;
    }

    /// <summary>
    /// Converts the bits of a BitArray to a 32 bit integer value (the BitArray length should not exceed 32 bits of course).
    /// </summary>
    /// <remarks>
    /// The bits are read in little endian format so bits[length - 1] is the bit with the hightest value (2^bits.Length).
    /// </remarks>
    /// <param name="bits">The BitArray to convert to a 32 bit integer value (<i>bits</i>/> content stays unchanded)</param>
    /// <returns>Integer value of the BitArray</returns>
    public static int ConvertToInt(this BitArray bits)
    {
        ArgumentNullException.ThrowIfNull(bits);
        if(bits.Length > 32)
        {
            throw new ArgumentException("Only BitArray objects with a length of <= 32 bits are supported.");
        }

        var result = new int[1];
        bits.CopyTo(result, 0);
        return result[0];
    }

    /// <summary>
    /// Converts a slice of a BitArray to a 32 bit integer value (the length of the slice should not exceed 32 bits of course).
    /// </summary>
    /// <remarks>
    /// The bits are read in little endian format so bits[length - 1] is the bit with the hightest value (2^bits.Length).
    /// </remarks>
    /// <param name="bits">The BitArray to convert to a 32 bit integer value (<i>bits</i>/> content stays unchanded)</param>
    /// <param name="start">Start index of the slice to convert</param>
    /// <param name="length">Length of continuous slice where bits are converted from (can not exceed 32 bits)</param>
    /// <returns>Integer value of the BitArray slice</returns>
    public static int ConvertToInt(this BitArray bits, int start, int length)
    {
        ArgumentNullException.ThrowIfNull(bits);
        if(length > 32)
        {
            throw new ArgumentException("Only slices with a length of <= 32 bits are supported.", nameof(length));
        }

        var slice = bits.Slice(start, length);

        var result = new int[1];
        slice.CopyTo(result, 0);
        return result[0];
    }

    /// <summary>
    /// Reverses the bits in a BitArray and returns this value as a new object.
    /// </summary>
    /// <param name="bits">The BitArray to reverse (the function doesnt change the content)</param>
    /// <returns>new BitArray object with the reverse bits of the input BitArray</returns>
    public static BitArray Reverse(this BitArray bits)
    {
        ArgumentNullException.ThrowIfNull(bits);
        var reverse = new BitArray(bits.Length);

        for(int i = 0; i < bits.Length; i++)
        {
            reverse[i] = bits[bits.Length - i - 1];
        }

        return reverse;
    }

    /// <summary>
    /// Counts the number of set bits in the BitArray.
    /// </summary>
    public static long CountOnes(this BitArray bits)
    {
        ArgumentNullException.ThrowIfNull(bits);

        long setBits = 0L;

        for(int i = 0; i < bits.Length; i++)
        {
            if(bits[i])
            {
                setBits++;
            }
        }

        return setBits;
    }

    /// <summary>
    /// Counts the number of unset bits (zeroes) in the BitArray.
    /// </summary>
    public static long CountZeroes(this BitArray bits)
    {
        ArgumentNullException.ThrowIfNull(bits);

        long zeroBits = 0L;

        for(int i = 0; i < bits.Length; i++)
        {
            if(!bits[i])
            {
                zeroBits++;
            }
        }

        return zeroBits;
    }
}
