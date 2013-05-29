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

        private static readonly int MAP_SIZE_OFFSET = 3;
        private static readonly int MAP_SIZE_MODIFIED_OFFSET = 3;

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
            oddRowOffset = Engine.TileWidth / 2;
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
            //Rectangle destinationRectangle = new Rectangle(
            //   0, 0, Engine.TileWidth, Engine.TileHeight);
            Rectangle destinationRectangle = new Rectangle(0, 0, 0, 0);
            Tile tile;

            Point startingPoint = Engine.VectorToCellIso(camera.Position);

            int mapSizeOffset = 0;

            if (startingPoint.X > MAP_SIZE_OFFSET && startingPoint.Y > MAP_SIZE_OFFSET)
            {
                mapSizeOffset = MAP_SIZE_OFFSET;
            }
            else
            {
                mapSizeOffset = MAP_SIZE_MODIFIED_OFFSET;
            }


            //TODO: kažko glitchina/bugina kairę pusę kai eini žemyn
            if (startingPoint.X > mapSizeOffset + 3)
            {
                startingPoint.X -= (mapSizeOffset + 4);
            }
            else
            {
                startingPoint.X = 0;
            }
            //HACKS
            if (startingPoint.Y > mapSizeOffset + 7)
            {
                startingPoint.Y -= (mapSizeOffset + 8);
            }
            else
            {
                startingPoint.Y = 0;
            }

            Vector2 bottomRightCorner = new Vector2(
                camera.Position.X + camera.ViewportRectangle.Width,
                camera.Position.Y + camera.ViewportRectangle.Height);
            Point endPoint = Engine.VectorToCellIso(bottomRightCorner);

            //TODO: neleisti kamerai užeiti už žemėlapio ribų
            if (endPoint.X < mapLayers[0].Width - mapSizeOffset)
            {
                endPoint.X += mapSizeOffset;
            }
            else
            {
                endPoint.X = mapLayers[0].Width - mapSizeOffset;
            }

            if (endPoint.Y < mapLayers[0].Height - mapSizeOffset)
            {
                endPoint.Y += mapSizeOffset;
            }
            else
            {
                endPoint.Y = mapLayers[0].Height - mapSizeOffset;
            }

           // Console.WriteLine("Camera.Y : {0}, mapLayers[0].Height:{1}", endPoint.Y, mapLayers[0].Height);

            foreach (MapLayer mapLayer in mapLayers)
            {
                //for (int y = 0; y < mapLayer.Height; y++)
                for (int y = startingPoint.Y; y < endPoint.Y; y++)
                {
                    int rowOffset = 0;

                    if (y % 2 == 1)
                    {
                        rowOffset = oddRowOffset;
                    }

                    //Console.WriteLine("RowOffset: {0} y: {1}", rowOffset, y);

                    destinationRectangle.Y = y * tileStepY - (int)camera.Position.Y + baseOffsetY;

                    //for (int x = 0; x < mapLayer.Width; x++)
                    for (int x = startingPoint.X; x < endPoint.X; x++)
                    {
                        tile = mapLayer.GetTile(x, y);

                        if (tile.TileIndex == -1 || tile.Tileset == -1)
                            continue;

                        destinationRectangle.X = x * tileStepX - (int)camera.Position.X + baseOffsetX + rowOffset;
                        destinationRectangle.Width = tilesets[tile.Tileset].TileWidth;
                        destinationRectangle.Height = tilesets[tile.Tileset].TileHeight;
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
