using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XRPGLibrary.TileEngine.TileMaps;

namespace XRPGLibrary.TileEngine.TileMaps
{
    public class TileMap2D : ATileMap
    {
        #region Field Region
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public TileMap2D(List<Tileset> tilesets, List<MapLayer> mapLayers)
            : base(tilesets, mapLayers)
        {
        }

        public TileMap2D(Tileset tileset, MapLayer mapLayer)
            : base(tileset, mapLayer)
        {
        }

        #endregion

        #region Abstract Method Region

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Rectangle destinationRectangle = new Rectangle(
                0, 0, Engine.TileWidth, Engine.TileHeight);
            Tile tile;

            foreach (MapLayer mapLayer in mapLayers)
            {
                for (int y = 0; y < mapLayer.Height; y++)
                {
                    destinationRectangle.Y = y * Engine.TileHeight - (int)camera.Position.Y;

                    for (int x = 0; x < mapLayer.Width; x++)
                    {
                        tile = mapLayer.GetTile(x, y);

                        destinationRectangle.X = x * Engine.TileWidth - (int)camera.Position.X;

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
