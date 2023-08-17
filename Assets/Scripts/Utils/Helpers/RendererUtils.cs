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
                spriteRenderer.color.SetAlpha(newAlpha);
        }
        else if (renderer is MeshRenderer meshRenderer)
        {
            meshRenderer.material.color =
                meshRenderer.material.color.SetAlpha(newAlpha);
        }
        else
        {
            Fatal("Unhandled renderer type");
        }
    }
}
