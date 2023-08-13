using System.Collections.Generic;
using System.Linq;

public static class ExtensionMethods
{
    #region Public

    /// Determines whether the collection is null or contains no elements.
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null || !enumerable.Any(); 
    }

    #endregion
}