namespace matthiasffm.Common;

/// <summary>
/// defines some basic generic delegates
/// </summary>
public static class Functions
{
    /// <summary>
    /// identity as a function
    /// </summary>
    public static T Identity<T>(T val) => val;

    /// <summary>
    /// default of a type as a function
    /// </summary>
    public static T Zero<T>() where T: notnull => default!;
}
