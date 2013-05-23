using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRPGLibrary.TileEngine
{
    public class Tile
    {
        #region Field Region

        private int tileIndex;
        private int tileset;

        #endregion

        #region Property Region

        public int TileIndex
        {
            get { return tileIndex; }
            set { tileIndex = value; }
        }

        public int Tileset
        {
            get { return tileset; }
            set { tileset = value; }
        }

        #endregion

        #region Constructor Region

        public Tile(int tileIndex, int tileset)
        {
            this.tileIndex = tileIndex;
            this.tileset = tileset;
        }

        #endregion

    }
}
