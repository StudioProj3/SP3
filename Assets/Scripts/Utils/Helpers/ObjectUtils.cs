// Bunch of extension methods for objects
public static class ObjectUtils
{
    public static bool IsDefault<T>(this T obj)
    {
        return Equals(obj, default(T));
    }
}
