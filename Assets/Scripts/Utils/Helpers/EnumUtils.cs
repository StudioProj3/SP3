using System;

public static class EnumUtils
{
    // Returns number of values in an `Enum` type
    public static int Count(Type type)
    {
        return Enum.GetValues(type).Length;
    }

    // Returns number of values in an `Enum` type from
    // an instance
    public static int Count<T>(this T var) where T : Enum
    {
        return Count(var.GetType());
    }

    // Returns the enum in string form but all lowercase
    public static string ToLower<T>(this T var) where T : Enum
    {
        return var.ToString().ToLower();
    }
}
