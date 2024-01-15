using System.Numerics;

namespace matthiasffm.Common.Math;

/// <summary>
/// Uses Euclids algorithm to calculate the greatest common divisor (GCD) or other ratios of two or more numbers.
/// </summary>
public static class Euclid
{
    /// <summary>
    /// Calculates the greatest common divisor (GCD) of two numbers.
    /// </summary>
    public static T Gcd<T>(T a, T b) where T : INumber<T>
    {
        if(a == T.Zero)
        {
            return b.Abs();
        }

        while(b != T.Zero)
        {
            var h = a % b;
            a = b;
            b = h;
        }

        return a.Abs();
    }

    /// <summary>
    /// Calculates the greatest common divisor (GCD) of all <paramref name="numbers"/>.
    /// </summary>
    public static T Gcd<T>(params T[] numbers) where T : INumber<T>
    {
        ArgumentNullException.ThrowIfNull(numbers);

        return numbers.Gcd();
    }

    /// <summary>
    /// Calculates the greatest common divisor (GCD) of all <paramref name="numbers"/>.
    /// </summary>
    public static T Gcd<T>(this IEnumerable<T> numbers) where T : INumber<T>
    {
        ArgumentNullException.ThrowIfNull(numbers);

        var iter = numbers.GetEnumerator();

        iter.MoveNext();
        var gcd = iter.Current;

        while(iter.MoveNext()) 
        {
            gcd = Gcd(gcd, iter.Current);
        }

        return gcd;
    }

    /// <summary>
    /// Calculates the greatest common divisor (GCD) of two numbers a and b and returns
    /// the coefficients x und y so that they fulfill Bézout's identity with ax + by = Gcd(a, b).
    /// </summary>
    public static (T gcd, T x, T y) GcdExt<T>(T a, T b) where T : INumber<T>
    {
        if(a == T.Zero)
        {
            return (b.Abs(), T.Zero, T.One);
        }
        if(b == T.Zero)
        {
            return (a.Abs(), T.One, T.Zero);
        }

        var (ggt, x, y) = GcdExt(b, a % b);
        return (ggt, y, x - (a / b) * y);
    }

    /// <summary>
    /// Calculates the least common multiple (LCM) of two numbers.
    /// </summary>
    public static T Lcm<T>(T a, T b) where T : INumber<T>
        => a * b / Gcd(a, b);

    /// <summary>
    /// Calculates the least common multiple (LCM) of all <paramref name="numbers"/>.
    /// </summary>
    public static T Lcm<T>(params T[] numbers) where T : INumber<T>
    {
        ArgumentNullException.ThrowIfNull(numbers);

        return numbers.Lcm();
    }

    /// <summary>
    /// Calculates the least common multiple (LCM) of all <paramref name="numbers"/>.
    /// </summary>
    public static T Lcm<T>(this IEnumerable<T> numbers) where T : INumber<T>
    {
        ArgumentNullException.ThrowIfNull(numbers);

        return numbers.Aggregate(T.One, (lcm, number) => Lcm(lcm, number));
    }
}
