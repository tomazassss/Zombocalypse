using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace XRPGLibrary.TileEngine.TileMaps
{
    public abstract class ATileMap
    {
        #region Field Region

        protected List<Tileset> tilesets;
        protected List<MapLayer> mapLayers;

        protected static int mapWidth;
        protected static int mapHeight;

        #endregion

        #region Property Region

        public static int WidthInPixels
        {
            get { return mapWidth * Engine.TileWidth; }
        }

        public static int HeightInPixels
        {
            get { return mapHeight * Engine.TileHeight; }
        }

        #endregion

        #region Constructor Region

        public ATileMap(List<Tileset> tilesets, List<MapLayer> mapLayers)
        {
            this.tilesets = tilesets;
            this.mapLayers = mapLayers;

            mapWidth = this.mapLayers[0].Width;
            mapHeight = this.mapLayers[0].Height;

            for (int i = 1; i < mapLayers.Count; i++)
            {
                if (mapWidth != this.mapLayers[i].Width || mapHeight != this.mapLayers[i].Height)
                {
                    throw new Exception("Map layer size exception");
                }
            }
        }

        public ATileMap(Tileset tileset, MapLayer mapLayer)
        {
            tilesets = new List<Tileset>();
            tilesets.Add(tileset);

            mapLayers = new List<MapLayer>();
            mapLayers.Add(mapLayer);

            mapWidth = mapLayers[0].Width;
            mapHeight = mapLayers[0].Height;
        }

        #endregion

        #region Abstract Method Region

        public abstract void Draw(SpriteBatch spriteBatch, Camera camera);
        public abstract void AddLayer(MapLayer layer);

        #endregion

        #region Method Region

        public void AddTileset(Tileset tileset)
        {
            tilesets.Add(tileset);
        }

        #endregion
    }
}
