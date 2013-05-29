using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.WorldClasses;

namespace XRPGLibrary.Collisions
{
    public class ObstaclesMapGenerator
    {
        #region Field Region

        private MapData mapData;
        private PathNode[,] grid;

        #endregion

        #region Property Region

        public PathNode[,] Grid
        {
            get { return grid; }
            set { grid = value; }
        }

        #endregion

        #region Constructor Region

        public ObstaclesMapGenerator(MapData mapData)
        {
            this.mapData = mapData;
            grid = new PathNode[mapData.MapHeight, mapData.MapWidth];
            FillGrid();
            //PrintGrid();
        }

        #endregion

        #region Method Region

        private void FillGrid()
        {
            for (int y = 0; y < mapData.MapHeight; y++)
            {
                for (int x = 0; x < mapData.MapWidth; x++)
                {
                    for (int i = mapData.Layers.Length - 1; i >= 0; i--)
                    {
                        TileData tile = mapData.Layers[i].GetTile(x, y);
                        if (tile.TileIndex != -1 && tile.TileSetIndex != -1 && mapData.Layers[i].LayerLevel == 0 || mapData.Layers[i].LayerLevel % 2 == 1)
                        {
                            grid[x, y] = new PathNode()
                            {
                                IsWall = true,
                                X = x,
                                Y = y,
                            };
                            break;
                        }
                        else if (tile.TileIndex != -1 && tile.TileSetIndex != -1 && mapData.Layers[i].LayerLevel == 2)
                        {
                            grid[x, y] = new PathNode()
                            {
                                IsWall = false,
                                X = x,
                                Y = y,
                            };
                            break;
                        }
                    }
                }
            }
        }

        private void PrintGrid()
        {
            //TODO: nezinojau kur saugot, tai jei reiks sios funkcijos pasikeiskit vieta i kuria saugosit.
            System.IO.StreamWriter file = new System.IO.StreamWriter("d:\\map.txt");
            
            for (int y = 0; y < mapData.MapHeight; y++)
            {
                if (y % 2 == 1)
                {
                    file.Write(" ");
                }

                for (int x = 0; x < mapData.MapWidth; x++)
                {
                    if (grid[x, y].IsWall)
                        file.Write(1);
                    else
                        file.Write(0);
                }
                file.WriteLine();
            }
            file.Close();
        }

        #endregion
    }
}
