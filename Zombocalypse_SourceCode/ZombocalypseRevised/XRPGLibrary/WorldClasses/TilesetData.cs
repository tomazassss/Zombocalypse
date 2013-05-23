using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRPGLibrary.WorldClasses
{
    public class TilesetData
    {
        #region Field Region

        private string tilesetName;
        private string tilesetImageName;
        private int tileWidthInPixels;
        private int tileHeightInPixels;
        private int tilesWide;
        private int tilesHigh;

        #endregion

        #region Property Region

        public string TilesetName
        {
            get { return this.tilesetName; }
            set { this.tilesetName = value; }
        }

        public string TilesetImageName
        {
            get { return this.tilesetImageName; }
            set { this.tilesetImageName = value; }
        }

        public int TileWidthInPixels
        {
            get { return this.tileWidthInPixels; }
            set { this.tileWidthInPixels = value; }
        }

        public int TileHeightInPixels
        {
            get { return this.tileHeightInPixels; }
            set { this.tileHeightInPixels = value; }
        }

        public int TilesWide
        {
            get { return this.tilesWide; }
            set { this.tilesWide = value; }
        }

        public int TilesHigh
        {
            get { return this.tilesHigh; }
            set { this.tilesHigh = value; }
        }

        #endregion
    }
}
