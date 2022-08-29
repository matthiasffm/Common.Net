namespace matthiasffm.Common;

/// <summary>
/// Stellt generische Basisfunktionen als einfache Delegates bereit.
/// </summary>
public static class Functions
{
    /// <summary>
    /// Die Identität als Funktion.
    /// </summary>
    public static T Identity<T>(T val) => val;

    /// <summary>
    /// Der Default eines Typs als Funktion.
    /// </summary>
    public static T Zero<T>() where T: notnull => default!;
}
