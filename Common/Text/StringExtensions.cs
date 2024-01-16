using System.Globalization;
using System.Text;

namespace matthiasffm.Common.Text;

/// <summary>
/// Provides extension methods for analyzing and manipulating strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Removes accents from every character in a string, for instance ä becomes a, ć becomes c, Š becomes S or ž becomes z.
    /// </summary>
    public static string RemoveAccents(this string s)
    {
        ArgumentNullException.ThrowIfNull(s);

        return string.Concat(s.Normalize(NormalizationForm.FormD)
                              .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
                     .Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Removes punctuation characters from a string.
    /// </summary>
    public static string RemovePunctuation(this string s)
    {
        ArgumentNullException.ThrowIfNull(s);

        return s.Replace("'", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace(",", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("!", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("?", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace(".", string.Empty, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Tries to convert the number in a string to an integer.
    /// If no conversion is possible the method returns the <paramref name="defaultValue"/>.
    /// </summary>
    /// <remarks>
    /// No exception is thrown.
    /// </remarks>
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
    /// Computes the Levenshtein distance between two strings as the smallest number of single character edits (deletions, insertions and swaps)
    /// to change one string into the other.
    /// </summary>
    /// <remarks>
    /// see https://en.wikipedia.org/wiki/Levenshtein_distance
    ///
    /// Runtime complexity is O(n²) and memory required is O(n).
    /// </remarks>
    public static int Levenshtein(this string left, string right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        // essentially compute a matrix MxN of character edits where every position is a min of the 3 edits
        // - delete char from left char
        // - delete char from right string
        // - swap chars
        // the end result for minimum numbers of character edits is then the bottom right cell in the matrix
        //
        // here is a matrix for the Levenshtein distance between lawn and flaw:
        //     f l a w
        //   0 1 2 3 4
        // l 1 1 1 2 3
        // a 2 2 2 1 2
        // w 3 3 3 2 1
        // n 4 4 4 3 2 <=

        // to reduce the memory footprint this method only keeps the last two rows of the matrix in memory
        // this is possible because to compute a value in the matrix one needs only the values directly to
        // the left, the top and the top left of this value.

        var d1 = new int[left.Length + 1];
        var d2 = new int[left.Length + 1];

        for(var i = 0; i <= left.Length; i++)
        {
            d1[i] = i;
        }

        for(var j = 1; j <= right.Length; j++)
        {
            d2[0] = j;

            for(var i = 1; i <= left.Length; i++)
            {
                var swapCost = (left[i - 1] == right[j - 1]) ? 0 : 1;

                d2[i] = Min(d2[i - 1] + 1,         // remove a char from left string
                            d1[i] + 1,             // remove a char from right string
                            d1[i - 1] + swapCost); // swap chars from left and right
            }

            (d1, d2) = (d2, d1);
        }

        return d1[left.Length];
    }

    /// <summary>
    /// Computes the distance between two strings by using the Optimal String Alignment algorithm (OSA).
    /// </summary>
    public static int OptimalStringAlignment(this string left, string right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return OsaDistance(left, right, left.Length, right.Length);
    }

    // TODO: replace O(n³) recursive calls with a linear algorithm like for Levenshtein distance
    //       see https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance#Optimal_string_alignment_distance
    private static int OsaDistance(string left, string right, int i, int j)
    {
        if(i == 0 && j == 0)
        {
            return 0;
        }

        int min = int.MaxValue;
        if(i > 0)
        {
            min = System.Math.Min(min, OsaDistance(left, right, i - 1, j) + 1);
        }
        if(j > 0)
        {
            min = System.Math.Min(min, OsaDistance(left, right, i, j - 1) + 1);
        }

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
