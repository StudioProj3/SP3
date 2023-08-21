using System;

using UnityEngine;

public static class TransformUtils
{
    public static Transform GetChild(this Transform transform,
        params int[] indices)
    {
        if (indices.Length == 0)
        {
            throw new ArgumentException("`indices` cannot be empty");
        }

        for (int i = 0; i < indices.Length; ++i)
        {
            transform = transform.GetChild(indices[i]);
        }

        return transform;
    }
}
