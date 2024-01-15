namespace matthiasffm.Common.Algorithms;

/// <summary>
/// Contains some basic utility methods.
/// </summary>
public static class Basics
{
    /// <summary>Swaps the content of a and b</summary>
    public static void Swap<T>(ref T a, ref T b)
    {
        (a, b) = (b, a);
    }

    /// <summary>Swaps two elements of a list at index i and j.</summary>
    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        ArgumentNullException.ThrowIfNull(list);

        (list[j], list[i]) = (list[i], list[j]);
    }
}
