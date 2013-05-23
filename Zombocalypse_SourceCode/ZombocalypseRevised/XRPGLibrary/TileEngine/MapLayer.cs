using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XRPGLibrary.WorldClasses;

namespace XRPGLibrary.TileEngine
{
    public class MapLayer
    {
        #region Field Region

        Tile[,] map;

        private int layerLevel;

        #endregion

        #region Property Region

        public int Width
        {
            get { return map.GetLength(1); }
        }

        public int Height
        {
            get { return map.GetLength(0); }
        }
        
        public int LayerLevel
        {
            get { return this.layerLevel; }
            set { this.layerLevel = value; }
        }

        #endregion

        #region Constructor Region

        public MapLayer(Tile[,] map)
        {
            this.map = map.Clone() as Tile[,];
        }

        public MapLayer(int width, int height, int layerLevel)
        {
            LayerLevel = layerLevel;
            map = new Tile[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[y, x] = new Tile(0, 0);
                }
            }
        }

        #endregion

        #region Method Region

        public Tile GetTile(int x, int y)
        {
            return map[y, x];
        }

        public int GetTileTileset(int x, int y)
        {
            return map[y, x].Tileset;
        }

        public void SetTile(int x, int y, Tile tile)
        {
            map[y, x] = tile;
        }

        public void SetTile(int x, int y, int tileIndex, int tileset)
        {
            map[y, x] = new Tile(tileIndex, tileset);
        }

        public void SetTileTileset(int x, int y, int tileset)
        {
            map[y, x].Tileset = tileset;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, List<Tileset> tilesets)
        {
            int baseOffsetX = -Engine.TileWidth;
            int baseOffsetY = -Engine.TileHeight;
            int tileStepX = Engine.TileWidth;
            int tileStepY = Engine.TileHeight / 2;
            int oddRowOffset = Engine.TileWidth / 2;

            Rectangle destination = new Rectangle(0, 0, 0, 0);
            Tile tile;

            for (int y = 0; y < Height; y++)
            {
                int rowOffset = 0;

                if (y % 2 == 1)
                {
                    //
                    rowOffset = oddRowOffset;
                }

                destination.Y = y * tileStepY - (int)camera.Position.Y + baseOffsetY;

                for (int x = 0; x < Width; x++)
                {
                    tile = GetTile(x, y);

                    if (tile.TileIndex == -1 || tile.Tileset == -1)
                        continue;

                    destination.X = x * tileStepX - (int)camera.Position.X + baseOffsetX + rowOffset;
                    destination.Width = tilesets[tile.Tileset].TileWidth;
                    destination.Height = tilesets[tile.Tileset].TileHeight;
                    spriteBatch.Draw(
                        tilesets[tile.Tileset].Image,
                        destination,
                        tilesets[tile.Tileset].SourceRectangles[tile.TileIndex],
                        Color.White);
                }
            }
        }

        public static MapLayer FromMapLayerData(MapLayerData data)
        {
            MapLayer layer = new MapLayer(data.Width, data.Height, data.LayerLevel);
            
            for (int y = 0; y < data.Height; y++)
                for (int x = 0; x < data.Width; x++)
                {
                    layer.SetTile(
                        x,
                        y,
                        data.GetTile(x, y).TileIndex,
                        data.GetTile(x, y).TileSetIndex);
                }

            return layer;
        }

        #endregion
    }
}
