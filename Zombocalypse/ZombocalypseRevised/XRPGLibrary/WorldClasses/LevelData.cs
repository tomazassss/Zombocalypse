using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRPGLibrary.WorldClasses
{
    public class LevelData
    {
        public string MapName;
        public int MapWidth;
        public int MapHeight;

        private LevelData()
        {
        }

        public LevelData(
            string mapName,
            int mapWidth,
            int mapHeight)
        {
            MapName = mapName;
            MapWidth = mapWidth;
            MapHeight = mapHeight;
        }
    }
}
