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
            //int x = 0;
            //int y = 0;
            //if (((int)position.Y < 16))
            //{
            //    y = 1;
            //}
            //else
            //{
            //    y = (((int)position.Y - 12) / 16) + 2;
            //}
            //if (y % 2 == 1)
            //{
            //    x = (((int)position.X + 24) / 64);
            //}
            //else
            //{
            //    x = ((int)position.X / 64) + 1;
            //}
            //return new Point(x, y);

            Point cell = new Point();
            double evenx = Math.Floor(position.X % Engine.TileWidth);
            double eveny = Math.Floor(position.Y % Engine.TileHeight);
            int halfWidth = Engine.TileWidth / 2;
            int halfHeight = Engine.TileHeight / 2;
            //Randama apytiksle vieta
            int x = (int)(Math.Floor((position.X + Engine.TileWidth) / Engine.TileWidth));
            int y = (int)(2 * (Math.Floor((position.Y + Engine.TileHeight) / Engine.TileHeight)));

            //Tikrinama ar taskas patenka i rasta apytiksle vieta
            Point center = new Point(Engine.TileHeight + Engine.TileWidth * (x - 1), halfHeight + halfHeight * (y - 2));

            Point A = new Point(center.X - halfWidth, center.Y);
            Point B = new Point(center.X, center.Y - halfHeight);
            Point C = new Point(center.X + halfWidth, center.Y);
            Point D = new Point(center.X, center.Y + halfHeight);
            Point U = new Point((C.X - A.X) / Engine.TileWidth, (C.Y - A.Y) / Engine.TileWidth);
            Point V = new Point((D.X - B.X) / Engine.TileHeight, (D.Y - B.Y) / Engine.TileHeight);
            Point W = new Point((int)position.X - center.X, (int)position.Y - center.Y);
            double xabs = Math.Abs(W.X * U.X + W.Y * U.Y);
            double yabs = Math.Abs(W.X * V.X + W.Y * V.Y);

            if (xabs / halfWidth + yabs / halfHeight <= 1)
                cell = new Point(x, y);
            else
            {
                //Jei ne priskiriama kita vieta
                x = (int)(Math.Floor((position.X + halfWidth) / Engine.TileWidth));
                y = (int)(2 * (Math.Floor((position.Y + halfHeight) / Engine.TileHeight)) + 1);
                cell = new Point(x, y);
            }
            return cell;
        }
        #endregion
    }
}
