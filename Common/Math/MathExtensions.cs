namespace matthiasffm.Common.Math;

public static class MathExtensions
{
    /// <summary>Liefert die Summe aller natürlichen Zahlen von 1 ... n in O(1)</summary>
    public static int SumNumbers(int n) => n * (n + 1) / 2; // Gaußsche Summenformel

    public static bool Between(this int val, int min, int max) => min <= val && val <= max;

    public static bool Between(this long val, long min, long max) => min <= val && val <= max;
}
