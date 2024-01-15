namespace matthiasffm.Common.Collections;

/// <summary>
/// Provides helper functions for IEnumerable and ICollection.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Allows to write item.In(collection) instead of collection.Contains(item).
    /// </summary>
    public static bool In<T>(this T subject, IEnumerable<T> items)
    {
        return items.Contains(subject);
    }

    /// <summary>
    /// Allows to write item.In(collection) instead of collection.Contains(item).
    /// </summary>
    /// <example>
    /// if(name.In("name1", "name2", "name3"))
    /// { ... }
    /// </example>
    public static bool In<T>(this T subject, params T[] items)
    {
        return items.Contains(subject);
    }

    /// <summary>
    /// Provides the AddRange method similar to List{T}.AddRange for all other ICollection-implementations.
    /// </summary>
    public static void AddRange<T>(this ICollection<T> coll, IEnumerable<T> toAdd)
    {
        ArgumentNullException.ThrowIfNull(coll);
        ArgumentNullException.ThrowIfNull(toAdd);

        foreach(var elem in toAdd)
        {
            coll.Add(elem);
        }
    }

    /// <summary>Provides iterator over all 32 bit integers from <i>start</i> to <i>end</i>.</summary>
    public static IEnumerable<int> To(this int start, int end)
    {
        int i = start;
        int diff = System.Math.Sign(end - start);

        for(;;)
        {
            yield return i;

            if(i == end)
            {
                break;
            }
            i += diff;
        }
    }

    /// <summary>Provides iterator over all 64 bit integers from <i>start</i> to <i>end</i>.</summary>
    public static IEnumerable<long> To(this long start, long end)
    {
        long i = start;
        long diff = System.Math.Sign(end - start);

        for(; ; )
        {
            yield return i;

            if(i == end)
            {
                break;
            }
            i += diff;
        }
    }

    /// <summary>
    /// Shifts an enumeration to the left by <i>count</i> elements and adds the overflowing elements to the end.
    /// </summary>
    /// <remarks>
    /// |0|1|2|3|4|5|                          |0|1|2|3|4|5|
    /// |-|-|-|-|-|-|   => RotateLeft(2):      |-|-|-|-|-|-|
    /// |a|b|c|c|e|f|          |a|b|           |c|c|e|f|a|b|
    /// </remarks>
    public static IEnumerable<T> RotateLeft<T>(this IEnumerable<T> coll, int count = 1)
    {
        if(count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        if(!coll.Any())
        {
            return coll;
        }

        return coll.Skip(count).Concat(coll.Take(count));
    }

    /// <summary>
    /// Shifts an enumeration to the right by <i>count</i> elements and adds the overflowing elements to the start
    /// (that means the overflowing elements are iterated first).
    /// </summary>
    /// <remarks>
    /// |0|1|2|3|4|5|                          |0|1|2|3|4|5|
    /// |-|-|-|-|-|-|   => RotateRight(2):     |-|-|-|-|-|-|
    /// |a|b|c|c|e|f|          |e|f|           |e|f|a|b|c|c|
    /// </remarks>
    public static IEnumerable<T> RotateRight<T>(this IEnumerable<T> coll, int count = 1)
    {
        if(count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        if(!coll.Any())
        {
            return coll;
        }

        return coll.TakeLast(count).Concat(coll.SkipLast(count));
    }

    /// <summary>
    /// Returns all pairs of consecutive elements from <i>coll</i> (directly consecutive if step == 1).
    /// </summary>
    public static IEnumerable<(T, T)> Pairs<T>(this IEnumerable<T> coll, int step = 1)
        => coll.Zip(coll.Skip(step));

    /// <summary>
    /// Returns all possible pairings of two disjoint elements from <i>coll</i>.
    /// </summary>
    public static IEnumerable<(T, T)> Variations<T>(this IEnumerable<T> coll)
        => coll.SelectMany((elem1, i) => coll.Where((elem2, j) => j != i)
                                             .Select(elem2 => (elem1, elem2)));

    /// <summary>
    /// Calls <i>action</i> on every element of the enumeration.
    /// </summary>
    public static void Do<T>(this IEnumerable<T> coll, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(coll);
        ArgumentNullException.ThrowIfNull(action);

        foreach(var elem in coll)
        {
            action(elem);
        }
    }

    /// <summary>
    /// Calls <i>action</i> on every element of the async enumeration.
    /// </summary>
    public static async Task DoAsync<T>(this IAsyncEnumerable<T> collAsync, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(collAsync);
        ArgumentNullException.ThrowIfNull(action);

        await foreach(var elem in collAsync)
        {
            action(elem);
        }
    }
}
