using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;

namespace XRPGLibrary.TileEngine.TileMaps
{
    public class TileMapIso : ATileMap
    {
        #region Field Region

        private static int baseOffsetX;
        private static int baseOffsetY;
        private static int tileStepX;
        private static int tileStepY;
        private static int oddRowOffset;

        #endregion

        #region Constructor Region

        public TileMapIso(List<Tileset> tilesets, List<MapLayer> mapLayers)
            : base(tilesets, mapLayers)
        {
            baseOffsetX = -Engine.TileWidth;
            baseOffsetY = -Engine.TileHeight;
            tileStepX = Engine.TileWidth;
            tileStepY = Engine.TileHeight / 2;
            oddRowOffset = Engine.TileHeight;
        }

        public TileMapIso(Tileset tileset, MapLayer mapLayer)
            : base(tileset, mapLayer)
        {
            baseOffsetX = -Engine.TileWidth;
            baseOffsetY = -Engine.TileHeight;
            tileStepX = Engine.TileWidth;
            tileStepY = Engine.TileHeight / 2;
            oddRowOffset = Engine.TileWidth / 2;
        }

        #endregion

        #region Abstract Method Region

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Rectangle destinationRectangle = new Rectangle(
               0, 0, Engine.TileWidth, Engine.TileHeight);
            Point firstCell = Engine.VectorToCell(camera.Position);
            //Console.WriteLine("FirstCell: ({0}, {1})", firstCell.X, firstCell.Y);

            Tile tile;

            foreach (MapLayer mapLayer in mapLayers)
            {
                for (int y = 0; y < mapLayer.Height; y++)
                {
                    int rowOffset = 0;

                    if (y % 2 == 1)
                    {
                        rowOffset = oddRowOffset;
                    }

                    //Console.WriteLine("RowOffset: {0} y: {1}", rowOffset, y);

                    destinationRectangle.Y = y * tileStepY - (int)camera.Position.Y + baseOffsetY;

                    for (int x = 0; x < mapLayer.Width; x++)
                    {
                        tile = mapLayer.GetTile(x, y);

                        destinationRectangle.X = x * tileStepX - (int)camera.Position.X + baseOffsetX + rowOffset;

                        spriteBatch.Draw(
                            tilesets[tile.Tileset].Image,
                            destinationRectangle,
                            tilesets[tile.Tileset].SourceRectangles[tile.TileIndex],
                            Color.White);
                    }
                }
            }
        }

        public override void AddLayer(MapLayer layer)
        {
            if (layer.Width != mapWidth && layer.Height != mapHeight)
            {
                throw new Exception("Map layer size exception");
            }

            mapLayers.Add(layer);
        }

        #endregion
    }
}
