namespace matthiasffm.Common;

public static class Functions
{
    public static T Identity<T>(T val) => val;
    public static T Zero<T>() where T: notnull => default!;
}
