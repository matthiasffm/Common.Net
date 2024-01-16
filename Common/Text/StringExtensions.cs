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
        // - swap chars in left and right
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

        // works similar to Levenshteins algorithm by essentially computing a matrix MxN of character
        // edits where every position is a min of the 4 edits
        // - delete char from left char
        // - delete char from right string
        // - swap chars in left and right
        // - transpose of chars in left and right (transpose is an additional edit in comparison to Levenshteins algorithm)
        // the end result for minimum numbers of character edits is then the bottom right cell in the matrix

        // to reduce the memory footprint this method only keeps the last three rows of the matrix in memory
        // this is possible because to compute a value in the matrix one needs only the values directly to
        // the left two rows, the top and the top left of this value.
        // one additional row is needed in comparison to Levenshteins algorithm for calculation the transpose value

        var d1 = new int[left.Length + 1];
        var d2 = new int[left.Length + 1];
        var d3 = new int[left.Length + 1];

        for(var i = 0; i <= left.Length; i++)
        {
            d2[i] = i;
        }

        for(var j = 1; j <= right.Length; j++)
        {
            d3[0] = j;

            for(var i = 1; i <= left.Length; i++)
            {
                var swapCost = (left[i - 1] == right[j - 1]) ? 0 : 1;

                d3[i] = Min(d3[i - 1] + 1,         // remove a char from left string
                            d2[i] + 1,             // remove a char from right string
                            d2[i - 1] + swapCost); // swap chars from left and right

                // transpose chars from left and right if iteration is at least in third row of MxN matrix
                if(i >= 2 && j >= 2 && left[i - 1] == right[j - 2] && left[i - 2] == right[j - 1])
                {
                    d3[i] = System.Math.Min(d3[i], d1[i - 2] + 1);
                }
            }

            (d1, d2) = (d2, d1);
            (d2, d3) = (d3, d2);
        }

        return d2[left.Length];
    }

    private static int Min(int a, int b, int c)
    {
        return System.Math.Min(a, System.Math.Min(b, c));
    }
}
