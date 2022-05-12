using System.Collections;
using System.Diagnostics;

namespace matthiasffm.Common.Collections;

public static class BitArrayExtensions
{
    /// <summary>
    /// Vergleicht die Inhalte zweiter Bitarrays auf Gleichheit.
    /// </summary>
    public static bool EqualsAll(this BitArray left, BitArray right)
        => left?.Length == right?.Length && 
           ((left == null && right == null) || ((BitArray)left!.Clone()).Xor(right!).OfType<bool>().All(b => b == false));

    /// <summary>
    /// Schneidet eine Teilmenge des Bitarrays von length Bits ab start aus und
    /// gibt sie als neues BitArray zurück.
    /// </summary>
    /// <param name="bits">Das Quell-Array, aus dem die Methode eine Teilmenge ausschneidet (dessen Inhalt wird dabei nicht geändert)</param>
    public static BitArray Slice(this BitArray bits, int start, int length)
    {
        ArgumentNullException.ThrowIfNull(bits);
        Debug.Assert(bits.Length >= start + length);

        var slice = new BitArray(length);

        for (int i = 0; i < length; i++)
        {
            slice[i] = bits[i + start];
        }

        return slice;
    }

    /// <summary>
    /// Konvertiert die Bits des BitArrays in einen 32-bitigen Integerwert. Dabei
    /// ist bits[length-1] das Bit mit dem höherstelligsten Wert 2^bits.Length (es
    /// wird von rechts gelesen == little endian).
    /// </summary>
    /// <param name="bits">Das Array mit den in einen 32 bit-Wert zu konvertierenden Bits (dessen Inhalt wird dabei nicht geändert)</param>
    /// <remarks>Es werden nur BitArrays mit einer max. Länge von 32 Bit unterstützt.</remarks>
    public static int ConvertToInt(this BitArray bits)
    {
        ArgumentNullException.ThrowIfNull(bits);
        Debug.Assert(bits.Length <= 32);

        var result = new int[1];
        bits.CopyTo(result, 0);
        return result[0];
    }

    /// <summary>
    /// Konvertiert die length Bits des BitArrays ab start in einen 32-bitigen Integerwert. Dabei
    /// ist bits[0] das Bit mit dem höherstelligsten Wert 2^bits.Length.
    /// </summary>
    /// <param name="bits">Das Array mit den in einen 32 bit-Wert zu konvertierenden Bits (dessen Inhalt wird dabei nicht geändert)</param>
    /// <remarks>Es werden nur BitArrays mit einer max. Länge von 32 Bit unterstützt.</remarks>
    public static int ConvertToInt(this BitArray bits, int start, int length)
    {
        ArgumentNullException.ThrowIfNull(bits);
        Debug.Assert(length <= 32);

        var slice = bits.Slice(start, length);

        var result = new int[1];
        slice.CopyTo(result, 0);
        return result[0];
    }

    /// <summary>
    /// Spiegelt ein BitArray und liefert das Ergebnis als neues BitArray zurück.
    /// </summary>
    /// <param name="bits">Das zu spiegelnde Array (dessen Inhalt wird dabei nicht geändert)</param>
    /// <returns>bits gespiegelt</returns>
    public static BitArray Reverse(this BitArray bits)
    {
        ArgumentNullException.ThrowIfNull(bits);
        var reverse = new BitArray(bits.Length);

        for (int i = 0; i < bits.Length; i++)
        {
            reverse[i] = bits[bits.Length - i - 1];
        }

        return reverse;
    }
}
