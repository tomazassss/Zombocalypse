using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XRPGLibrary.TileEngine
{
    public class Engine
    {
        #region Field Region

        private static int tileWidth;
        private static int tileHeight;

        #endregion

        #region Property Region

        public static int TileWidth
        {
            get { return tileWidth; }
        }

        public static int TileHeight
        {
            get { return tileHeight; }
        }

        #endregion

        #region Constructor Region

        public Engine(int tileWidth, int tileHeight)
        {
            Engine.tileWidth = tileWidth;
            Engine.tileHeight = tileHeight;
        }

        #endregion

        #region Method Region

        public static Point VectorToCell(Vector2 position)
        {
            return new Point((int)position.X / tileWidth,
                             (int)position.Y / tileHeight);
        }

        public static Point VectorToCellIso(Vector2 position)
        {
            int x = 0;
            int y = 0;
            Console.WriteLine("{0}", position.Y % tileWidth);
            if (position.Y % tileHeight > tileHeight / 2)
            {
                y = (int)(position.Y / tileHeight) + 1;
            }
            else
            {
                y = (int)(position.Y / tileHeight) + 1;
            }
            if (position.X % tileWidth > tileWidth / 2)
            {
                x = (int)(position.X / tileWidth) + 1;
            }
            else
            {
                x = (int)(position.X / tileWidth);
            }
            return new Point(x, y);
        }
        #endregion
    }
}
