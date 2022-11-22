using System.Globalization;
using System.Text;

namespace matthiasffm.Common.Text;

/// <summary>
/// Stellt Erweiterungsmethoden für Zeichenketten bereit.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Entfernt Akzente aus einzelnen Zeichen. Aus ä wird a, aus ć wird c, aus Š wird S und aus ž wird z.
    /// </summary>
    public static string RemoveAccents(this string s)
    {
        ArgumentNullException.ThrowIfNull(s);

        return string.Concat(s.Normalize(NormalizationForm.FormD)
                              .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
                     .Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Entfernt Punkte und weitere überflüssige Zeichen aus Namen.
    /// </summary>
    public static string RemovePunctuation(this string s)
    {
        ArgumentNullException.ThrowIfNull(s);

        return s.Replace("'", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace(",", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace(".", string.Empty, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Parst eine Zeichenkette in eine Zahl. Wenn die Zeichenkette ungültig ist oder
    /// keine Umwandlung erlaubt, liefert die Methode den <paramref name="defaultValue"/> zurück.
    /// </summary>
    public static int SafeParseInt(this string s, int defaultValue = 0)
    {
        int val = defaultValue;

        if(!string.IsNullOrWhiteSpace(s))
        {
            int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out val);
        }

        return val;
    }

    // TODO: optimalere Implementierung bereitstellen, die nicht alle Varianten jedesmal komplett
    //       rekursiv in O(3^length) durchrechnet, sondern per dynamischer Programmierung einmal berechnete Distancen
    //       abspeichert und wiederverwendet.

    /// <summary>
    /// Ermittelt die Levenshtein-Distanz zwischen 2 Zeichenketten, also die kleinste Anzahl an gelöschten, eingefügten oder vertauschten
    /// einzelnen Buchstaben, damit die zwei Zeichenketten übereinstimmen.
    /// </summary>
    /// <remarks>
    /// siehe https://en.wikipedia.org/wiki/Levenshtein_distance
    /// </remarks>
    public static int Levenshtein(this string left, string right)
    {
        return LevenshteinSpan(left, right);
    }

    private static int LevenshteinSpan(ReadOnlySpan<char> left, ReadOnlySpan<char> right)
    {
        if(left.Length == 0 || right.Length == 0)
        {
            return System.Math.Max(left.Length, right.Length);
        }

        if(left[0] == right[0])
        {
            return LevenshteinSpan(left[1..], right[1..]);
        }
        else
        {
            return 1 + Min(LevenshteinSpan(left[1..], right),          // Entfernung für erster char in left gelöscht
                           LevenshteinSpan(left, right[1..]),          // Entfernung für erster char in right gelöscht
                           LevenshteinSpan(left[1..], right[1..]));    // Entfernung für erster char in left und right vertauscht
        }
    }

    /// <summary>
    /// Ermittelt die Distanz zwischen 2 Zeichenketten per optimal-string-alignment-Algorithms, der .
    /// </summary>
    /// <remarks>
    /// siehe https://en.wikipedia.org/wiki/Levenshtein_distance
    /// </remarks>
    public static int OptimalStringAlignment(this string left, string right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return OsaDistance(left, right, left.Length, right.Length);
    }

    private static int OsaDistance(string left, string right, int i, int j)
    {
        if(i == 0 && j == 0)
        {
            return 0;
        }

        int min = i > 0 ? OsaDistance(left, right, i - 1, j) + 1 : OsaDistance(left, right, i, j - 1) + 1;

        if(i > 0 && j > 0)
        {
            min = System.Math.Min(min, OsaDistance(left, right, i - 1, j - 1) + (left[i - 1] != right[j - 1] ? 1 : 0));
        }

        if(i > 1 && j > 1 && left[i - 1] == right[j - 2] && left[i - 2] == right[j - 1])
        {
            min = System.Math.Min(min, OsaDistance(left, right, i - 2, j - 2) + 1);
        }

        return min;
    }

    private static int Min(int a, int b, int c)
    {
        return System.Math.Min(a, System.Math.Min(b, c));
    }
}
