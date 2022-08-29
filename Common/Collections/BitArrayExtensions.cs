using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace matthiasffm.Common.Collections;

/// <summary>
/// Stellt nützliche Erweiterungsmethoden für BitArrays bereit, zB Umwandlungen in Ints, für Vergleiche oder
/// um Slices daraus zu erstellen.
/// </summary>
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
    /// <param name="start">Startindex ab dem das Slice erstellt werden soll</param>
    /// <param name="length">Länge des Slice (muss positiv und kleiner oder gleich bits.Length - start sein)</param>
    public static BitArray Slice(this BitArray bits, int start, int length)
    {
        ArgumentNullException.ThrowIfNull(bits);
        if(bits.Length < start + length || length < 0)
        {
            throw new IndexOutOfRangeException("Das angeforderte Slice liegt ausserhalb des BitArrays");
        }

        var slice = new BitArray(length);

        for(int i = 0; i < length; i++)
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
        if(bits.Length > 32)
        {
            throw new ArgumentException("Es werden nur BitArrays mit Länge <= 32 Bits unterstützt.");
        }

        var result = new int[1];
        bits.CopyTo(result, 0);
        return result[0];
    }

    /// <summary>
    /// Konvertiert die length Bits des BitArrays ab start in einen 32-bitigen Integerwert. Dabei
    /// ist bits[0] das Bit mit dem höherstelligsten Wert 2^bits.Length.
    /// </summary>
    /// <param name="bits">Das Array mit den in einen 32 bit-Wert zu konvertierenden Bits (dessen Inhalt wird dabei nicht geändert)</param>
    /// <param name="start">Startindex ab dem Bits in Integer umgerechnet werden sollen</param>
    /// <param name="length">Anzahl der Bits im Array, die in Integer umgerechnet werden sollen (muss positiv und kleiner oder gleich bits.Length - start sein)</param>
    /// <remarks>Es werden nur BitArrays mit einer max. Länge von 32 Bit unterstützt.</remarks>
    public static int ConvertToInt(this BitArray bits, int start, int length)
    {
        ArgumentNullException.ThrowIfNull(bits);
        if(length > 32)
        {
            throw new ArgumentException("Es sind nur Aufrufe mit Länge <= 32 Bits erlaubt", nameof(length));
        }
        if(bits.Length < start + length || length < 0)
        {
            throw new IndexOutOfRangeException("Der zu konvertierende Bereich liegt ausserhalb des BitArrays");
        }

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

        for(int i = 0; i < bits.Length; i++)
        {
            reverse[i] = bits[bits.Length - i - 1];
        }

        return reverse;
    }

    /// <summary>
    /// Zählt die Anzahl an gesetzten Bits im Array.
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
    /// Zählt die Anzahl an ungesetzten Bits im Array.
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
