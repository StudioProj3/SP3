using UnityEngine;

public static class VectorUtils
{
    public static Vector2 Set(this Vector2 vector2,
        float x = float.MaxValue, float y = float.MaxValue)
    {
        vector2 = new(x == float.MaxValue ? vector2.x : x,
            y == float.MaxValue ? vector2.y : y);

        return vector2;
    }

    public static Vector3 Set(this Vector3 vector3,
        float x = float.MaxValue, float y = float.MaxValue,
        float z = float.MaxValue)
    {
        vector3 = new(x == float.MaxValue ? vector3.x : x,
            y == float.MaxValue ? vector3.y : y,
            z == float.MaxValue ? vector3.z : z);

        return vector3;
    }
}
