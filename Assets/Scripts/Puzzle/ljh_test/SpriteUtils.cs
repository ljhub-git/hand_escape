using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SpriteUtils //sprite 정보를 받아와서 읽고 만듬
{
    public static Sprite CreateSpriteFromTexture2D(
        Texture2D spriteTexture,
        int x,
        int y,
        int w, 
        int h,
        float pixelsPerUnit = 1.0f,
        SpriteMeshType spriteType = SpriteMeshType.Tight)
    {
        Sprite newSprite = Sprite.Create(
            spriteTexture,
            new Rect(x, y, w, h),
            new Vector2(0, 0),
            pixelsPerUnit,
            0,
            spriteType);
        return newSprite;
    }

    public static Texture2D LoadTexture(string resourcePath)
    {
        Texture2D tex = Resources.Load<Texture2D>(resourcePath);
        return tex;
    }
}
