using System.Numerics;

namespace matthiasffm.Common.Math;

/// <summary>
/// Provides methods to solve problems from number theory.
/// </summary>
public static class NumberTheory
{
    /// <summary>
    /// Solves the linear congruence x for n tuples of numbers with x = [a(i), mod m(i)]:
    /// x = [a(1), mod m(1)]
    /// x = [a(2), mod m(2)]
    /// ...
    /// x = [a(n), mod m(n)]
    /// 
    /// The solution is the smallest x solving all these equations.
    /// </summary>
    /// <param name="input">Array of tuples of a(i) modulo m(i).</param>
    /// <returns>
    /// Tuple of x mod m' with m' being the least common multiple of all m(i) which satisfies all criteria from the input tuples.
    /// </returns>
    /// <remarks>
    /// see https://en.wikipedia.org/wiki/Chinese_remainder_theorem
    /// </remarks>
    /// <exception cref="OverflowException">In case of Overflowexception change from something like Int or Long to BigInteger.</exception>
    public static (T, T) CalcSimultaneousCongruences<T>(params (T, T)[] input) where T : INumber<T>
    {
        return CalcSimultaneousCongruences((IEnumerable<(T, T)>)input);
    }

    /// <see cref="CalcSimultaneousCongruences{T}(ValueTuple{T, T}[])"/>
    /// <exception cref="OverflowException">In case of Overflowexception change from something like Int or Long to BigInteger.</exception>
    public static (T, T) CalcSimultaneousCongruences<T>(IEnumerable<(T, T)> input) where T : INumber<T>
    {
        ArgumentNullException.ThrowIfNull(input);

        // solve congruences pairwise

        var iter = input.GetEnumerator();

        iter.MoveNext();
        var aggr = iter.Current;

        while(iter.MoveNext())
        {
            aggr = CalcSimultaneousCongruence(aggr.Item1, aggr.Item2, iter.Current.Item1, iter.Current.Item2);
        }

        return aggr;
    }

    /// <summary>
    /// Solves x = a1 mod m1 and x = a2 mod m2 for x for given a1, m1, a2 and m2.
    /// </summary>
    /// <returns>
    /// Tuple of x mod m' with m' being the least common multiple of m1 and m2 which satisfies all criteria from both input tuples.
    /// </returns>
    private static (T, T) CalcSimultaneousCongruence<T>(T a1, T m1, T a2, T m2) where T : INumber<T>
    {
        // first compute least common multiple with help from extended Euclid algorithm

        var (gcd, y, _) = Euclid.GcdExt(m1, m2);
        var lcm = m1 * m2 / gcd;

        // x can now be computed directly from lcm by using Chinese Restsatz (https://de.wikipedia.org/w/index.php?title=Chinesischer_Restsatz&section=8)

        var x = a1 - ((a1 - a2) / gcd * y * m1) % lcm;

        if(x < T.Zero)
        {
            return (x + lcm, lcm);
        }
        else
        {
            return (x, lcm);
        };
    }
}
