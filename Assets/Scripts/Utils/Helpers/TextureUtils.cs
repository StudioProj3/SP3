using UnityEngine;

public static class TextureUtils
{
    public static Texture2D GenPixel(Color color)
    {
        Texture2D texture2d = new(1, 1);
        texture2d.SetPixel(1, 1, color);
        texture2d.Apply();

        return texture2d;
    }
}
