using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XRPGLibrary.TileEngine;
using XRPGLibrary.WorldClasses;

namespace XRPGLibrary.Collisions
{
    public class CollisionsManager
    {
        #region Field Region

        private MapData map;
        private Vector2 spritePosition;
        private float spriteSpeed;        

        #endregion

        #region Property Region

        public void SetMap(MapData map)
        {
            this.map = map;
        }

        public void SetSpriteSpeed(float spriteSpeed)
        {
            this.spriteSpeed = spriteSpeed;
        }

        public Vector2 SpritePosition()
        {
            return spritePosition;
        }

        #endregion

        #region Constructor Region

        public CollisionsManager()
        {
        }

        #endregion

        #region Method Region

        public Vector2 DetectCollisions(Vector2 motion, Vector2 spritePosition)
        {
            this.spritePosition = spritePosition;
            Vector2 currentPosition = spritePosition;
            Vector2 finalPosition;
            currentPosition.X += 24;
            currentPosition.Y += 40;
            Point currentCellPosition = Engine.VectorToCellIso(currentPosition);
            float x = 0;
            float y = 0;

            //if (motion.Y > 0)
            //{
            //    y = BottomCollisionDetection(motion, currentPosition, currentCellPosition);
            //}
            //else if (motion.Y < 0)
            //{
            //    y = TopCollisionDetection(motion, currentPosition, currentCellPosition);
            //}

            //if (motion.X > 0)
            //{
            //    x = RightCollisionDetection(motion, currentPosition, currentCellPosition);
            //}
            //else if (motion.X < 0)
            //{
            //    x = LeftCollisionDetection(motion, currentPosition, currentCellPosition);
            //}

            float x1 = 0;
            float y1 = 0;

            if (motion.Y > 0)
            {
                y1 = BottomCollisionDetection(motion, currentPosition, currentCellPosition);
            }
            else if (motion.Y < 0)
            {
                y1 = TopCollisionDetection(motion, currentPosition, currentCellPosition);
            }

            if (motion.X > 0)
            {
                x1 = RightCollisionDetection(motion, currentPosition, currentCellPosition);
            }
            else if (motion.X < 0)
            {
                x1 = LeftCollisionDetection(motion, currentPosition, currentCellPosition);
            }

            //Istrizos kliutys, darbas su x asimi
            if (x == 0 && motion.X >= 0 && motion.Y >= 0)
            {
                x = BottomRightColDetX(motion, currentPosition, currentCellPosition);
            }
            if (x == 0 && motion.X >= 0 && motion.Y <= 0)
            {
                x = TopRightColDetX(motion, currentPosition, currentCellPosition);
            }
            if (x == 0 && motion.X <= 0 && motion.Y >= 0)
            {
                x = BottomLeftColDetX(motion, currentPosition, currentCellPosition);
            }
            if (x == 0 && motion.X <= 0 && motion.Y <= 0)
            {
                x = TopLeftColDetX(motion, currentPosition, currentCellPosition);
            }

            finalPosition = spritePosition;
            if (x != 0)
                finalPosition.X = x;
            else if (x1 != 0)
            {
                finalPosition.X = x1;
            }
            else
            {
                Vector2 tempPosition = finalPosition;
                tempPosition.X += motion.X * spriteSpeed;
                tempPosition.X += 24;
                tempPosition.Y += 40;
                Point cellPosition = Engine.VectorToCellIso(tempPosition);

                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    TileData tile = map.Layers[i].GetTile(cellPosition.X, cellPosition.Y);
                    if (tile.TileIndex != -1 && tile.TileSetIndex != -1 && map.Layers[i].LayerLevel == 0 || map.Layers[i].LayerLevel % 2 == 1)
                    {
                        break;
                    }
                    else if (tile.TileIndex != -1 && tile.TileSetIndex != -1)
                    {
                        finalPosition.X += motion.X * spriteSpeed;
                        break;
                    }
                }

                //finalPosition.X += motion.X * spriteSpeed;
            }

            currentPosition = finalPosition;
            currentPosition.X += 24;
            currentPosition.Y += 40;
            currentCellPosition = Engine.VectorToCellIso(currentPosition);


            //Istrizos kliutys, darbas su Y asimi
            if (y == 0 && motion.Y >= 0)
            {
                y = BottomLeftColDetY(motion, currentPosition, currentCellPosition);
            }
            if (y == 0 && motion.Y >= 0)
            {
                y = BottomRightColDetY(motion, currentPosition, currentCellPosition);
            }
            if (y == 0 && motion.Y <= 0)
            {
                y = TopLeftColDetY(motion, currentPosition, currentCellPosition);
            }
            if (y == 0 && motion.Y <= 0)
            {
                y = TopRightColDetY(motion, currentPosition, currentCellPosition);
            }


            if (y != 0)
                finalPosition.Y = y;
            else if (y1 != 0)
            {
                finalPosition.Y = y1;
            }
            else
            {
                Vector2 tempPosition = finalPosition;
                tempPosition.Y += motion.Y * spriteSpeed;
                tempPosition.X += 24;
                tempPosition.Y += 40;
                Point cellPosition = Engine.VectorToCellIso(tempPosition);

                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    TileData tile = map.Layers[i].GetTile(cellPosition.X, cellPosition.Y);
                    if (tile.TileIndex != -1 && tile.TileSetIndex != -1 && map.Layers[i].LayerLevel == 0 || map.Layers[i].LayerLevel % 2 == 1)
                    {
                        break;
                    }
                    else if (tile.TileIndex != -1 && tile.TileSetIndex != -1)
                    {
                        finalPosition.Y += motion.Y * spriteSpeed;
                        break;
                    }
                }

                //finalPosition.Y += motion.Y * spriteSpeed;
            }
            return finalPosition;
        }
        //TODO: patobulinti statmenu kliuciu atpazinima (dar ne iki galo gerai veikia, Karolis).
        private float BottomCollisionDetection(Vector2 motion, Vector2 currentPosition, Point currentCellPosition)
        {
            float testMoveY = currentPosition.Y;

            if (currentCellPosition.Y % 2 == 1)
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    TileData tileBelow = map.Layers[i].GetTile(currentCellPosition.X, currentCellPosition.Y + 2);
                    TileData tileBelowLeft = map.Layers[i].GetTile(currentCellPosition.X - 1, currentCellPosition.Y + 2);  
                    TileData tileBelowRight = map.Layers[i].GetTile(currentCellPosition.X + 1, currentCellPosition.Y + 2);

                    if (ObstaclesCheckCancel(tileBelowLeft, tileBelow, tileBelowRight, i))
                    {
                        continue;
                    }
                    if (ObstaclesCheck(tileBelowLeft, tileBelow, tileBelowRight, i))
                    {
                        for (int j = map.Layers.Length - 1; j >= 0; j--)
                        {
                            TileData tileLeft = map.Layers[j].GetTile(currentCellPosition.X, currentCellPosition.Y + 1);
                            TileData tileRight = map.Layers[j].GetTile(currentCellPosition.X + 1, currentCellPosition.Y + 1);
                            if (tileLeft.TileIndex != -1 && tileLeft.TileSetIndex != -1 &&
                                tileRight.TileIndex != -1 && tileRight.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel % 2 == 0 && map.Layers[j].LayerLevel != 0)
                            {
                                //Console.WriteLine("Nelyg 1");
                                testMoveY += motion.Y * spriteSpeed;
                                int maxY = currentCellPosition.Y * Engine.TileHeight / 2 - 1;
                                if (testMoveY > maxY)
                                {
                                    testMoveY = maxY;
                                }
                                testMoveY -= 40;
                                return testMoveY;
                            }

                            if (tileLeft.TileIndex != -1 && tileLeft.TileSetIndex != -1 &&
                                tileRight.TileIndex != -1 && tileRight.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel == 0 || map.Layers[j].LayerLevel % 2 == 1)
                            {
                                //Console.WriteLine("Nelyg 2");
                                testMoveY += motion.Y * spriteSpeed;
                                int maxY = (currentCellPosition.Y - 1) * Engine.TileHeight / 2 - 1;
                                if (testMoveY > maxY)
                                {
                                    testMoveY = maxY;
                                }
                                testMoveY -= 40;
                                return testMoveY;
                            }

                            if (CheckAnomaly(tileLeft, tileRight, j))
                            {
                                testMoveY = 0;
                                return testMoveY;
                            }
                        }
                    }
                    else if (CancelCheck(tileBelowLeft, tileBelow, tileBelowRight))
                    {
                        testMoveY = 0;
                        return testMoveY;
                    }
                }
            }
            else
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {

                    TileData tileBelow = map.Layers[i].GetTile(currentCellPosition.X, currentCellPosition.Y + 2);
                    TileData tileBelowLeft = map.Layers[i].GetTile(currentCellPosition.X - 1, currentCellPosition.Y + 2);
                    TileData tileBelowRight = map.Layers[i].GetTile(currentCellPosition.X + 1, currentCellPosition.Y + 2);

                    if (ObstaclesCheckCancel(tileBelowLeft, tileBelow, tileBelowRight, i))
                    {
                        continue;
                    }
                    if (ObstaclesCheck(tileBelowLeft, tileBelow, tileBelowRight, i))
                    {
                        for (int j = map.Layers.Length - 1; j >= 0; j--)
                        {
                            TileData tileLeft = map.Layers[j].GetTile(currentCellPosition.X - 1, currentCellPosition.Y + 1);
                            TileData tileRight = map.Layers[j].GetTile(currentCellPosition.X, currentCellPosition.Y + 1);
                            if (tileLeft.TileIndex != -1 && tileLeft.TileSetIndex != -1 &&
                                tileRight.TileIndex != -1 && tileRight.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel == 0 || map.Layers[j].LayerLevel % 2 == 1)
                            {
                                //Console.WriteLine("lyg 1");
                                testMoveY += motion.Y * spriteSpeed;
                                int maxY = (currentCellPosition.Y - 1) * Engine.TileHeight / 2 - 1;
                                if (testMoveY > maxY)
                                {
                                    testMoveY = maxY;
                                }
                                testMoveY -= 40;
                                return testMoveY;
                            }

                            if (tileLeft.TileIndex != -1 && tileLeft.TileSetIndex != -1 &&
                                tileRight.TileIndex != -1 && tileRight.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel % 2 == 0 && map.Layers[j].LayerLevel != 0)
                            {
                                //Console.WriteLine("lyg 2");
                                testMoveY += motion.Y * spriteSpeed;
                                int maxY = currentCellPosition.Y * Engine.TileHeight / 2 - 1;
                                if (testMoveY > maxY)
                                {
                                    testMoveY = maxY;
                                }
                                testMoveY -= 40;
                                return testMoveY;
                            }

                            if (CheckAnomaly(tileLeft, tileRight, j))
                            {
                                testMoveY = 0;
                                return testMoveY;
                            }
                        }
                    }
                    else if (CancelCheck(tileBelowLeft, tileBelow, tileBelowRight))
                    {
                        testMoveY = 0;
                        return testMoveY;
                    }
                }
            }
            testMoveY = 0;
            return testMoveY;
        }

        private float TopCollisionDetection(Vector2 motion, Vector2 currentPosition, Point currentCellPosition)
        {
            float testMoveY = currentPosition.Y;
            if (currentCellPosition.Y % 2 == 1)
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    TileData tileTop = map.Layers[i].GetTile(currentCellPosition.X, currentCellPosition.Y - 2);
                    TileData tileTopLeft = map.Layers[i].GetTile(currentCellPosition.X - 1, currentCellPosition.Y - 2);
                    TileData tileTopRight = map.Layers[i].GetTile(currentCellPosition.X + 1, currentCellPosition.Y - 2);

                    if (ObstaclesCheckCancel(tileTopLeft, tileTop, tileTopRight, i))
                    {
                        continue;
                    }
                    if (ObstaclesCheck(tileTopLeft, tileTop, tileTopRight, i))
                    {
                        //Console.WriteLine("Nelyg");
                        for (int j = map.Layers.Length - 1; j >= 0; j--)
                        {
                            TileData tileLeft = map.Layers[j].GetTile(currentCellPosition.X, currentCellPosition.Y - 1);
                            TileData tileRight = map.Layers[j].GetTile(currentCellPosition.X + 1, currentCellPosition.Y - 1);
                            if (tileLeft.TileIndex != -1 && tileLeft.TileSetIndex != -1 &&
                                tileRight.TileIndex != -1 && tileRight.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel % 2 == 0 && map.Layers[j].LayerLevel != 0)
                            {
                                //Console.WriteLine("Nelyg 2");
                                testMoveY += motion.Y * spriteSpeed;
                                int maxY = (currentCellPosition.Y - 2) * Engine.TileHeight / 2 + 1;
                                if (testMoveY < maxY)
                                {
                                    testMoveY = maxY;
                                }
                                testMoveY -= 40;
                                return testMoveY;
                            }


                            if (tileLeft.TileIndex != -1 && tileLeft.TileSetIndex != -1 &&
                                tileRight.TileIndex != -1 && tileRight.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel == 0 || map.Layers[j].LayerLevel % 2 == 1)
                            {
                                //Console.WriteLine("Nelyg 1");
                                testMoveY += motion.Y * spriteSpeed;
                                int maxY = (currentCellPosition.Y - 1)* Engine.TileHeight / 2 + 1;
                                if (testMoveY < maxY)
                                {
                                    testMoveY = maxY;
                                }
                                testMoveY -= 40;
                                return testMoveY;
                            }

                            if (CheckAnomaly(tileLeft, tileRight, j))
                            {
                                testMoveY = 0;
                                return testMoveY;
                            }
                        }
                    }
                    else if (CancelCheck(tileTopLeft, tileTop, tileTopRight))
                    {
                        testMoveY = 0;
                        return testMoveY;
                    }
                }
            }
            else
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {

                    TileData tileTop = map.Layers[i].GetTile(currentCellPosition.X, currentCellPosition.Y - 2);
                    TileData tileTopLeft = map.Layers[i].GetTile(currentCellPosition.X - 1, currentCellPosition.Y - 2);
                    TileData tileTopRight = map.Layers[i].GetTile(currentCellPosition.X + 1, currentCellPosition.Y - 2);

                    if (ObstaclesCheckCancel(tileTopLeft, tileTop, tileTopRight, i))
                    {
                        continue;
                    }
                    if (ObstaclesCheck(tileTopLeft, tileTop, tileTopRight, i))
                    {
                        //Console.WriteLine("Lyg");
                        for (int j = map.Layers.Length - 1; j >= 0; j--)
                        {
                            TileData tileLeft = map.Layers[j].GetTile(currentCellPosition.X - 1, currentCellPosition.Y - 1);
                            TileData tileRight = map.Layers[j].GetTile(currentCellPosition.X, currentCellPosition.Y - 1);
                            if (tileLeft.TileIndex != -1 && tileLeft.TileSetIndex != -1 &&
                                tileRight.TileIndex != -1 && tileRight.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel == 0 || map.Layers[j].LayerLevel % 2 == 1)
                            {
                                //Console.WriteLine("lyg 2");
                                testMoveY += motion.Y * spriteSpeed;
                                int maxY = (currentCellPosition.Y - 1) * Engine.TileHeight / 2 + 1;
                                if (testMoveY < maxY)
                                {
                                    testMoveY = maxY;
                                }
                                testMoveY -= 40;
                                return testMoveY;
                            }

                            if (tileLeft.TileIndex != -1 && tileLeft.TileSetIndex != -1 &&
                                tileRight.TileIndex != -1 && tileRight.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel % 2 == 0 && map.Layers[j].LayerLevel != 0)
                            {
                                //Console.WriteLine("lyg 1");
                                testMoveY += motion.Y * spriteSpeed;
                                int maxY = (currentCellPosition.Y - 2) * Engine.TileHeight / 2 + 1;
                                if (testMoveY < maxY)
                                {
                                    testMoveY = maxY;
                                }
                                testMoveY -= 40;
                                return testMoveY;
                            }

                            if (CheckAnomaly(tileLeft, tileRight, j))
                            {
                                testMoveY = 0;
                                return testMoveY;
                            }
                        }
                    }
                    else if (CancelCheck(tileTopLeft, tileTop, tileTopRight))
                    {
                        testMoveY = 0;
                        return testMoveY;
                    }
                }
            }
            testMoveY = 0;
            return testMoveY;
        }

        private float RightCollisionDetection(Vector2 motion, Vector2 currentPosition, Point currentCellPosition)
        {
            float testMoveX = currentPosition.X;

            if (currentCellPosition.Y % 2 == 1)
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    TileData tileAboveRight = map.Layers[i].GetTile(currentCellPosition.X + 1, currentCellPosition.Y - 2);
                    TileData tileRight = map.Layers[i].GetTile(currentCellPosition.X + 1, currentCellPosition.Y);
                    TileData tileBelowRight = map.Layers[i].GetTile(currentCellPosition.X + 1, currentCellPosition.Y + 2);

                    if (ObstaclesCheckCancel(tileAboveRight, tileRight, tileBelowRight, i))
                    {
                        continue;
                    }
                    if (ObstaclesCheck(tileAboveRight, tileRight, tileBelowRight, i))
                    {
                        //Console.WriteLine("Nelyg");
                        for (int j = map.Layers.Length - 1; j >= 0; j--)
                        {
                            TileData tileRAbove = map.Layers[j].GetTile(currentCellPosition.X + 1, currentCellPosition.Y - 1);
                            TileData tileRBelow = map.Layers[j].GetTile(currentCellPosition.X + 1, currentCellPosition.Y + 1);
                            if (tileRAbove.TileIndex != -1 && tileRAbove.TileSetIndex != -1 &&
                                tileRBelow.TileIndex != -1 && tileRBelow.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel % 2 == 0 && map.Layers[j].LayerLevel != 0)
                            {
                                //Console.WriteLine("Nelyg 2");
                                testMoveX += motion.X * spriteSpeed;
                                int maxX = currentCellPosition.X * Engine.TileWidth - 1 + (Engine.TileWidth / 2);
                                if (testMoveX > maxX)
                                {
                                    testMoveX = maxX;
                                }
                                testMoveX -= 24;
                                return testMoveX;
                            }


                            if (tileRAbove.TileIndex != -1 && tileRAbove.TileSetIndex != -1 &&
                                tileRBelow.TileIndex != -1 && tileRBelow.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel == 0 || map.Layers[j].LayerLevel % 2 == 1)
                            {
                                //Console.WriteLine("Nelyg 1");
                                testMoveX += motion.X * spriteSpeed;
                                int maxX = currentCellPosition.X * Engine.TileWidth - 1;
                                if (testMoveX > maxX)
                                {
                                    testMoveX = maxX;
                                }
                                testMoveX -= 24;
                                return testMoveX;
                            }

                            if (CheckAnomaly(tileRAbove, tileRBelow, j))
                            {
                                testMoveX = 0;
                                return testMoveX;
                            }
                        }
                    }
                    else if (CancelCheck(tileAboveRight, tileRight, tileBelowRight))
                    {
                        testMoveX = 0;
                        return testMoveX;
                    }
                }
            }
            else
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {

                    TileData tileAboveRight = map.Layers[i].GetTile(currentCellPosition.X + 1, currentCellPosition.Y - 2);
                    TileData tileRight = map.Layers[i].GetTile(currentCellPosition.X + 1, currentCellPosition.Y);
                    TileData tileBelowRight = map.Layers[i].GetTile(currentCellPosition.X + 1, currentCellPosition.Y + 2);

                    if (ObstaclesCheckCancel(tileAboveRight, tileRight, tileBelowRight, i))
                    {
                        continue;
                    }
                    if (ObstaclesCheck(tileAboveRight, tileRight, tileBelowRight, i))
                    {
                        //Console.WriteLine("Lyg");
                        for (int j = map.Layers.Length - 1; j >= 0; j--)
                        {
                            TileData tileRAbove = map.Layers[j].GetTile(currentCellPosition.X, currentCellPosition.Y - 1);
                            TileData tileRBelow = map.Layers[j].GetTile(currentCellPosition.X, currentCellPosition.Y + 1);
                            if (tileRAbove.TileIndex != -1 && tileRAbove.TileSetIndex != -1 &&
                                tileRBelow.TileIndex != -1 && tileRBelow.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel == 0 || map.Layers[j].LayerLevel % 2 == 1)
                            {
                                //Console.WriteLine("lyg 2");
                                testMoveX += motion.X * spriteSpeed;
                                int maxX = currentCellPosition.X * Engine.TileWidth - 1 - (Engine.TileWidth / 2);
                                if (testMoveX > maxX)
                                {
                                    testMoveX = maxX;
                                }
                                testMoveX -= 24;
                                return testMoveX;
                            }

                            if (tileRAbove.TileIndex != -1 && tileRAbove.TileSetIndex != -1 &&
                                tileRBelow.TileIndex != -1 && tileRBelow.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel % 2 == 0 && map.Layers[j].LayerLevel != 0)
                            {
                                //Console.WriteLine("lyg 1");
                                testMoveX += motion.X * spriteSpeed;
                                int maxX = currentCellPosition.X * Engine.TileWidth - 1;
                                if (testMoveX > maxX)
                                {
                                    testMoveX = maxX;
                                }
                                testMoveX -= 24;
                                return testMoveX;
                            }

                            if (CheckAnomaly(tileRAbove, tileRBelow, j))
                            {
                                testMoveX = 0;
                                return testMoveX;
                            }
                        }
                    }
                    else if (CancelCheck(tileAboveRight, tileRight, tileBelowRight))
                    {
                        testMoveX = 0;
                        return testMoveX;
                    }
                }
            }
            testMoveX = 0;
            return testMoveX;
        }

        private float LeftCollisionDetection(Vector2 motion, Vector2 currentPosition, Point currentCellPosition)
        {
            float testMoveX = currentPosition.X;


            if (currentCellPosition.Y % 2 == 1)
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    TileData tileAboveLeft = map.Layers[i].GetTile(currentCellPosition.X - 1, currentCellPosition.Y - 2);
                    TileData tileLeft = map.Layers[i].GetTile(currentCellPosition.X - 1, currentCellPosition.Y);
                    TileData tileBelowLeft = map.Layers[i].GetTile(currentCellPosition.X - 1, currentCellPosition.Y + 2);

                    if (ObstaclesCheckCancel(tileAboveLeft, tileLeft, tileBelowLeft, i))
                    {
                        continue;
                    }
                    if (ObstaclesCheck(tileAboveLeft, tileLeft, tileBelowLeft, i))
                    {
                        //Console.WriteLine("Nelyg");
                        for (int j = map.Layers.Length - 1; j >= 0; j--)
                        {
                            TileData tileLAbove = map.Layers[j].GetTile(currentCellPosition.X, currentCellPosition.Y - 1);
                            TileData tileLBelow = map.Layers[j].GetTile(currentCellPosition.X, currentCellPosition.Y + 1);
                            if (tileLAbove.TileIndex != -1 && tileLAbove.TileSetIndex != -1 &&
                                tileLBelow.TileIndex != -1 && tileLBelow.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel % 2 == 0 && map.Layers[j].LayerLevel != 0)
                            {
                                //Console.WriteLine("Nelyg 1");
                                testMoveX += motion.X * spriteSpeed;
                                int maxX = (currentCellPosition.X - 1) * Engine.TileWidth + 1 + Engine.TileWidth / 2;
                                if (testMoveX < maxX)
                                {
                                    testMoveX = maxX;
                                }
                                testMoveX -= 24;
                                return testMoveX;
                            }

                            if (tileLAbove.TileIndex != -1 && tileLAbove.TileSetIndex != -1 &&
                                tileLBelow.TileIndex != -1 && tileLBelow.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel == 0 || map.Layers[j].LayerLevel % 2 == 1)
                            {
                                //Console.WriteLine("Nelyg 2");
                                testMoveX += motion.X * spriteSpeed;
                                int maxX = currentCellPosition.X * Engine.TileWidth + 1;
                                if (testMoveX < maxX)
                                {
                                    testMoveX = maxX;
                                }
                                testMoveX -= 24;
                                return testMoveX;
                            }

                            if (CheckAnomaly(tileLAbove, tileLBelow, j))
                            {
                                testMoveX = 0;
                                return testMoveX;
                            }
                        }
                    }
                    else if (CancelCheck(tileAboveLeft, tileLeft, tileBelowLeft))
                    {
                        testMoveX = 0;
                        return testMoveX;
                    }
                }
            }
            else
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {

                    TileData tileAboveLeft = map.Layers[i].GetTile(currentCellPosition.X - 1, currentCellPosition.Y - 2);
                    TileData tileLeft = map.Layers[i].GetTile(currentCellPosition.X - 1, currentCellPosition.Y);
                    TileData tileBelowLeft = map.Layers[i].GetTile(currentCellPosition.X - 1, currentCellPosition.Y + 2);

                    if (ObstaclesCheckCancel(tileAboveLeft, tileLeft, tileBelowLeft, i))
                    {
                        continue;
                    }
                    if (ObstaclesCheck(tileAboveLeft, tileLeft, tileBelowLeft, i))
                    {
                        //Console.WriteLine("Lyg");
                        for (int j = map.Layers.Length - 1; j >= 0; j--)
                        {
                            TileData tileLAbove = map.Layers[j].GetTile(currentCellPosition.X - 1, currentCellPosition.Y - 1);
                            TileData tileLBelow = map.Layers[j].GetTile(currentCellPosition.X - 1, currentCellPosition.Y + 1);
                            if (tileLAbove.TileIndex != -1 && tileLAbove.TileSetIndex != -1 &&
                                tileLBelow.TileIndex != -1 && tileLBelow.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel == 0 || map.Layers[j].LayerLevel % 2 == 1)
                            {
                                //Console.WriteLine("lyg 1");
                                testMoveX += motion.X * spriteSpeed;
                                int maxX = (currentCellPosition.X - 1) * Engine.TileWidth + 1 + Engine.TileWidth / 2;
                                if (testMoveX < maxX)
                                {
                                    testMoveX = maxX;
                                }
                                testMoveX -= 24;
                                return testMoveX;
                            }

                            if (tileLAbove.TileIndex != -1 && tileLAbove.TileSetIndex != -1 &&
                                tileLBelow.TileIndex != -1 && tileLBelow.TileSetIndex != -1 &&
                                map.Layers[j].LayerLevel % 2 == 0 && map.Layers[j].LayerLevel != 0)
                            {
                                //Console.WriteLine("lyg 2");
                                testMoveX += motion.X * spriteSpeed;
                                int maxX = (currentCellPosition.X - 1) * Engine.TileWidth + 1;
                                if (testMoveX < maxX)
                                {
                                    testMoveX = maxX;
                                }
                                testMoveX -= 24;
                                return testMoveX;
                            }

                            if (CheckAnomaly(tileLAbove, tileLBelow, j))
                            {
                                testMoveX = 0;
                                return testMoveX;
                            }
                        }
                    }
                    else if (CancelCheck(tileAboveLeft, tileLeft, tileBelowLeft))
                    {
                        testMoveX = 0;
                        return testMoveX;
                    }
                }
            }
            testMoveX = 0;
            return testMoveX;
        }
        //Laikinos funkcijos istrizoms kliutims atpazinti y asiai
        private float BottomLeftColDetY(Vector2 motion, Vector2 currentPosition, Point currentCellPosition)
        {
            float testMoveY = currentPosition.Y;

            bool testTile = false;
            bool testTile2 = false;

            if (currentCellPosition.Y % 2 == 1)
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X + 1, currentCellPosition.Y + 1, i)) 
                        testTile = true;
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X, currentCellPosition.Y + 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas nelyg");
                        int x = (int)(currentPosition.X - Engine.TileWidth / 2) % Engine.TileWidth;
                        int y = x / 2;
                        int maxY = currentCellPosition.Y * (Engine.TileHeight / 2) + y - (Engine.TileHeight / 2) - 1;
                        testMoveY += motion.Y * spriteSpeed;
                        if (testMoveY > maxY)
                        {
                            testMoveY = maxY;
                        }
                        testMoveY -= 40;
                        return testMoveY;

                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveY = 0;
                        return testMoveY;
                    }
                }
            }
            else
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X - 1, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X - 1, currentCellPosition.Y + 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas lyg");
                        int x = (int)currentPosition.X % Engine.TileWidth;
                        int y = x / 2;
                        int maxY = currentCellPosition.Y * (Engine.TileHeight / 2) + y - (Engine.TileHeight / 2) - 1;
                        testMoveY += motion.Y * spriteSpeed;
                        if (testMoveY > maxY)
                        {
                            testMoveY = maxY;
                        }
                        testMoveY -= 40;
                        return testMoveY;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveY = 0;
                        return testMoveY;
                    }
                }
            }

            testMoveY = 0;
            return testMoveY;
        }

        private float BottomRightColDetY(Vector2 motion, Vector2 currentPosition, Point currentCellPosition)
        {
            float testMoveY = currentPosition.Y;

            bool testTile = false;
            bool testTile2 = false;

            if (currentCellPosition.Y % 2 == 1)
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X + 1, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X + 1, currentCellPosition.Y + 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas nelyg");
                        int x = (int)(currentPosition.X + Engine.TileWidth / 2) % Engine.TileWidth;
                        int y = Engine.TileHeight / 2 - x / 2;
                        int maxY = currentCellPosition.Y * (Engine.TileHeight / 2) + y - 1;
                        testMoveY += motion.Y * spriteSpeed;
                        if (testMoveY > maxY)
                        {
                            testMoveY = maxY;
                        }
                        testMoveY -= 40;
                        return testMoveY;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveY = 0;
                        return testMoveY;
                    }
                }
            }
            else
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X - 1, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X, currentCellPosition.Y + 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas lyg");
                        int x = (int)currentPosition.X % Engine.TileWidth;
                        int y = Engine.TileHeight / 2 - x / 2;
                        int maxY = currentCellPosition.Y * (Engine.TileHeight / 2) + y - 1;
                        testMoveY += motion.Y * spriteSpeed;
                        if (testMoveY > maxY)
                        {
                            testMoveY = maxY;
                        }
                        testMoveY -= 40;
                        return testMoveY;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveY = 0;
                        return testMoveY;
                    }
                }
            }

            testMoveY = 0;
            return testMoveY;
        }

        private float TopLeftColDetY(Vector2 motion, Vector2 currentPosition, Point currentCellPosition)
        {
            float testMoveY = currentPosition.Y;

            bool testTile = false;
            bool testTile2 = false;

            if (currentCellPosition.Y % 2 == 1)
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X + 1, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X, currentCellPosition.Y - 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas nelyg");
                        int x = (int)(currentPosition.X + Engine.TileWidth / 2) % Engine.TileWidth;
                        int y = Engine.TileHeight / 2 - x / 2;
                        int maxY = (currentCellPosition.Y - 2) * (Engine.TileHeight / 2) + y + 1;
                        testMoveY += motion.Y * spriteSpeed;
                        if (testMoveY < maxY)
                        {
                            testMoveY = maxY;
                        }
                        testMoveY -= 40;
                        return testMoveY;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveY = 0;
                        return testMoveY;
                    }
                }
            }
            else
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X - 1, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X - 1, currentCellPosition.Y - 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas lyg");
                        int x = (int)(currentPosition.X) % Engine.TileWidth;
                        int y = Engine.TileHeight / 2 - x / 2;
                        int maxY = (currentCellPosition.Y - 2) * (Engine.TileHeight / 2) + y + 1;
                        testMoveY += motion.Y * spriteSpeed;
                        if (testMoveY < maxY)
                        {
                            testMoveY = maxY;
                        }
                        testMoveY -= 40;
                        return testMoveY;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveY = 0;
                        return testMoveY;
                    }
                }
            }

            testMoveY = 0;
            return testMoveY;
        }

        private float TopRightColDetY(Vector2 motion, Vector2 currentPosition, Point currentCellPosition)
        {
            float testMoveY = currentPosition.Y;

            bool testTile = false;
            bool testTile2 = false;

            if (currentCellPosition.Y % 2 == 1)
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X + 1, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X + 1, currentCellPosition.Y - 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas nelyg");
                        int x = (int)(currentPosition.X + Engine.TileWidth / 2) % Engine.TileWidth;
                        int y = x / 2;
                        int maxY = (currentCellPosition.Y - 3)* (Engine.TileHeight / 2) + y + 1;
                        testMoveY += motion.Y * spriteSpeed;
                        if (testMoveY < maxY)
                        {
                            testMoveY = maxY;
                        }
                        testMoveY -= 40;
                        return testMoveY;

                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveY = 0;
                        return testMoveY;
                    }
                }
            }
            else
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X - 1, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X, currentCellPosition.Y - 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas lyg");

                        int x = (int)currentPosition.X % Engine.TileWidth;
                        int y = x / 2;
                        int maxY = (currentCellPosition.Y - 3) * (Engine.TileHeight / 2) + y + 1;
                        testMoveY += motion.Y * spriteSpeed;
                        if (testMoveY < maxY)
                        {
                            testMoveY = maxY;
                        }
                        testMoveY -= 40;
                        return testMoveY;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveY = 0;
                        return testMoveY;
                    }
                }
            }

            testMoveY = 0;
            return testMoveY;
        }
        //Laikinos funkcijos istrizoms kliutims atpazinti x asiai
        private float BottomLeftColDetX(Vector2 motion, Vector2 currentPosition, Point currentCellPosition)
        {
            float testMoveX = currentPosition.X;

            bool testTile = false;
            bool testTile2 = false;

            if (currentCellPosition.Y % 2 == 1)
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X + 1, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X, currentCellPosition.Y + 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas nelyg");
                        int y = (int)(currentPosition.Y - 16) % (Engine.TileHeight);
                        int x = (int)(currentCellPosition.X) * 64;
                        int minX = x - (64 - 2 * (y + 1)) + 1;
                        testMoveX += motion.X * spriteSpeed;
                        if (testMoveX < minX)
                        {
                            testMoveX = minX;
                        }
                        testMoveX -= 24;
                        return testMoveX;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveX = 0;
                        return testMoveX;
                    }
                }
            }
            else
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X - 1, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X - 1, currentCellPosition.Y + 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas lyg");
                        int y = (int)currentPosition.Y % (Engine.TileHeight);
                        int x = (int)(currentCellPosition.X) * 64 - 32;
                        int minX = x - (64 - 2 * (y + 1)) + 1;
                        testMoveX += motion.X * spriteSpeed;
                        if (testMoveX < minX)
                        {
                            testMoveX = minX;
                        }
                        testMoveX -= 24;
                        return testMoveX;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveX = 0;
                        return testMoveX;
                    }
                }
            }

            testMoveX = 0;
            return testMoveX;
        }

        private float BottomRightColDetX(Vector2 motion, Vector2 currentPosition, Point currentCellPosition)
        {
            float testMoveX = currentPosition.X;

            bool testTile = false;
            bool testTile2 = false;

            if (currentCellPosition.Y % 2 == 1)
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X + 1, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X + 1, currentCellPosition.Y + 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas nelyg");
                        int y = (int)(currentPosition.Y - 16) % (Engine.TileHeight);
                        int x = (int)(currentCellPosition.X) * 64;
                        int minX = x + (64 - 2 * (y + 1));
                        testMoveX += motion.X * spriteSpeed;
                        if (testMoveX > minX)
                        {
                            testMoveX = minX;
                        }
                        testMoveX -= 24;
                        return testMoveX;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveX = 0;
                        return testMoveX;
                    }
                }
            }
            else
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X - 1, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X, currentCellPosition.Y + 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas lyg");
                        int y = (int)currentPosition.Y % (Engine.TileHeight);
                        int x = (int)(currentCellPosition.X) * 64 - 32;
                        int minX = x + (64 - 2 * (y + 1));
                        testMoveX += motion.X * spriteSpeed;
                        if (testMoveX > minX)
                        {
                            testMoveX = minX;
                        }
                        testMoveX -= 24;
                        return testMoveX;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveX = 0;
                        return testMoveX;
                    }
                }
            }

            testMoveX = 0;
            return testMoveX;
        }

        private float TopLeftColDetX(Vector2 motion, Vector2 currentPosition, Point currentCellPosition)
        {
            float testMoveX = currentPosition.X;

            bool testTile = false;
            bool testTile2 = false;

            if (currentCellPosition.Y % 2 == 1)
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X + 1, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X, currentCellPosition.Y - 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas nelyg");
                        int y = (int)(currentPosition.Y - 16) % (Engine.TileHeight);
                        int x = (int)(currentCellPosition.X) * 64;
                        int minX = x - (2 * y) + 2;
                        testMoveX += motion.X * spriteSpeed;
                        if (testMoveX < minX)
                        {
                            testMoveX = minX;
                        }
                        testMoveX -= 24;
                        return testMoveX;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveX = 0;
                        return testMoveX;
                    }
                }
            }
            else
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X - 1, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X - 1, currentCellPosition.Y - 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas lyg");
                        int y = (int)currentPosition.Y % (Engine.TileHeight);
                        int x = (int)(currentCellPosition.X) * 64 - 32;
                        int minX = x - (2 * y) + 2;
                        testMoveX += motion.X * spriteSpeed;
                        if (testMoveX < minX)
                        {
                            testMoveX = minX;
                        }
                        testMoveX -= 24;
                        return testMoveX;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveX = 0;
                        return testMoveX;
                    }
                }
            }

            testMoveX = 0;
            return testMoveX;
        }

        private float TopRightColDetX(Vector2 motion, Vector2 currentPosition, Point currentCellPosition)
        {
            float testMoveX = currentPosition.X;

            bool testTile = false;
            bool testTile2 = false;

            if (currentCellPosition.Y % 2 == 1)
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X + 1, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X + 1, currentCellPosition.Y - 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas nelyg");
                        int y = (int)(currentPosition.Y - 16) % (Engine.TileHeight);
                        int x = (int)(currentCellPosition.X) * 64;
                        int minX = x + (2 * y) - 2;
                        testMoveX += motion.X * spriteSpeed;
                        if (testMoveX > minX)
                        {
                            testMoveX = minX;
                        }
                        testMoveX -= 24;
                        return testMoveX;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveX = 0;
                        return testMoveX;
                    }
                }
            }
            else
            {
                for (int i = map.Layers.Length - 1; i >= 0; i--)
                {
                    if (CheckTile(currentCellPosition.X, currentCellPosition.Y + 1, i))
                        testTile = true;
                    if (CheckTile(currentCellPosition.X - 1, currentCellPosition.Y - 1, i))
                        testTile2 = true;
                    bool testTile3 = CheckTile(currentCellPosition.X, currentCellPosition.Y - 1, i);
                    if (!testTile3 && testTile && testTile2)
                    {
                        //Console.WriteLine("Istrizas lyg");
                        int y = (int)currentPosition.Y % (Engine.TileHeight);
                        int x = (int)(currentCellPosition.X) * 64 - 32;
                        int minX = x + (2 * y) - 2;
                        testMoveX += motion.X * spriteSpeed;
                        if (testMoveX > minX)
                        {
                            testMoveX = minX;
                        }
                        testMoveX -= 24;
                        return testMoveX;
                    }
                    else if (testTile3 && testTile && testTile2)
                    {
                        testMoveX = 0;
                        return testMoveX;
                    }
                }
            }

            testMoveX = 0;
            return testMoveX;
        }

        private bool ObstaclesCheck(TileData tileL, TileData tile, TileData tileR, int i)
        {
            bool isObstacle = false;
            if (tileL.TileIndex != -1 && tileL.TileSetIndex != -1 &&
                tile.TileIndex != -1 && tile.TileSetIndex != -1 &&
                tileR.TileIndex != -1 && tileR.TileSetIndex != -1 &&
                map.Layers[i].LayerLevel == 0 || map.Layers[i].LayerLevel % 2 == 1 || map.Layers[i].LayerLevel == -1)
                isObstacle = true;

            return isObstacle;
        }

        private bool ObstaclesCheckCancel(TileData tileL, TileData tile, TileData tileR, int i)
        {
            bool notObstacle = false;
            if (tileL.TileIndex == -1 && tileL.TileSetIndex == -1 &&
                tile.TileIndex == -1 && tile.TileSetIndex == -1 &&
                tileR.TileIndex == -1 && tileR.TileSetIndex == -1 &&
                map.Layers[i].LayerLevel == -1)
                notObstacle = true;
            return notObstacle;
        }

        //private bool ObstaclesCheck(TileData tileL, TileData tile, TileData tileR, int i)
        //{
        //    bool isObstacle = false;
        //    if (tileL.TileIndex != -1 && tileL.TileSetIndex != -1 &&
        //        tile.TileIndex != -1 && tile.TileSetIndex != -1 &&
        //        map.Layers[i].LayerLevel == 0 || map.Layers[i].LayerLevel % 2 == 1)
        //        isObstacle = true;
        //    if (tile.TileIndex != -1 && tile.TileSetIndex != -1 &&
        //        tileR.TileIndex != -1 && tileR.TileSetIndex != -1 &&
        //        map.Layers[i].LayerLevel == 0 || map.Layers[i].LayerLevel % 2 == 1)
        //        isObstacle = true;
        //    Console.WriteLine("TL: {0}, T: {1}, TR: {2}", tileL.TileIndex, tile.TileIndex, tileR.TileIndex);
        //    Console.WriteLine("isObstacle: {0}", isObstacle);
        //    return isObstacle;
        //}

        private bool CancelCheck(TileData tileL, TileData tile, TileData tileR)
        {
            bool cancel = false;
            if (tile.TileIndex != -1 && tile.TileSetIndex != -1 ||
                tileL.TileIndex != -1 && tileL.TileSetIndex != -1 ||
                tileR.TileIndex != -1 && tileR.TileSetIndex != -1)
                cancel = true;
            return cancel;
        }
        //private bool CancelCheck(TileData tileL, TileData tile, TileData tileR)
        //{
        //    bool cancel = false;
        //    if (tile.TileIndex != -1 && tile.TileSetIndex != -1 &&
        //        tileL.TileIndex != -1 && tileL.TileSetIndex != -1)
        //        cancel = true;
        //    if (tile.TileIndex != -1 && tile.TileSetIndex != -1 &&
        //        tileR.TileIndex != -1 && tileR.TileSetIndex != -1)
        //        cancel = true;
        //    return cancel;
        //}

        private bool CheckTile(int x, int y, int i)
        {
            bool isFilled = false;
            TileData tile = map.Layers[i].GetTile(x, y);
            if (tile.TileIndex != -1 && tile.TileSetIndex != -1 &&
                map.Layers[i].LayerLevel % 2 == 0 && map.Layers[i].LayerLevel != 0)
                isFilled = true;

            return isFilled;
        }

        private bool CheckAnomaly(TileData tile1, TileData tile2, int j)
        {
            bool cancel = false;
            if ((tile1.TileIndex != -1 && tile1.TileSetIndex != -1 &&
                tile2.TileIndex == -1 && tile2.TileSetIndex == -1) &&
                map.Layers[j].LayerLevel % 2 == 0 && map.Layers[j].LayerLevel != 0)
            {
                cancel = true;
            }
            if ((tile1.TileIndex == -1 && tile1.TileSetIndex == -1 &&
                tile2.TileIndex != -1 && tile2.TileSetIndex != -1) &&
                map.Layers[j].LayerLevel % 2 == 0 && map.Layers[j].LayerLevel != 0)
            {
                cancel = true;
            }
            return cancel;
        }
        #endregion
    }
}
