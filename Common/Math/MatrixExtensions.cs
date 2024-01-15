namespace matthiasffm.Common.Math;

/// <summary>
/// Provides extension methods for using 2 dimensional arrays of the form [m, n].
/// </summary>
public static class MatrixExtensions
{
#pragma warning disable CA1814 // its a basic matrix, no space is wasted here by definition

    /// <summary>
    /// Maps every element of a matrix to a result value and provides an enumeration over these values in column and then row order.
    /// </summary>
    /// <typeparam name="TSource">Element type of the [m, n] matrix</typeparam>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <param name="matrix">the source [m, n] matrix</param>
    /// <param name="selector">
    /// A transform function to map each source element to a TResult value;
    /// the second parameter of the function represents the row index of the source element.
    /// the third parameter of the function represents the column index of the source element.
    /// </param>
    /// <returns>Enumeration of all mapped values of the [m, n] matrix in column and then row order.</returns>
    public static IEnumerable<TResult> Select<TSource, TResult>(this TSource[,] matrix, Func<TSource, int, int, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(matrix);
        ArgumentNullException.ThrowIfNull(selector);

        for(int row = 0; row < matrix.GetLength(0); row++)
        {
            for(int col = 0; col < matrix.GetLength(1); col++)
            {
                yield return selector(matrix[row, col], row, col);
            }
        }
    }

    /// <summary>
    /// Applies a function to every element of a matrix.
    /// </summary>
    /// <typeparam name="TSource">Element type of the [m, n] matrix</typeparam>
    /// <param name="matrix">the source [m, n] matrix</param>
    /// <param name="action">
    /// This function is applied to every element of the matrix;
    /// the second parameter of the function represents the row index of the source element.
    /// the third parameter of the function represents the column index of the source element.
    /// </param>
    public static void ForEach<TSource>(this TSource[,] matrix, Action<TSource, int, int> action)
    {
        ArgumentNullException.ThrowIfNull(matrix);
        ArgumentNullException.ThrowIfNull(action);

        for(int row = 0; row < matrix.GetLength(0); row++)
        {
            for(int col = 0; col < matrix.GetLength(1); col++)
            {
                action(matrix[row, col], row, col);
            }
        }
    }

    /// <summary>
    /// Fills a matrix with values provided by a function.
    /// </summary>
    /// <typeparam name="TSource">Element type of the [m, n] matrix</typeparam>
    /// <param name="matrix">the source [m, n] matrix</param>
    /// <param name="func">
    /// This function is called for every position in the matrix and the result values are written to the matrix elements;
    /// the first parameter of the function represents the row index of the source element.
    /// the second parameter of the function represents the column index of the source element.
    /// </param>
    public static void Populate<TSource>(this TSource[,] matrix, Func<int, int, TSource> func)
    {
        ArgumentNullException.ThrowIfNull(matrix);
        ArgumentNullException.ThrowIfNull(func);

        for(int row = 0; row < matrix.GetLength(0); row++)
        {
            for(int col = 0; col < matrix.GetLength(1); col++)
            {
                matrix[row, col] = func(row, col);
            }
        }
    }

    /// <summary>
    /// Counts all elements of the matrix satisfying the condition specified in <i>predicate</i>.
    /// </summary>
    /// <typeparam name="TSource">Element type of the [m, n] matrix</typeparam>
    /// <param name="matrix">the source [m, n] matrix</param>
    /// <param name="predicate">
    /// This function is called for every element in the matrix and if the predicate function returns true the count is raised by 1;
    /// </param>
    /// <returns>Number of elements in the matrix fulfilling <paramref name="predicate"/>.</returns>
    public static int Count<TSource>(this TSource[,] matrix, Func<TSource, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(matrix);
        ArgumentNullException.ThrowIfNull(predicate);

        var count = 0;

        for(int row = 0; row < matrix.GetLength(0); row++)
        {
            for(int col = 0; col < matrix.GetLength(1); col++)
            {
                if(predicate(matrix[row, col]))
                {
                    count++;
                }
            }
        }

        return count;
    }

    /// <summary>
    /// Converts a matrix of the form 'array of arrays' into a [m, n] matrix of the same type.
    /// </summary>
    /// <typeparam name="TSource">Element type of both matrix objects</typeparam>
    /// <param name="input">the source matrix in the form 'array of arrays'</param>
    /// <returns>a [m, n] matrix with the same dimensions and elements as <paramref name="input"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">when not all arrays in the input array have the same length (== row width).</exception>
    public static TSource[,] ConvertToMatrix<TSource>(this TSource[][] input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var cols = input.Length > 0 ? input[0].Length : 0;
        var res = new TSource[input.Length, cols];

        for(int row = 0; row < res.GetLength(0); row++)
        {
            if(input[row].Length != cols)
            {
                throw new ArgumentOutOfRangeException(nameof(input), $"Row {row} has not the same number of elements ({cols}) like reference row 0");
            }
            for(int col = 0; col < res.GetLength(1); col++)
            {
                res[row, col] = input[row][col];
            }
        }

        return res;
    }

    /// <summary>
    /// Compares two matrices for equality in dimensions and content.
    /// </summary>
    /// <typeparam name="TSource">Element type of both matrix objects</typeparam>
    /// <param name="left">first matrix</param>
    /// <param name="right">second matrix</param>
    /// <returns>
    /// <i>true</i> when both matrices have the same dimensions and the same elements.
    /// </returns>
    /// <remarks>
    /// Uses the default EqualityComparer of <typeparamref name="TSource"/> for equality tests.</remarks>
    public static bool SequenceEquals<TSource>(this TSource[,] left, TSource[,] right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        if(left.Rank != right.Rank ||
           left.GetLength(0) != right.GetLength(0) ||
           left.GetLength(1) != right.GetLength(1))
        {
            return false;
        }

        for(int row = 0; row < left.GetLength(0); row++)
        {
            for(int col = 0; col < left.GetLength(1); col++)
            {
                if(!EqualityComparer<TSource>.Default.Equals(left[row, col], right[row, col]))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Applies an accumulator function over all elements of a [m, n] matrix. The specified seed value is
    /// used as the initial accumulator value.
    /// </summary>
    /// <typeparam name="TSource">Element type of the [m, n] matrix</typeparam>
    /// <param name="matrix">the [m, n] matrix with all values to accumulate over   </param>
    /// <typeparam name="TResult">The type of the accumulator value.</typeparam>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="accumulateFunc">An accumulator function to be invoked on each element.</param>
    /// <returns>The final accumulator value</returns>
    public static TResult Aggregate<TSource, TResult>(this TSource[,] matrix, TResult seed, Func<TResult, TSource, TResult> accumulateFunc)
    {
        ArgumentNullException.ThrowIfNull(matrix);
        ArgumentNullException.ThrowIfNull(accumulateFunc);

        TResult accumulator = seed;

        for(int row = 0; row < matrix.GetLength(0); row++)
        {
            for(int col = 0; col < matrix.GetLength(1); col++)
            {
                accumulator = accumulateFunc(accumulator, matrix[row, col]);
            }
        }

        return accumulator;
    }
}
