using UnityEngine;

using static DebugUtils;

public static class RendererUtils
{
    public static void SetAlpha(this Renderer renderer,
        float newAlpha)
    {
        if (renderer is SpriteRenderer spriteRenderer)
        {
            Color newColor = spriteRenderer.color;
            newColor.a = newAlpha;

            spriteRenderer.color = newColor;
        }
        else if (renderer is MeshRenderer meshRenderer)
        {
            Color newColor = meshRenderer.material.color;
            newColor.a = newAlpha;

            meshRenderer.material.color = newColor;
        }
        else
        {
            Fatal("Unhandled renderer type");
        }
    }
}
