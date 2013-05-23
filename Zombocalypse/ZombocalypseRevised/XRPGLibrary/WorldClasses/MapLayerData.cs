using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRPGLibrary.WorldClasses
{ 
    public struct TileData
    {
        #region Field Region

        private int tileIndex;
        private int tileSetIndex;

        #endregion

        #region Property Region

        public int TileIndex
        {
            get { return this.tileIndex; }
            set { this.tileIndex = value; }
        }

        public int TileSetIndex
        {
            get { return this.tileSetIndex; }
            set { this.tileSetIndex = value; }
        }

        #endregion

        #region Constructor Region

        public TileData(int tileIndex, int tileSetIndex)
        {
            this.tileIndex = tileIndex;
            this.tileSetIndex = tileSetIndex;
        }

        #endregion
    }

    public class MapLayerData
    {
        #region Field Region

        private string mapLayerName;
        private int width;
        private int height;
        private int layerLevel;
        private TileData[] layer;

        #endregion

        #region Property Region

        public string MapLayerName
        {
            get { return this.mapLayerName; }
            set { this.mapLayerName = value; }
        }

        public int Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        public int Height
        {
            get { return this.height; }
            set { this.height = value; }
        }

        public int LayerLevel
        {
            get { return this.layerLevel; }
            set { this.layerLevel = value; }
        }

        public TileData[] Layer
        {
            get { return this.layer; }
            set { this.layer = value; }
        }

        #endregion

        #region Constructor Region

        private MapLayerData()
        {
        }

        

        public MapLayerData(string mapLayerName, int width, int height, int layerLevel)
        {
            MapLayerName = mapLayerName;
            Width = width;
            Height = height;
            LayerLevel = layerLevel;
            Layer = new TileData[height * width];
        }

        public MapLayerData(string mapLayerName, int width, int height, int layerLevel, int tileIndex, int tileSet)
        {
            MapLayerName = mapLayerName;
            Width = width;
            Height = height;
            LayerLevel = layerLevel;
            Layer = new TileData[height * width];

            TileData tile = new TileData(tileIndex, tileSet);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    SetTile(x, y, tile);
        }

        #endregion

        #region Method Region

        public void SetTile(int x, int y, TileData tile)
        {
            Layer[y * Width + x] = tile;
        }

        public void SetTile(int x, int y, int tileIndex, int tileSet)
        {
            Layer[y * Width + x] = new TileData(tileIndex, tileSet);
        }

        public TileData GetTile(int x, int y)
        {
            return Layer[y * Width + x];
        }

        #endregion
    }
}