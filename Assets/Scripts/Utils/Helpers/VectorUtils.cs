using UnityEngine;

public static class VectorUtils
{
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
