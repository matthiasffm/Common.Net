using matthiasffm.Common.Collections;

namespace matthiasffm.Common.Algorithms;

/// <summary>
/// Utility class for sorting collections with the HeapSort algorithm.
/// </summary>
public static class HeapSort
{
    /// <summary>
    /// Sorts the collection of <paramref name="items"/> with the Heapsort algorithm.
    /// </summary>
    /// <param name="items">elements to sort</param>
    /// <returns>sorted <paramref name="items"/></returns>
    /// <remarks>Sorts the elements in O(n logn) time and O(n) space.</remarks>
    public static IEnumerable<TElement> Sort<TElement>(IEnumerable<TElement> items)
    {
        var heap = new BinaryHeap<TElement>(items);
        while(heap.Count > 0)
        {
            yield return heap.ExtractMin();
        }
    }
}
