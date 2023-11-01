using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGridToFile : MonoBehaviour
{
    public TileGrid TileGrid;
    
    
    void Awake()
    {
        var tiles = GridComposer.CreatePseudorandomTileMatrix(8, 8, 64, 2, TileGrid.Tileset);
        TileGrid.GenerateImage(512,512,tiles);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
