using System;
using UnityEngine;


namespace Truchet.Tiles
{

    public class Tileset : ScriptableObject
    {
        [Serializable]
        public class Division
        {
            public Texture2D[] Tiles;
        }

        public Division[] Divisions;
    }

}