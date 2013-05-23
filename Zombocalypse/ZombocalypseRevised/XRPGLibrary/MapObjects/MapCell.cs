using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XRPGLibrary.Maps
{
    public class MapCell
    {
        private Vector2 position;
        private int tileIndex;

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public int TileIndex
        {
            get { return this.tileIndex; }
            set { this.tileIndex = value; }
        }
    }
}
