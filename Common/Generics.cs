using System.Collections.Generic;
using System.Linq;

namespace Common;

public static class GenericsExtensions
{
    /// <summary>
    /// Erlaubt es statt collection.Contains(item) einfacher item.In(collection) zu schreiben.
    /// Damit sieht eine Linq-Query dem gewünschten generierten SQL ähnlicher.
    /// </summary>
    public static bool In<T> (this T subject, IEnumerable<T> items)
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
        foreach(var elem in toAdd)
        {
            coll.Add(elem);
        }
    }
}
