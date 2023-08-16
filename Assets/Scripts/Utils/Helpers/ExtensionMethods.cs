using System.Collections.Generic;
using System.Linq;

public static class ExtensionMethods
{
    // Determines whether the collection is null or contains no elements.
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null || !enumerable.Any(); 
    }

    public static bool IsEmpty<T>(this List<T> list)
    {
        return list.Count > 0; 
    }
}