namespace matthiasffm.Common.Math;

/// <summary>
/// Methoden zur Bestimmung des größten gemeinsamen Teilers zweier oder mehrere ganzer Zahlen nach Euklid.
/// </summary>
public static class Euclid
{
    /// <summary>
    /// Berechnet den größten gemeinsamen Teiler der ganzen Zahlen a und b
    /// </summary>
    public static long Gcd(long a, long b)
    {
        if(a == 0)
        {
            return System.Math.Abs(b);
        }

        while(b != 0)
        {
            var h = a % b;
            a = b;
            b = h;
        }

        return System.Math.Abs(a);
    }

    /// <summary>
    /// Berechnet den größten gemeinsamen Teiler der ganzen Zahlen a und b
    /// </summary>
    public static int Gcd(int a, int b)
    {
        if(a == 0)
        {
            return System.Math.Abs(b);
        }

        while(b != 0)
        {
            var h = a % b;
            a = b;
            b = h;
        }

        return System.Math.Abs(a);
    }

    /// <summary>
    /// Berechnet den größten gemeinsamen Teiler aller ganzen Zahlen in <paramref name="numbers"/>.
    /// </summary>
    public static long Gcd(params long[] numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);

        return ((IEnumerable<long>)numbers).Gcd();
    }

    /// <summary>
    /// Berechnet den größten gemeinsamen Teiler aller ganzen Zahlen in <paramref name="numbers"/>.
    /// </summary>
    public static int Gcd(params int[] numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);

        return ((IEnumerable<int>)numbers).Gcd();
    }

    /// <summary>
    /// Berechnet den größten gemeinsamen Teiler aller ganzen Zahlen in <paramref name="numbers"/>.
    /// </summary>
    public static long Gcd(this IEnumerable<long> numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);

        return numbers.Skip(1).Aggregate(numbers.First(), (gcd, next) => Gcd(gcd, next));
    }

    /// <summary>
    /// Berechnet den größten gemeinsamen Teiler aller ganzen Zahlen in <paramref name="numbers"/>.
    /// </summary>
    public static int Gcd(this IEnumerable<int> numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);

        return numbers.Skip(1).Aggregate(numbers.First(), (gcd, next) => Gcd(gcd, next));
    }

    /// <summary>
    /// Berechnet den größten gemeinsamen Teiler zweier ganzer Zahlen a und b sowie die Koeffizienten x und y nach dem
    /// Lemma von Bezout, so dass a * x + b * y = GGT(a, b). Hierbei sind x und y ebenfalls ganzzahlig.
    /// </summary>
    public static (long gcd, long x, long y) GcdExt(long a, long b)
    {
        if(a == 0)
        {
            return (System.Math.Abs(b), 0, 1);
        }
        if(b == 0)
        {
            return (System.Math.Abs(a), 1, 0);
        }

        var (ggt, x, y) = GcdExt(b, a % b);
        return (ggt, y, x - (a / b) * y);
    }

    /// <summary>
    /// Berechnet den größten gemeinsamen Teiler zweier ganzer Zahlen a und b sowie die Koeffizienten x und y nach dem
    /// Lemma von Bezout, so dass a * x + b * y = GGT(a, b). Hierbei sind x und y ebenfalls ganzzahlig.
    /// </summary>
    public static (int gcd, int x, int y) GcdExt(int a, int b)
    {
        if(a == 0)
        {
            return (System.Math.Abs(b), 0, 1);
        }
        if(b == 0)
        {
            return (System.Math.Abs(a), 1, 0);
        }

        var (ggt, x, y) = GcdExt(b, a % b);
        return (ggt, y, x - (a / b) * y);
    }
}
