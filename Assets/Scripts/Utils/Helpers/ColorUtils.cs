using UnityEngine;

public static class ColorUtils
{
    public static Color SetAlpha(this Color color, float newAlpha)
    {
        color.a = newAlpha;
        return color;
    }
}
