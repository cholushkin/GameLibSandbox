using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TextureBlitter : MonoBehaviour
{
    public Texture2D sourceTexture; // The source texture to copy from
    public Texture2D targetTexture; // The target texture to copy to
    public Vector2 targetPosition; // The position to copy the source texture to on the target

    void Start()
    {
        DrawTexture(sourceTexture, targetTexture, (int)targetPosition.x, (int)targetPosition.y);
        targetTexture.Apply(); // Apply the changes to the target texture
        SaveTargetTextureAsPNG(targetTexture, "result.png"); // Save the target texture as a PNG file
    }

    void DrawTexture(Texture2D source, Texture2D target, int startX, int startY)
    {
        Color[] sourcePixels = source.GetPixels();
        Color[] targetPixels = target.GetPixels();

        int sourceWidth = source.width;
        int sourceHeight = source.height;

        int targetWidth = target.width;
        int targetHeight = target.height;

        for (int x = 0; x < sourceWidth; x++)
        {
            for (int y = 0; y < sourceHeight; y++)
            {
                int sourceIndex = y * sourceWidth + x;
                int targetX = startX + x;
                int targetY = startY + y;

                if (targetX >= 0 && targetX < targetWidth && targetY >= 0 && targetY < targetHeight)
                {
                    int targetIndex = targetY * targetWidth + targetX;

                    // Get the source and target colors
                    Color sourceColor = sourcePixels[sourceIndex];
                    Color targetColor = targetPixels[targetIndex];

                    // Calculate the blended color using alpha blending
                    Color blendedColor = BlendColors(sourceColor, targetColor);

                    // Apply the blended color to the target pixel
                    targetPixels[targetIndex] = blendedColor;
                }
            }
        }

        // Set the modified pixels back to the target texture
        target.SetPixels(targetPixels);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Color BlendColors(Color source, Color target)
    {
        // Calculate the alpha value for the result
        float resultAlpha = source.a + target.a * (1 - source.a);

        // Calculate the blended color using alpha blending
        Color blendedColor = (source * source.a + target * target.a * (1 - source.a)) / resultAlpha;

        // Set the alpha value for the blended color
        blendedColor.a = resultAlpha;

        return blendedColor;
    }




    void SaveTargetTextureAsPNG(Texture2D texture, string filePath)
    {
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, bytes);
        Debug.Log("Saved to: " + filePath);
    }
}
