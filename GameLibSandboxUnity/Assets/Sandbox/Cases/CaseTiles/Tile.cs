using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Truchet.Tiles
{
    public abstract class Tile
    {
        public int X { get; }
        public int Y { get; }
        public int Level { get; }

        public Tile(int x, int y, int level)
        {
            this.X = x;
            this.Y = y;
            this.Level = level;
        }

        public abstract bool IsContainer();
    }

    class ContainerTile : Tile
    {
        // Clockwise from NW: NW, NE, SE, SW, 
        public Tile[] Container { get; }

        public ContainerTile(int x, int y, int level, Tile[] subdivison)
            : base(x, y, level)
        {
            if (subdivison.Any(t => t == null))
                throw new Exception("container has to be filled");

            Container = subdivison;
        }

        public override bool IsContainer()
        {
            return true;
        }
    }

    class TextureTile : Tile
    {
        public Texture2D Image { get; }
        public TextureTile(int x, int y, int level, Texture2D image)
            : base(x, y, level)
        {
            Image = image;
        }

        public override bool IsContainer()
        {
            return false;
        }
    }
}