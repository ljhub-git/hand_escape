using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class SpriteUtils
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
        if (spriteTexture.width < w || spriteTexture.height < h) 
        { 
            throw new ArgumentException($"Could not create sprite ({x}, {y}, {w}, {h}) from a {spriteTexture.width}x{spriteTexture.height} texture."); 
        }

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



    public static Texture2D ResizeTexture(Texture2D originalTexture, int newWidth, int newHeight)
    {
        int minSize = 50; // 최소 크기
        newWidth = Mathf.Max(minSize, newWidth);
        newHeight = Mathf.Max(minSize, newHeight);

        RenderTexture rt = new RenderTexture(newWidth, newHeight, 24);
        RenderTexture.active = rt;
        Graphics.Blit(originalTexture, rt);
        Texture2D newTexture = new Texture2D(newWidth, newHeight);
        newTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        newTexture.Apply();
        return newTexture;
    }

    public static Texture2D ResizeTexture(Texture2D originalTexture, float scaleFactor)
    {
        Debug.Log("ResizeTexture에오");

        int newWidth = Mathf.RoundToInt(originalTexture.width * scaleFactor);
        int newHeight = Mathf.RoundToInt(originalTexture.height * scaleFactor);
        return ResizeTexture(originalTexture, newWidth, newHeight);
    }







}
