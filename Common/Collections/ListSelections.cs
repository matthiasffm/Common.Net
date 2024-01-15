namespace matthiasffm.Common.Collections;

using matthiasffm.Common.Algorithms;

// TODO: provide immutable functions (using ReadOnylSpan) that dont change the underlying list data => more memory consumption
//       but better concurrent behaviour


/// <summary>
/// Provides algorithms for generic lists.
/// </summary>
public static class ListSelections
{
    /// <summary>
    /// Partitions a list in place by finding a partition index x so that all elements in list[...x-1] ≤ list[x] and
    /// all elements in list[x+1...] ≤ list[x].
    /// </summary>
    /// <param name="list">list to partition</param>
    /// <returns>partition index x</returns>
    /// <remarks>runs in O(n)</remarks>
    public static int Partition<T>(this IList<T> list) where T : IComparable
    {
        ArgumentNullException.ThrowIfNull(list);

        return list.Count > 0 ? list.Partition(0, list.Count - 1) : 0;
    }

    /// <summary>
    /// Partitions a slice of a list in place by finding a partition index x so that all elements in list[p...x-1] ≤ list[x]
    /// and all elements in list[x+1...r] ≤ list[x].
    /// </summary>
    /// <param name="list">list to partition</param>
    /// <param name="p">start index of the slice where partition begins</param>
    /// <param name="r">end index of the slice where partition ends</param>
    /// <returns>partition index x (p ≤ x ≤ r)</returns>
    /// <remarks>runs in O(n)</remarks>
    public static int Partition<T>(this IList<T> list, int p, int r) where T : IComparable
    {
        ArgumentNullException.ThrowIfNull(list);

        if(p < 0 || p >= list.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(p), "Index p is outside of list.");
        }
        if(r < 0 || r >= list.Count || p > r)
        {
            throw new ArgumentOutOfRangeException(nameof(r), "Index r is outside of list.");
        }

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

    /// <summary>
    /// Finds the median element in an unsorted list.
    /// </summary>
    /// <returns>median element</returns>
    /// <remarks>runs in O(n)</remarks>
    public static T Median<T>(this IList<T> list) where T : IComparable
    {
        ArgumentNullException.ThrowIfNull(list);

        if(list.Count == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(list), "List has no elements");
        }

        return list.NthSmallest((list.Count & 1) == 1 ? (list.Count + 1) / 2 : list.Count / 2);
    }

    /// <summary>
    /// Finds the n-th smallest element in an unsorted list.
    /// </summary>
    /// <param name="list">unsorted list</param>
    /// <param name="nth">the n in 'n-th smallest' (1 ≤ n ≤ list.Count)</param>
    /// <returns>n-th smallest element in list</returns>
    /// <remarks>runs in O(n)</remarks>
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
