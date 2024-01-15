using System.Numerics;

namespace matthiasffm.Common.Math;

/// <summary>
/// Provides extension methods for basic math functions.
/// </summary>
public static class MathExtensions
{
    /// <summary>
    /// Returns the sum of all positive numbers from 1 to n.
    /// </summary>
    public static T SumNumbers<T>(T n) where T : INumberBase<T>
        => n * (n + T.One) / (T.One + T.One);

    /// <summary>
    /// Determines if a number is in the interval [min, max].
    /// </summary>
    public static bool Between<T>(this T val, T min, T max) where T : IComparisonOperators<T, T, bool>
        => min <= val && val <= max;

    /// <summary>
    /// there is no INumber interface a la T.IAbsOperation so we need this
    /// </summary>
    public static T Abs<T>(this T number) where T : INumberBase<T>, IComparisonOperators<T, T, bool>
        => number >= T.Zero ? number : -number;

    /// <summary>
    /// Clamps a specified numerical value to a specified minimum range.
    /// </summary>
    public static T Clamp<T>(this T val, T min) where T : IComparable<T>
        => val.CompareTo(min) < 0 ? min : val;

    /// <summary>
    /// Clamps a specified numerical value to a specified minimum and maximum range.
    /// </summary>
    public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        => val.CompareTo(min) < 0 ? min : (val.CompareTo(max) > 0 ? max : val);
}
