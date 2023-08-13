using System;
using System.Collections.Generic;

public static class ListUtils
{
    // Checks that every single `Func<bool>` returns true
    // If the `list` is null or empty, return `null`
    public static bool? AllTrue(this List<Func<bool>> list)
    {
        if (list == null)
        {
            return null;
        }

        foreach (Func<bool> func in list)
        {
            // If any one of the `Func<bool>` is false,
            // return false
            if (!func())
            {
                return false;
            }
        }

        // If none of them are false check the `Count`
        // and return true if it is more than 0, else null
        return list.Count > 0 ? true : null;
    }
}
