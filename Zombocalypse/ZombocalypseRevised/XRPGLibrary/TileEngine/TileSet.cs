using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XRPGLibrary.TileEngine
{
    public class Tileset
    {
        #region Field Region

        private Texture2D image;
        private int tileWidth;
        private int tileHeight;
        private int tilesWide;
        private int tilesHigh;
        private Rectangle[] sourceRectangles;

        #endregion

        #region Property Region

        public Texture2D Image
        {
            get { return image; }
            private set { image = value; }
        }

        public int TileWidth
        {
            get { return tileWidth; }
            private set { tileWidth = value; }
        }

        public int TileHeight
        {
            get { return tileHeight; }
            private set { tileHeight = value; }
        }

        public int TilesWide
        {
            get { return tilesWide; }
            private set { tilesWide = value; }
        }

        public int TilesHigh
        {
            get { return tilesHigh; }
            private set { tilesHigh = value; }
        }

        public Rectangle[] SourceRectangles
        {
            get { return (Rectangle[])sourceRectangles.Clone(); }
        }

        #endregion

        #region Constructor Region

        public Tileset(Texture2D image, int tilesWide, int tilesHigh,
                       int tileWidth, int tileHeight)
        {
            this.image = image;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.tilesWide = tilesWide;
            this.tilesHigh = tilesHigh;

            int tiles = tilesWide * tilesHigh;

            sourceRectangles = new Rectangle[tiles];

            int tile = 0;

            for (int y = 0; y < tilesHigh; y++)
            {
                for (int x = 0; x < tilesWide; x++)
                {
                    sourceRectangles[tile] = new Rectangle(
                        x * tileWidth,
                        y * tileHeight,
                        tileWidth,
                        tileHeight);
                    tile++;
                }
            }
        }

        #endregion

        #region Method Region
        #endregion
    }
}
