using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Truchet.Tiles;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
public class TileGridComposerRandom : MonoBehaviour
{
    public bool ComposeOnAwake;
    public int RowCount;
    public int ColumnCount;
    public int Divisions;
    public int TileSize;
    private Tile[,] _tiles;

    void Awake()
    {
        if (ComposeOnAwake)
            _tiles = Compose(RowCount, ColumnCount, TileSize, Divisions);
    }


    [Button()]
    void Compose()
    {
        _tiles = Compose(RowCount, ColumnCount, TileSize, Divisions);
    }


    public Tile[,] Compose(int rowCount, int columnCount, int tileSize, int divisions)
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
                t = new TextureTile(x, y, level, null);
            }
            return t;
        }


        var tiles = new Tile[rowCount, columnCount];
        for (var x = 0; x < columnCount; x++)
            for (var y = 0; y < rowCount; y++)
                tiles[y, x] = GenerateRandomTile(1, x * tileSize, y * tileSize);
        return tiles;
    }

    private void OnDrawGizmos()
    {
        var tileGridSize = new Vector3(ColumnCount * TileSize, RowCount * TileSize, TileSize);
        var bottomLeftCorner = transform.position - tileGridSize * 0.5f;
        bottomLeftCorner.z = transform.position.z;

        // Draw box of the grid
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, tileGridSize);


        // Draw gizmos for each tile in the _tiles array
        if (_tiles != null)
        {
            foreach (var tile in _tiles)
            {
                DrawTileGizmos(bottomLeftCorner, tile);
            }
        }
    }

    // Recursive method to draw gizmos for a tile and its children
    private void DrawTileGizmos(Vector3 bottomLeftCorner, Tile tile)
    {
        var tileAspect = TileSize / (int)Mathf.Pow(2, tile.Level - 1);
        var halfTileSize = new Vector3(tileAspect * 0.5f, tileAspect * 0.5f, 0);
        var tileSize = new Vector3(tileAspect, tileAspect, TileSize * 0.5f);
        var tilePos = new Vector3(tile.X, tile.Y, 0);

        if (tile.IsContainer())
        {
            var container = (ContainerTile)tile;
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(bottomLeftCorner + tilePos + halfTileSize, tileSize);

            foreach (var subTile in container.Container)
            {
                DrawTileGizmos(bottomLeftCorner, subTile);
            }
        }
        else if (tile is TextureTile textureTile)
        {
            // Handle TextureTile visualization (e.g., draw a sphere)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(bottomLeftCorner + tilePos + halfTileSize, tileSize.x / 2f);

            // Display additional information about the TextureTile
            if (textureTile.Image != null)
            {
                Gizmos.color = Color.white;
                //Gizmos.Label(new Vector3(tile.X, tile.Y, 0), textureTile.Image.name);
            }
        }
    }
}
