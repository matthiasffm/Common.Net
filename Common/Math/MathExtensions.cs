namespace matthiasffm.Common.Math;

/// <summary>
/// Stellt Erweiterungsmethoen für mathematische Basisfunktionen bereit.
/// </summary>
public static class MathExtensions
{
    /// <summary>Liefert die Summe aller natürlichen Zahlen von 1 ... n in O(1)</summary>
    public static int SumNumbers(int n) => n * (n + 1) / 2; // Gaußsche Summenformel

    /// <summary>
    /// Ermittelt ob eine Zahl im Intervall von min bis max liegt.
    /// </summary>
    public static bool Between(this int val, int min, int max) => min <= val && val <= max;

    /// <summary>
    /// Ermittelt ob eine Zahl im Intervall von min bis max liegt.
    /// </summary>
    public static bool Between(this long val, long min, long max) => min <= val && val <= max;
}
