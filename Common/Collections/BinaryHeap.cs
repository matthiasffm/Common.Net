using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using static matthiasffm.Common.Algorithms.Basics;

namespace matthiasffm.Common.Collections;

/// <summary>
/// Handle type for referencing elements of a binary heap
/// </summary>
public abstract class Handle { }

/// <summary>
/// Implements a mininum heap in the form of a complete binary tree where every
/// parent has a smaller or equal value than its children (heap property).
/// </summary>
public class BinaryHeap<TElement> : IPriorityQueue<TElement, Handle>
{
    /// <summary>
    /// Handle type for elements in a Binary Heap.
    /// </summary>
    private sealed class BinaryHeapHandle : Handle
    {
        internal TElement _element;
        internal int      _heapIndex;

        internal BinaryHeapHandle(TElement element, int heapIndex)
        {
            _element   = element;
            _heapIndex = heapIndex;
        }
    }

    private readonly IComparer<TElement> _comparer;

    private BinaryHeapHandle[] _elems;
    private int _last = -1;

    /// <summary>
    /// Creates a new binary heap with the specified <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity">reserved memory for the specified number of elements</param>
    /// <remarks>Uses the default comparer for 'less than' comparisons of <typeparamref name="TElement"/> instances.</remarks>
    public BinaryHeap(int capacity = 10)
    {
        if(capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        _elems = new BinaryHeapHandle[capacity];

        _comparer = Comparer<TElement>.Default;
    }

    /// <summary>
    /// Creates a new binary heap with the specified <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity">reserved memory for the specified number of elements</param>
    /// <param name="comparer">The comparer to use for 'less than' comparisons of <typeparamref name="TElement"/> instances.</param>
    public BinaryHeap(int capacity, IComparer<TElement> comparer)
    {
        if(capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        _elems = new BinaryHeapHandle[capacity];

        _comparer = comparer;
    }

    /// <summary>
    /// Creates a new binary heap containing all <paramref name="items"/>.
    /// </summary>
    /// <param name="items">Elements of the new binary heap.</param>
    /// <remarks>
    /// It takes O(n) comparisons to build the binary heap from the input collection.
    /// Uses the default comparer for 'less than' comparisons of <typeparamref name="TElement"/> instances.
    /// </remarks>
    public BinaryHeap(IEnumerable<TElement> items)
    {
        _elems = items.Select((elem, idx) => new BinaryHeapHandle(elem, idx))
                      .ToArray();

        _last     = _elems.Length - 1;
        _comparer = Comparer<TElement>.Default;

        BuildHeap();
    }

    /// <summary>
    /// Creates a new binary heap containing all <paramref name="items"/>.
    /// </summary>
    /// <param name="items">Elements of the new binary heap.</param>
    /// <param name="comparer">The comparer to use for 'less than' comparisons of <typeparamref name="TElement"/> instances.</param>
    /// <remarks>
    /// It takes O(n) comparisons to build the binary heap from the input collection.
    /// </remarks>
    public BinaryHeap(IEnumerable<TElement> items, IComparer<TElement> comparer)
    {
        _elems = items.Select((elem, idx) => new BinaryHeapHandle(elem, idx))
                      .ToArray();

        _last = _elems.Length - 1;
        _comparer = comparer;

        BuildHeap();
    }

    /// <see cref="IPriorityQueue{TElement, THandle}.Count"/>
    /// <remarks>Returns the number of elements in the binary heap in O(1) steps.</remarks>
    public int Count => _last + 1;

    /// <see cref="IPriorityQueue{TElement, THandle}.Contains(TElement, out THandle)"/>
    /// <remarks>Searches the heap in O(n) steps.</remarks>
    public bool Contains(TElement toFind, [MaybeNullWhen(false)] out Handle handle)
    {
        for(int i = 0; i <= _last; i++)
        {
            if(object.Equals(_elems[i]._element, toFind))
            {
                handle = _elems[i];
                return true;
            }
        }

        handle = default;
        return false;
    }

    /// <see cref="IPriorityQueue{TElement, THandle}.Min"/>
    /// <remarks>Returns the minimum element in the binary heap in O(1) steps.</remarks>
    public TElement Min
    {
        get
        {
            if(_last >= 0)
            {
                return _elems[0]._element;
            }
            else
            {
                throw new InvalidOperationException("The binary heap is empty.");
            }
        }
    }
 
    /// <see cref="IPriorityQueue{TElement, THandle}.ExtractMin"/>
    /// <remarks>Removes and returns the minimum element from the binary heap in O(log n) steps.</remarks>
    public TElement ExtractMin()
    {
        if(_last >= 0)
        {
            var min = _elems[0]._element;

            SwapHeapNodes(0, _last);
            _last--;

            if(_last > 0)
            {
                Heapify(0);
            }

            CheckInvariant();

            return min;
        }
        else
        {
            throw new InvalidOperationException("The binary heap is empty.");
        }
    }

    /// <see cref="IPriorityQueue{TElement, THandle}.TryExtractMin(out TElement)"/>
    /// <remarks>Tries to removes and return the minimum element from the binary heap in O(log n) steps.</remarks>
    public bool TryExtractMin([MaybeNullWhen(false)] out TElement element)
    {
        if(_last >= 0)
        {
            element = ExtractMin();
            return true;
        }
        else
        {
            element = default;
            return false;
        }
    }

    /// <see cref="IPriorityQueue{TElement, THandle}.TryExtractMin(out TElement)"/>
    /// <remarks>Decreases the value of one element to a lesser value in O(log n) comparison steps.</remarks>
    public void DecreaseElement(Handle? handle, TElement newElement)
    {
        BinaryHeapHandle? h = handle as BinaryHeapHandle;
        if(h is not null && h._heapIndex >= 0 && h._heapIndex <= _last)
        {
            if(_comparer.Compare(newElement, _elems[h._heapIndex]._element) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newElement));
            }

            DecreaseInternal(h._heapIndex, newElement);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(handle));
        }
    }

    /// <see cref="IPriorityQueue{TElement, THandle}.Insert(TElement)"/>
    /// <remarks>
    /// Inserts one new element to the binary heap in O(log n) comparison steps or
    /// O(n) copy steps if the heap has insufficent size and has to be reallocated.
    /// </remarks>
    public Handle Insert(TElement element)
    {
        ResizeInternalHeap(_last + 1);

        ++_last;
        _elems[_last] = new BinaryHeapHandle(element, _last);
        return DecreaseInternal(_last, element);
    }

    /// <see cref="IPriorityQueue{TElement, THandle}.InsertAndExtractMin(TElement)"/>
    /// <remarks>
    /// More efficient implementation than calling Insert + ExtractMin separately by
    /// evaluating if the new element is the new min. In this case the operation returns in O(1).
    /// In other cases this method runs in O(logn) like DecreaseElement.
    /// </remarks>
    public TElement InsertAndExtractMin(TElement element)
    {
        if((_last < 0) || _comparer.Compare(_elems[0]._element, element) >= 0)
        {
            return element;
        }
        else
        {
            var min = _elems[0]._element;

            _elems[0] = new BinaryHeapHandle(element, 0);
            Heapify(0);

            CheckInvariant();

            return min;
        }
    }

    /// <see cref="IPriorityQueue{TElement, THandle}.Clear"/>
    /// <remarks>Clears the binary heap in O(1). The underlying memory is _not_ freed.</remarks>
    public void Clear()
    {
        _last = -1;
    }

    private void SwapHeapNodes(int left, int right)
    {
        Swap(ref _elems[left], ref _elems[right]);
        (_elems[left]._heapIndex, _elems[right]._heapIndex) = (_elems[right]._heapIndex, _elems[left]._heapIndex);
    }

    private BinaryHeapHandle DecreaseInternal(int idx, TElement newElem)
    {
        _elems[idx]._element = newElem;

        while(idx > 0 &&
              Parent(idx) is var parent &&
              _comparer.Compare(_elems[parent]._element, _elems[idx]._element) > 0)
        {
            SwapHeapNodes(parent, idx);
            idx = parent;
        }

        CheckInvariant();

        return _elems[idx];
    }

    // parent index
    private static int Parent(int idx) => (idx + 1) / 2 - 1;
    // index of left child
    private static int Left(int idx)   => idx * 2 + 1;
    // index of right child
    private static int Right(int idx)  => idx * 2 + 2;

    /// <summary>
    /// Maintains the minimum heap property after node i was changed
    /// starts at node i and swaps child nodes so long til the min heap property is restored
    /// </summary>
    /// <remarks>runs in O(log n)</remarks>
    private void Heapify(int i)
    {
        int smallest;

        var left = Left(i);
        if((left <= _last) && (_comparer.Compare(_elems[left]._element, _elems[i]._element) < 0))
        {
            smallest = left;
        }
        else
        {
            smallest = i;
        }

        var right = Right(i);
        if((right <= _last) && (_comparer.Compare(_elems[right]._element, _elems[smallest]._element) < 0))
        {
            smallest = right;
        }

        if(smallest != i)
        {
            SwapHeapNodes(i, smallest);
            Heapify(smallest);
        }

        CheckInvariant(i);
    }

    /// <summary>
    /// Reorders all items in _elems so that the resulting array is a minimum heap.
    /// To do this just ensure the heap property from the bottom up to the top.
    /// </summary>
    /// <remarks>runs in O(n)</remarks>
    private void BuildHeap()
    {
        for(int i = _last / 2; i >= 0; i--)
        {
            Heapify(i);
        }

        CheckInvariant();
    }

    private void ResizeInternalHeap(int newCapacity)
    {
        if(newCapacity >= _elems.Length)
        {
            var newArray = new BinaryHeapHandle[System.Math.Max(newCapacity, _elems.Length * 2)];
            _elems.CopyTo(newArray, 0);
            _elems = newArray;

            CheckInvariant();
        }
    }

    /// <summary>
    /// Checks the heap property for all elements of the binary tree starting at index <paramref name="idx"/>.
    /// </summary>
    /// <remarks>
    /// Heap property: parent element is less than the left child element and
    ///                parent element is less than the right child element
    /// </remarks>
    /// <param name="idx">idx = 0 means the whole heap will be checked</param>
    [Conditional("DEBUG")]
    private void CheckInvariant(int idx = 0)
    {
        if(idx <= _last)
        {
            // check min heap property (parent smaller than children)

            var parent = _elems[idx];

            if(Left(idx) <= _last)
            {
                Debug.Assert(_comparer.Compare(parent._element, _elems[Left(idx)]._element) <= 0);
                CheckInvariant(Left(idx));
            }

            if(Right(idx) <= _last)
            {
                Debug.Assert(_comparer.Compare(parent._element, _elems[Right(idx)]._element) <= 0);
                CheckInvariant(Right(idx));
            }

            // check handle integrity

            Debug.Assert(_elems[idx]._heapIndex >= 0);
            Debug.Assert(_elems[idx]._heapIndex <= _last);
        }
    }
}
