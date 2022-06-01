namespace matthiasffm.Common;

public static class EnumerableExtensions
{
    /// <summary>
    /// Erlaubt es statt collection.Contains(item) einfacher item.In(collection) zu schreiben.
    /// Damit sieht eine Linq-Query dem gewünschten generierten SQL ähnlicher.
    /// </summary>
    public static bool In<T>(this T subject, IEnumerable<T> items)
    {
        return items.Contains(subject);
    }

    /// <summary>
    /// Erlaubt es statt collection.Contains(item) einfacher item.In(collection) zu schreiben.
    /// Abfragen folgender Natur sind damit einfacher schreib- und lesbar
    /// if(name.In("name1", "name2", "name3"))
    /// { ... }
    /// </summary>
    public static bool In<T>(this T subject, params T[] items)
    {
        return items.Contains(subject);
    }

    /// <summary>
    /// Stellt die AddRange-Methode aus List auch für andere Collections bereit.
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

    /// <summary>Liefert einen Iterator über ganze Zahlen von start bis end</summary>
    /// <remarks>
    /// Ähnlich Enumerable.Range erstellt diese Methode einen Zahlen-Iterator. Range kann aber kein
    /// Intervall von a bis b direkt iterieren, sondern nur von a bis a+count. Mit dieser Erweiterung
    /// kann der Nutzer a und b direkt ohne erst die Differenz berechnen zu müssen angeben.
    /// </remarks>
    public static IEnumerable<int> To(this int start, int end)
    {
        int i    = start;
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

    /// <summary>Liefert einen Iterator über ganze Zahlen von start bis end</summary>
    /// <remarks>
    /// Ähnlich Enumerable.Range erstellt diese Methode einen Zahlen-Iterator. Range kann aber kein
    /// Intervall von a bis b direkt iterieren, sondern nur von a bis a+count. Mit dieser Erweiterung
    /// kann der Nutzer a und b direkt ohne erst die Differenz berechnen zu müssen angeben.
    /// </remarks>
    public static IEnumerable<long> To(this long start, long end)
    {
        long i    = start;
        long diff = System.Math.Sign(end - start);

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

    /// <summary>Rotiert die Aufzählung nach links um count Elemente. Links herausgeschiftete Elemente werden rechts eingefügt</summary>
    /// <remarks>
    /// |0|1|2|3|4|5|                          |0|1|2|3|4|5|
    /// |-|-|-|-|-|-|   => RotateLeft(2):      |-|-|-|-|-|-|
    /// |a|b|c|c|e|f|          |a|b|           |c|c|e|f|a|b|
    /// </remarks>
    public static IEnumerable<T> RotateLeft<T>(this IEnumerable<T> coll, int count = 1)
    {
        return coll.Skip(count).Concat(coll.Take(count));
    }

    /// <summary>Rotiert die Aufzählung nach rechts um count Elemente. Links herausgeschiftete Elemente werden rechts eingefügt</summary>
    /// <remarks>
    /// |0|1|2|3|4|5|                          |0|1|2|3|4|5|
    /// |-|-|-|-|-|-|   => RotateRight(2):     |-|-|-|-|-|-|
    /// |a|b|c|c|e|f|          |e|f|           |e|f|a|b|c|c|
    /// </remarks>
    public static IEnumerable<T> RotateRight<T>(this IEnumerable<T> coll, int count = 1)
    {
        return coll.TakeLast(count).Concat(coll.SkipLast(count));
    }

    /// <summary>Liefert die Menge aller (direkt bei step=1) aufeinanderfolgenden Elementpaare aus coll.</summary>
    public static IEnumerable<(T, T)> Pairs<T>(this IEnumerable<T> coll, int step = 1)
        => coll.Zip(coll.Skip(step));

    /// <summary>Liefert die Menge aller möglichen Kombinationen von Elementpaaren aus coll (ohne Wiederholung).</summary>
    public static IEnumerable<(T, T)> Variations<T>(this IEnumerable<T> coll)
        => coll.SelectMany((elem1, i) => coll.Where((elem2, j) => j != i)
                                             .Select(elem2 => (elem1, elem2)));
}
