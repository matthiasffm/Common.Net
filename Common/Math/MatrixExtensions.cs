namespace matthiasffm.Common.Math;

#pragma warning disable CA1814 // Matrizen sind geeignet und korrekt als multidimensionales Array

public static class MatrixExtensions
{
    /// <summary>
    /// Mappt jedes Element der Matrix per Select-Functor auf ein TResult.
    /// </summary>
    /// <typeparam name="TSource">Elementtyp der Matrix</typeparam>
    /// <typeparam name="TResult">Ergebnistyp der Selektion</typeparam>
    /// <param name="matrix">Matrix deren Elemente spalten- und dann zeilenweise iteriert werden.</param>
    /// <param name="selector">der Functor mappt einzelne Elemente der Matrix auf den Ergebnistypen</param>
    /// <returns>Menge der Ergebnisdaten in der Reihenfolge spalten- und dann zeilenweise.</returns>
    public static IEnumerable<TResult> Select<TSource, TResult>(this TSource[,] matrix, Func<TSource, int, int, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(matrix);
        ArgumentNullException.ThrowIfNull(selector);

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for(int j = 0; j < matrix.GetLength(1); j++)
            {
                yield return selector(matrix[i, j], i, j);
            }
        }
    }

    /// <summary>
    /// Führt auf jedem Element der Matrix eine Methode aus.
    /// </summary>
    /// <typeparam name="TSource">Elementtyp der Matrix</typeparam>
    /// <param name="matrix">Matrix deren Elemente spalten- und dann zeilenweise iteriert werden.</param>
    /// <param name="selector">die Methode wird für jeden Elementtyp mit dem Element, der Zeile und dann der Spalte aufgerufen.</param>
    public static void ForEach<TSource>(this TSource[,] matrix, Action<TSource, int, int> action)
    {
        ArgumentNullException.ThrowIfNull(matrix);
        ArgumentNullException.ThrowIfNull(action);

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for(int j = 0; j < matrix.GetLength(1); j++)
            {
                action(matrix[i, j], i, j);
            }
        }
    }

    /// <summary>
    /// Füllt die Matrix per Functor mit Werten.
    /// </summary>
    /// <typeparam name="TSource">Elementtyp der Matrix</typeparam>
    /// <param name="matrix">Matrix deren Elemente spalten- und dann zeilenweise iteriert werden.</param>
    /// <param name="selector">der Functor erzeugt den Wert für eine Zeile und Spalte</param>
    public static void Populate<TSource>(this TSource[,] matrix, Func<int, int, TSource> func)
    {
        ArgumentNullException.ThrowIfNull(matrix);
        ArgumentNullException.ThrowIfNull(func);

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for(int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = func(i, j);
            }
        }
    }

    /// <summary>
    /// Zählt alle Elemente der Matrix die eine Bedingung erfüllen.
    /// </summary>
    /// <typeparam name="TSource">Elementtyp der Matrix</typeparam>
    /// <param name="matrix">Matrix deren Elemente spalten- und dann zeilenweise iteriert werden.</param>
    /// <param name="predicate">die Entscheidungsfunktion wird für jedes Element der Matrix aufgerufen</param>
    /// <returns>Anzahl an Elementen in der Matrix, für die <paramref name="predicate"/> <i>true</i> liefert.</returns>
    public static int Count<TSource>(this TSource[,] matrix, Func<TSource, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(matrix);
        ArgumentNullException.ThrowIfNull(predicate);

        var count = 0;

        for(int i = 0; i < matrix.GetLength(0); i++)
        {
            for(int j = 0; j < matrix.GetLength(1); j++)
            {
                if(predicate(matrix[i, j]))
                {
                    count++;
                }
            }
        }

        return count;
    }

    /// <summary>
    /// Konvertiert ein Matrix in der Form Array of Arrays in eine Matrix als multidimensionales Array.
    /// </summary>
    /// <typeparam name="TSource">Elementtyp der Matrix</typeparam>
    /// <param name="input">Die Matrix als Array of Arrays.</param>
    /// <returns>eine Matrix als multidimensionales Array mit den gleichen Dimensionen wie <paramref name="input"/>.</returns>
    public static TSource[,] ConvertToMatrix<TSource>(this TSource[][] input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var res = new TSource[input.Length, input[0].Length];

        for(int i = 0; i < res.GetLength(0); i++)
        {
            for(int j = 0; j < res.GetLength(1); j++)
            {
                res[i, j] = input[i][j];
            }
        }

        return res;
    }

    /// <summary>
    /// Vergleicht zwei Matrizen auf Gleichheit in Dimension und Inhalt.
    /// </summary>
    /// <typeparam name="TSource">Elementtyp der Matrix</typeparam>
    /// <param name="left">Matrix 1</param>
    /// <param name="right">Matrix 2</param>
    /// <returns>
    /// <i>true</i>, wenn sich die Matrizen weder in Dimension noch in Inhalt unterscheiden.
    /// <i>false</i>, sonst.
    /// </returns>
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

        for(int i = 0; i < left.GetLength(0); i++)
        {
            for(int j = 0; j < left.GetLength(1); j++)
            {
                if(!EqualityComparer<TSource>.Default.Equals(left[i, j], right[i, j]))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Aggregiert alle Elemente einer Matrix per Aggregat-Funktion
    /// </summary>
    /// <typeparam name="TSource">Elementtyp der Matrix</typeparam>
    /// <typeparam name="TResult">Aggregatstyp</typeparam>
    /// <param name="matrix">Matrix mit den spalten- und dann zeilenweisen zu aggregierenden Elementen</param>
    /// <param name="seed">Startzustand der Aggregation</param>
    /// <param name="aggregateFunc">Aggregat-Funktion die elementweise die Matrix auf-aggregiert</param>
    /// <returns>Ergebnis der Aggregation</returns>
    public static TResult Aggregate<TSource, TResult>(this TSource[,] matrix, TResult seed, Func<TResult, TSource, TResult> aggregateFunc)
    {
        ArgumentNullException.ThrowIfNull(matrix);
        ArgumentNullException.ThrowIfNull(aggregateFunc);

        TResult aggregate = seed;

        for(int i = 0; i < matrix.GetLength(0); i++)
        {
            for(int j = 0; j < matrix.GetLength(1); j++)
            {
                aggregate = aggregateFunc(aggregate, matrix[i, j]);
            }
        }

        return aggregate;
    }
}
