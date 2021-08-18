using System.Globalization;
using System.Text;

namespace matthiasffm.Common;

public static class StringExtensions
{
    /// <summary>
    /// Entfernt Akzente aus einzelnen Zeichen. Aus ä wird a, aus ć wird c, aus Š wird S und aus ž wird z.
    /// </summary>
    public static string RemoveAccents(this string s)
    {
        var result = new StringBuilder();

        foreach(var c in s.Normalize(NormalizationForm.FormD))
        {
            if(CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                result.Append(c);
            }
        }

        return result.ToString()
                     .Normalize(NormalizationForm.FormC);
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

    /// <summary>
    /// Ermittelt die Levenshtein-Distanz zwischen 2 Zeichenketten.
    /// </summary>
    /// <remarks>
    /// siehe https://en.wikipedia.org/wiki/Levenshtein_distance
    /// </remarks>
    public static int Levenshtein(this string left, string right)
    {
        if(left.Length == 0 || right.Length == 0)
        {
            return Math.Max(left.Length, right.Length);
        }

        if(left[0] == right[0])
        {
            return Levenshtein(left[1..], right[1..]);
        }
        else
        {
            return 1 + Min(Levenshtein(left[1..], right),
                           Levenshtein(left, right[1..]),
                           Levenshtein(left[1..], right[1..]));
        }
    }


    public static int OptimalStringAlignemnt(this string left, string right)
    {
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
            min = Math.Min(min, OsaDistance(left, right, i - 1, j - 1) + (left[i - 1] != right[j - 1] ? 1 : 0));
        }

        if(i > 1 && j > 1 && left[i - 1] == right[j - 2] && left[i - 2] == right[j - 1])
        {
            min = Math.Min(min, OsaDistance(left, right, i - 2, j - 2) + 1);
        }

        return min;
    }

    private static int Min(int a, int b, int c)
    {
        return Math.Min(a, Math.Min(b, c));
    }
}
