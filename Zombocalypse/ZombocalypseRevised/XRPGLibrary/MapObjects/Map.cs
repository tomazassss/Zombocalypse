using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XRPGLibrary.Maps
{
    public class Map
    {
        private String mapName;
        private Point mapSize;
        private List<Layer> mapLayers;

        public String MapName
        {
            get { return this.mapName; }
            set { this.mapName = value; }
        }

        public Point MapSize
        {
            get { return this.mapSize; }
            set { this.mapSize = value; }
        }

        public List<Layer> MapLayers
        {
            get { return this.mapLayers; }
            set { this.mapLayers = value; }
        }
    }
}
