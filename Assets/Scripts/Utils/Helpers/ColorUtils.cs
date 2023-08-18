using UnityEngine;

public static class ColorUtils
{
    public static Color Set(this Color color,
        float r = float.MaxValue, float g = float.MaxValue,
        float b = float.MaxValue, float a = float.MaxValue)
    {
        color = new(r.IsMax() ? color.r : r,
            g.IsMax() ? color.g : g,
            b.IsMax() ? color.b : b,
            a.IsMax() ? color.a : a);

        return color;
    }
}
