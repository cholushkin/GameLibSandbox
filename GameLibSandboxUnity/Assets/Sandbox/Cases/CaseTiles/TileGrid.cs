using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Truchet.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileGrid : MonoBehaviour
{
    public Camera RenderCamera;
    public int DivisionLevels;
    public int TileSize;
    public bool Borderless;
    public Tileset Tileset;

    private RenderTexture renderTexture;
    public Texture testTex;

    public void GenerateImage(int canvasWidth, int canvasHeight, Tile[,] tileMatrix)
    {
        // Create a RenderTexture for rendering the image
        renderTexture = new RenderTexture(canvasWidth, canvasHeight, 32, RenderTextureFormat.ARGB32);
        RenderTexture.active = renderTexture;

        if (RenderCamera == null)
        {

            var cameraGameObject = new GameObject("RenderCamera");
            RenderCamera = cameraGameObject.AddComponent<Camera>();

            RenderCamera.targetTexture = renderTexture;
            RenderCamera.orthographic = true;
            RenderCamera.orthographicSize = canvasHeight / 2;
            RenderCamera.nearClipPlane = -1;
            RenderCamera.farClipPlane = 1;

            RenderCamera.clearFlags = CameraClearFlags.SolidColor;
            RenderCamera.backgroundColor = Color.clear; // Set the background color as needed
            print(RenderCamera.aspect);
        }

        //for (int currentLevel = 0; currentLevel < DivisionLevels; currentLevel++)
        //{
        //    var subTileQueue = new Queue<Tile>();
        //    int offset = CalculateOffset(currentLevel);

        //    foreach (Tile t in tileMatrix)
        //    {
        //        if (t.IsContainer())
        //        {
        //            var ct = (ContainerTile)t;
        //            foreach (Tile child in ct.Container)
        //            {
        //                subTileQueue.Enqueue(child);
        //            }
        //        }
        //        else
        //        {
        //            var texTile = (TextureTile)t;
        //            RenderTileToTexture(texTile.Image, offset, RenderCamera);
        //        }
        //    }

        //    //tileQueue = subTileQueue;
        //}

        Graphics.DrawTexture(new Rect(0, 0, 100, 100), testTex);

        // Create a texture to save the result
        Texture2D resultTexture = new Texture2D(canvasWidth, canvasHeight, TextureFormat.ARGB32, false);
        resultTexture.ReadPixels(new Rect(0, 0, canvasWidth, canvasHeight), 0, 0);
        resultTexture.Apply();

        // Save the texture to a file or use it in Unity as needed
        SaveTextureToFile(resultTexture, "output.png");
    }


    private Rect CalculateDestinationRect(Vector2 targetPositionUV, TextureTile tile)
    {
        float targetX = targetPositionUV.x * renderTexture.width;
        float targetY = targetPositionUV.y * renderTexture.height;

        Rect destinationRect = new Rect(targetX, targetY, tile.Image.width, tile.Image.height);

        return destinationRect;
    }

    private void DrawTextureToRenderTarget(Texture2D sourceTexture, RenderTexture destinationTexture, Rect destinationRect)
    {
        Graphics.DrawTexture(destinationRect, sourceTexture, new Rect(0, 0, 1, 1), 0, 0, 0, 0);
    }

    private void RenderTileToTexture(Texture2D tileTexture, int offset, Camera renderCamera)
    {
        // Create a temporary material with a shader that draws textures
        Material material = new Material(Shader.Find("Unlit/Texture"));

        // Set the texture to be drawn
        material.mainTexture = tileTexture;

        // Draw the texture onto the RenderTexture
        Graphics.Blit(tileTexture, renderTexture, material);

        // Clean up the material
        Destroy(material);
    }

    private static void SaveTextureToFile(Texture2D texture, string fileName)
    {
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(fileName, bytes);
    }


    // The CalculateOffset method is responsible for determining the offset value that is applied to the position
    // of tiles based on the current level of division and a set of generation parameters. This offset is used to
    // position tiles correctly when drawing them onto the canvas,
    // especially as you go deeper into the levels of tile division.
    private int CalculateOffset(int currentLevel)
    {
        int offset = 0;

        if (Borderless) 
            offset -= TileSize / 2;

        if (currentLevel <= 0) 
            return offset;

        int temp = TileSize / 4;
        offset += temp;
        for (int pow = 1; pow < currentLevel; pow++)
        {
            temp /= 2;
            offset += temp;
        }

        return offset;
    }
}


public static class GridComposer
{
    public static Tile[,] CreatePseudorandomTileMatrix(int rowCount, int columnCount, int tileSize, int divisions, Tileset tileset)
    {
        Tile GenerateRandomTile(int level, int x, int y)
        {
            Tile t;

            if (level < divisions && Random.Range(0, level + 1) == 0)
            {
                // NW; NE; SE; SW
                Tile[] subdivision = new Tile[4];
                int offset = tileSize / (int)Mathf.Pow(2, level);
                subdivision[0] = GenerateRandomTile(level + 1, x, y);
                subdivision[1] = GenerateRandomTile(level + 1, x + offset, y);
                subdivision[2] = GenerateRandomTile(level + 1, x + offset, y + offset);
                subdivision[3] = GenerateRandomTile(level + 1, x, y + offset);
                t = new ContainerTile(x, y, level, subdivision);
            }
            else
            {
                int randomType = Random.Range(1, 14);
                t = new TextureTile(x, y, level, tileset.Divisions[level - 1].Tiles[randomType]);
            }
            return t;
        }


        var tiles = new Tile[rowCount, columnCount];
        for (var x = 0; x < columnCount; x++)
            for (var y = 0; y < rowCount; y++)
                tiles[y, x] = GenerateRandomTile(1, x * tileSize, y * tileSize);
        return tiles;
    }

   

}

