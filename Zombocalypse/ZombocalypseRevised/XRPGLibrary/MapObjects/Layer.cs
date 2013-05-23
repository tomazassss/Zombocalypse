using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRPGLibrary.Maps
{
    public class Layer
    {
        private String layerName;
        private int layerLevel;
        private List<MapCell> mapCells;

        public String LayerName
        {
            get { return this.layerName; }
            set { this.layerName = value; }
        }

        public int LayerLevel
        {
            get { return this.layerLevel; }
            set { this.layerLevel = value; }
        }

        public List<MapCell> MapCells
        {
            get { return this.mapCells; }
            set { this.mapCells = value; }
        }
    }
}
