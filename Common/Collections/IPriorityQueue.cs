using System.Diagnostics.CodeAnalysis;

namespace matthiasffm.Common.Collections;

#pragma warning disable CA1711 // IPriorityQueue is named correctly and just missing from dotnet standard collections

/// <summary>
/// Interface of all minimum queues where enqueue and dequeue are ordered by a priority value and the first element is the one
/// with the lowest priority value.
/// </summary>
/// <typeparam name="TElement">Type of the elements stored in the priority queue.</typeparam>
/// <typeparam name="THandle">Type of the handle used to identify elements in the priority queue for <see cref="DecreaseElement(THandle, TElement)"/> operations.</typeparam>
/// <example>
/// var handle = pqueue.Insert(5);
/// pqueue.DecreaseElement(handle, 3);
/// </example>
public interface IPriorityQueue<TElement, THandle>
{
    /// <summary>
    /// Number of elements in the priority queue.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Searches for a specific element in the queue and returns its handle.
    /// </summary>
    /// <param name="toFind">Element to search for</param>
    /// <param name="handle">Handle of the element stored in the priority queue</param>
    /// <returns><i>true</i>, if <paramref name="toFind"/> is contained in this priority queue, else <i>false</i></returns>
    bool Contains(TElement toFind, [MaybeNullWhen(false)] out THandle handle);

    /// <summary>
    /// Returns the minimum element in this queue. Does not change the contents of the queue in the process of finding the minimum element.
    /// </summary>
    /// <exception cref="InvalidOperationException">The queue contains no elements</exception>
    TElement Min { get; }

    /// <summary>
    /// Removes the minimum element from the queue and returns its value.
    /// </summary>
    /// <exception cref="InvalidOperationException">The queue contains no elements</exception>
    TElement ExtractMin();

    /// <summary>
    /// Tries to extract the minimum element from the queue and returns its value.
    /// </summary>
    /// <param name="element">The element with the minimum value.</param>
    /// <returns><i>true</i>, if the queue had at least one element, else <i>false</i></returns>
    bool TryExtractMin([MaybeNullWhen(false)] out TElement element);

    /// <summary>
    /// Decreases the value of an element specified by its handle to a new (lesser) value.
    /// </summary>
    /// <param name="handle">The handle of the element to decrease</param>
    /// <param name="newElement">The new (lesser) value of the element at <paramref name="handle"/></param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="handle"/> is no valid handle of the priority queue or
    /// <paramref name="newElement"/> doesnt decrease the element but is greater than the previous value at this handle.
    /// </exception>
    void DecreaseElement(THandle handle, TElement newElement);

    /// <summary>
    /// Inserts an element into the priority queue and returns its handle.
    /// </summary>
    /// <param name="element">Value to insert into the priority queue.</param>
    /// <returns>Handle of the new element in the queue.</returns>
    THandle Insert(TElement element);

    /// <summary>
    /// Inserts a new element into the queue and extracts the new minimum from the queue.
    /// </summary>
    /// <param name="element">The new value to insert into the priority queue</param>
    /// <returns>the minimum value of all values in the queue and the new element</returns>
    /// <remarks>
    /// Depending on the implementation of the priority queue this can be more efficient than calling <see cref="Insert(TElement)"/> and
    /// <see cref="ExtractMin"/> after another.
    /// </remarks>
    TElement InsertAndExtractMin(TElement element);

    /// <summary>
    /// Removes all elements from the priority queue.
    /// </summary>
    void Clear();
}
