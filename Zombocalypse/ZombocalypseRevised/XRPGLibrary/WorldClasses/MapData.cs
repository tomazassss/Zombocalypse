using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRPGLibrary.WorldClasses
{
    public class MapData
    {
        #region Field Region

        private string mapName;
        private int mapWidth;
        private int mapHeight;
        private MapLayerData[] layers;
        private TilesetData[] tilesets;

        #endregion

        #region Property Region

        public String MapName
        {
            get { return this.mapName; }
            set { this.mapName = value; }
        }

        public int MapWidth
        {
            get { return this.mapWidth; }
            set { this.mapWidth = value; }
        }

        public int MapHeight
        {
            get { return this.mapHeight; }
            set { this.mapHeight = value; }
        }

        public MapLayerData[] Layers
        {
            get { return this.layers; }
            set { this.layers = value; }
        }

        public TilesetData[] Tilesets
        {
            get { return this.tilesets; }
            set { this.tilesets = value; }
        }

        #endregion

        #region Constructor Region

        private MapData()
        {
        }
        public MapData(string mapName, int mapWidth, int mapHeight)
        {
            this.mapName = mapName;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
        }

        #endregion

        #region Method Region

        public void setAdditionalData(List<TilesetData> tilesets, List<MapLayerData> layers){
            this.tilesets = tilesets.ToArray();
            this.layers = layers.ToArray();
        }

        #endregion
    }
}
