using UnityEngine;

public static class SpriteUtils
{
    public static Texture2D GenTexture(this Sprite sprite)
    {
        Texture2D texture = new((int)sprite.rect.width,
            (int)sprite.rect.height);

        Color[] pixels = sprite.texture.GetPixels(
            (int)sprite.textureRect.x, (int)sprite.textureRect.y,
            sprite.texture.width, sprite.texture.height);

        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }
}