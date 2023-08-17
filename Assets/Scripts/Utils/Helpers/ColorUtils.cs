using UnityEngine;

public abstract class ColorUtils
{
    public static Color SetAlpha(Color color, float newAlpha)
    {
        color.a = newAlpha;
        return color;
    }
}
