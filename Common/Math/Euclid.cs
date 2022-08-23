using System.Numerics;

namespace matthiasffm.Common.Math;

/// <summary>
/// Methoden zur Bestimmung des größten gemeinsamen Teilers zweier oder mehrere Zahlen nach Euklid.
/// </summary>
public static class Euclid
{
    /// <summary>
    /// Berechnet den größten gemeinsamen Teiler der Zahlen a und b
    /// </summary>
    public static T Gcd<T>(T a, T b) where T : INumber<T>
    {
        if(a == T.Zero)
        {
            return Abs(b);
        }

        while(b != T.Zero)
        {
            var h = a % b;
            a = b;
            b = h;
        }

        return Abs(a);
    }

    private static T Abs<T>(T number) where T : INumber<T> => number >= T.Zero ? number : -number;

    /// <summary>
    /// Berechnet den größten gemeinsamen Teiler aller Zahlen in <paramref name="numbers"/>.
    /// </summary>
    public static T Gcd<T>(params T[] numbers) where T : INumber<T>
    {
        ArgumentNullException.ThrowIfNull(numbers);

        return ((IEnumerable<T>)numbers).Gcd();
    }

    /// <summary>
    /// Berechnet den größten gemeinsamen Teiler aller Zahlen in <paramref name="numbers"/>.
    /// </summary>
    public static T Gcd<T>(this IEnumerable<T> numbers) where T : INumber<T>
    {
        ArgumentNullException.ThrowIfNull(numbers);

        return numbers.Skip(1).Aggregate(numbers.First(), (gcd, next) => Gcd(gcd, next));
    }

    /// <summary>
    /// Berechnet den größten gemeinsamen Teiler zweier Zahlen a und b sowie die Koeffizienten x und y nach dem
    /// Lemma von Bezout, so dass a * x + b * y = GGT(a, b).
    /// </summary>
    public static (T gcd, T x, T y) GcdExt<T>(T a, T b) where T : INumber<T>
    {
        if(a == T.Zero)
        {
            return (Abs(b), T.Zero, T.One);
        }
        if(b == T.Zero)
        {
            return (Abs(a), T.One, T.Zero);
        }

        var (ggt, x, y) = GcdExt(b, a % b);
        return (ggt, y, x - (a / b) * y);
    }
}
