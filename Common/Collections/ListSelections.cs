namespace matthiasffm.Common.Collections;

using matthiasffm.Common.Algorithms;

public static class ListSelections
{
    /// <summary>
    /// Partitioniert eine List in place und findet einen Partitionsindex x, so dass in list[...x-1] alle Elemente kleiner gleich list[x] und
    /// alle Elemente in list[x+1...] größer gleich list[x] sind.
    /// </summary>
    /// <param name="list">die zu partitionierende Liste</param>
    /// <returns>Partitionsindex x</returns>
    /// <remarks>Laufzeit O(n)</remarks>
    public static int Partition<T>(this IList<T> list) where T : IComparable
    {
        ArgumentNullException.ThrowIfNull(list);

        return list.Count > 0 ? list.Partition(0, list.Count - 1) : 0;
    }

    /// <summary>
    /// Partitioniert eine List in place und findet einen Partitionsindex x, so dass in list[...x-1] alle Elemente kleiner gleich list[x] und
    /// alle Elemente in list[x+1...] größer gleich list[x] sind.
    /// </summary>
    /// <param name="list">die zu partitionierende Liste</param>
    /// <param name="p">Index, ab dem die Liste partitioniert werden soll</param>
    /// <param name="r">Index, bis zu dem die Liste partitioniert werden soll</param>
    /// <returns>Partitionsindex x</returns>
    /// <remarks>Laufzeit O(n)</remarks>
    public static int Partition<T>(this IList<T> list, int p, int r) where T : IComparable
    {
        ArgumentNullException.ThrowIfNull(list);

        var x = list[r];
        var i = p - 1;

        for(int j = p; j < r; j++)
        {
            if(list[j].CompareTo(x) <= 0)
            {
                i++;
                list.Swap(i, j);
            }
        }

        list.Swap(i + 1, r);

        return i + 1;
    }

    /// <summary>Ermittelt den Median in einer unsortierten Liste.</summary>
    /// <remarks>Laufzeit O(n)</remarks>
    public static T Median<T>(this IList<T> list) where T : IComparable
    {
        ArgumentNullException.ThrowIfNull(list);

        if(list.Count == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(list), "List has no elements");
        }

        return list.NthSmallest((list.Count & 1) == 1 ? (list.Count + 1) / 2 : list.Count / 2);
        // return list.NthSmallest((list.Count & 1) == 1 ? list.Count / 2 : list.Count / 2 - 1);
    }

    /// <summary>Ermittelt das n-kleinste Element einer Liste</summary>
    /// <param name="list">unsortierte Liste, in der das n-kleinste Element gesucht wird</param>
    /// <param name="nth">Index des gesuchten n-kleinste Element (1...list.Count)</param>
    /// <returns>n-kleinstes Element in list</returns>
    /// <remarks>Laufzeit O(n)</remarks>
    public static T NthSmallest<T>(this IList<T> list, int nth) where T : IComparable
    {
        if(list == null || list.Count == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(list), "List has no elements");
        }
        if(nth <= 0 || nth > list.Count) 
        {
            throw new ArgumentOutOfRangeException(nameof(nth));
        }

        return list.SelectNth(0, list.Count - 1, nth);
    }

    private static T SelectNth<T>(this IList<T> list, int p, int r, int i) where T : IComparable
    {
        if(p == r)
        {
            return list[p];
        }

        var q = list.Partition(p, r);

        var k = q - p + 1;
        if(i < k)
        {
            return SelectNth(list, p, q - 1, i);
        }
        else if(i == k)
        {
            return list[q];
        }
        else // i > k
        {
            return SelectNth(list, q + 1, r, i - k);
        }
    }
}
