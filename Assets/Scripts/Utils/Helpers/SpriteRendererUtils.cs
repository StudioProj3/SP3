using UnityEngine;

public static class SpriteRendererUtils
{
    public static void SetAlpha(this SpriteRenderer spriteRenderer,
        float newAlpha)
    {
        Color newColor = spriteRenderer.color;
        newColor.a = newAlpha;

        spriteRenderer.color = newColor;
    }
}
