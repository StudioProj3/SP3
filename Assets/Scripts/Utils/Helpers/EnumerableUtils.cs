using System.Linq;
using System.Collections.Generic;

public static class EnumerableUtils
{
    // Determines whether the collection is null or contains no elements.
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null || !enumerable.Any(); 
    }
}
