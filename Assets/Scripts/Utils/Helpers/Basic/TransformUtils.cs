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

    public static GameObject ChildGO(this Transform transform,
        params int[] indices)
    {
        Transform result = GetChild(transform, indices);

        return result.gameObject;
    }

    public static GameObject Parent(this Transform transform)
    {
        return transform.parent.gameObject;
    }

    // Get the parent recursively with `number` 1 being just
    // `transform.parent` and 2 being `transform.parent.parent`
    // so on and forth
    public static Transform Parent(this Transform transform,
        int number)
    {
        for (int i = 0; i < number; ++i)
        {
            transform = transform.parent;
        }

        return transform;
    }
}
