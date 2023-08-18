using UnityEngine;

using static DebugUtils;

public static class RendererUtils
{
    public static void SetAlpha(this Renderer renderer,
        float newAlpha)
    {
        if (renderer is SpriteRenderer spriteRenderer)
        {
            spriteRenderer.color =
                spriteRenderer.color.Set(a: newAlpha);
        }
        else if (renderer is MeshRenderer meshRenderer)
        {
            meshRenderer.material.color =
                meshRenderer.material.color.Set(a: newAlpha);
        }
        else
        {
            Fatal("Unhandled renderer type");
        }
    }
}
