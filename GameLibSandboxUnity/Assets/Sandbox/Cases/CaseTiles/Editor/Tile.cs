using System;
using System.Drawing;

namespace Truchet.Tiles.Editor
{
    class GraphicTile : Tile
    {
        //Reference to the tileset img
        public Image Image { get; }
        public TileType Type { get; }
        public GraphicTile(int x, int y, int level, TileType type, Image image)
            : base(x, y, level)
        {
            Type = type;
            Image = image;
        }
        public override bool IsContainer()
        {
            return false;
        }
    }
}