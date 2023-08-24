using UnityEngine;

public static class SpriteUtils
{
    public static Texture2D GenTexture(this Sprite sprite)
    {
        Texture2D texture = new((int)sprite.textureRect.width,
            (int)sprite.textureRect.height);

        Color[] pixels = sprite.texture.GetPixels(
            (int)sprite.textureRect.x, (int)sprite.textureRect.y,
            (int)sprite.textureRect.width, (int)sprite.textureRect.height);

        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }
}
