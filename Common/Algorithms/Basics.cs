namespace matthiasffm.Common.Algorithms;

/// <summary>
/// Fasst generische Standardfunktionen zusammen, die sonst kontextuell zu keinem Thema gehören.
/// </summary>
public static class Basics
{
    /// <summary>Tauscht die Inhalte von a und b</summary>
    public static void Swap<T>(ref T a, ref T b)
    {
        (a, b) = (b, a);
    }

    /// <summary>Tauscht die zwei Elemente der Liste an Position i und j</summary>
    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        ArgumentNullException.ThrowIfNull(list);

        (list[j], list[i]) = (list[i], list[j]);
    }
}
