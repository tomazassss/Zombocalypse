using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Zombocalypse
{
    public class Ai
    {
        SpriteAnimation aiAnim;
        int counter = 0;

        public Ai()
        {
        }

        public Ai(SpriteAnimation anim)
        {
            aiAnim = anim;
            aiAnim.AddAnimation("WalkEast", 0, 48 * 0, 48, 48, 8, 0.1f);
            aiAnim.AddAnimation("WalkNorth", 0, 48 * 1, 48, 48, 8, 0.1f);
            aiAnim.AddAnimation("WalkNorthEast", 0, 48 * 2, 48, 48, 8, 0.1f);
            aiAnim.AddAnimation("WalkNorthWest", 0, 48 * 3, 48, 48, 8, 0.1f);
            aiAnim.AddAnimation("WalkSouth", 0, 48 * 4, 48, 48, 8, 0.1f);
            aiAnim.AddAnimation("WalkSouthEast", 0, 48 * 5, 48, 48, 8, 0.1f);
            aiAnim.AddAnimation("WalkSouthWest", 0, 48 * 6, 48, 48, 8, 0.1f);
            aiAnim.AddAnimation("WalkWest", 0, 48 * 7, 48, 48, 8, 0.1f);

            aiAnim.AddAnimation("IdleEast", 0, 48 * 0, 48, 48, 1, 0.2f);
            aiAnim.AddAnimation("IdleNorth", 0, 48 * 1, 48, 48, 1, 0.2f);
            aiAnim.AddAnimation("IdleNorthEast", 0, 48 * 2, 48, 48, 1, 0.2f);
            aiAnim.AddAnimation("IdleNorthWest", 0, 48 * 3, 48, 48, 1, 0.2f);
            aiAnim.AddAnimation("IdleSouth", 0, 48 * 4, 48, 48, 1, 0.2f);
            aiAnim.AddAnimation("IdleSouthEast", 0, 48 * 5, 48, 48, 1, 0.2f);
            aiAnim.AddAnimation("IdleSouthWest", 0, 48 * 6, 48, 48, 1, 0.2f);
            aiAnim.AddAnimation("IdleWest", 0, 48 * 7, 48, 48, 1, 0.2f);

            aiAnim.Position = new Vector2(GeneratePosition(), GeneratePosition());
            aiAnim.DrawOffset = new Vector2(-24, -38);
            aiAnim.CurrentAnimation = "WalkEast";
            aiAnim.IsAnimating = true;
        }
        public int GeneratePosition()
        {
            Random random = new Random();
            return random.Next(36, 500);
        }
        public void DrawAi(TileMap myMap, SpriteBatch spriteBatch)
        {
            Point aiStandingOn = myMap.WorldToMapCell(new Point((int)aiAnim.Position.X, (int)aiAnim.Position.Y));
            int aiHeight = myMap.Rows[aiStandingOn.Y].Columns[aiStandingOn.X].HeightTiles.Count * Tile.HeightTileOffset;
            aiAnim.Draw(spriteBatch, 0, -aiHeight);
        }
        public void MoveAi(Vector2 playerPosition)
        {

            if (CheckMove(playerPosition, aiAnim.Position))
            {
                if (counter >= 50)
                {
                    moveRandom();
                    counter = 0;
                }
                else
                {
                    counter++;
                }            
            }
            else
            {
                if (counter >= 5)
                {
                    moveToPlayer(playerPosition);
                    counter = 0;
                }
                else
                {
                    counter++;
                }    
            }

        }
        public bool CheckMove(Vector2 playerPosition, Vector2 animationPosition)
        {
            bool moveRandom = true;
            if (rangeBetween(playerPosition, animationPosition) > 200)
            {
                moveRandom = true;
            }
            else
            {
                moveRandom = false;
            }
            return moveRandom;
        }
        public double rangeBetween(Vector2 playerPosition)
        {
            double range = rangeBetween(playerPosition, aiAnim.Position);
            return range;
        }
        public double rangeBetween(Vector2 playerPosition, Vector2 animationPosition)
        {
            double range = Math.Sqrt(
                                Math.Pow(playerPosition.X - animationPosition.X, 2) +
                                Math.Pow(playerPosition.Y - animationPosition.Y, 2));
            return range;

        }
        public void moveRandom()
        {
            Vector2 moveVector = Vector2.Zero;
            Vector2 moveDir = Vector2.Zero;
            string animation = "";
            int direction = 0;

            Random random = new Random();
            direction = random.Next(0, 3);
            switch (direction)
            {
                case 0:
                    moveDir = new Vector2(0, -2);
                    animation = "WalkNorth";
                    moveVector += new Vector2(0, -2);
                    break;
                case 1:
                    moveDir = new Vector2(-2, 0);
                    animation = "WalkWest";
                    moveVector += new Vector2(-2, 0);
                    break;
                case 2:
                    moveDir = new Vector2(2, 0);
                    animation = "WalkEast";
                    moveVector += new Vector2(2, 0);
                    break;
                case 3:
                    moveDir = new Vector2(0, 2);
                    animation = "WalkSouth";
                    moveVector += new Vector2(0, 2);
                    break;
                default:
                    moveDir = new Vector2(0, 0);
                    animation = "Idle";
                    moveVector += new Vector2(0, 0);
                    break;
            }
            SetAnimationPosition(moveDir, animation);

            aiAnim.Position = SetAiPosition();
        }
        public void moveToPlayer(Vector2 playerPosition)
        {
            Vector2 moveVector = Vector2.Zero;
            Vector2 moveDir = Vector2.Zero;
            string animation = "";
            if (playerPosition.X - aiAnim.Position.X >= 0)
            {
                moveDir = new Vector2(1, 0);
                animation = "WalkEast";
                moveVector += new Vector2(1, 0);
            }
            else
            {
                moveDir = new Vector2(-1, 0);
                animation = "WalkWest";
                moveVector += new Vector2(-1, 0);
            }
            if (playerPosition.Y - aiAnim.Position.Y >= 0)
            {
                moveDir = new Vector2(0, 1);
                animation = "WalkSouth";
                moveVector += new Vector2(0, 1);
            }
            else
            {
                moveDir = new Vector2(0, -1);
                animation = "WalkNorth";
                moveVector += new Vector2(0, -1);
            }
            if (playerPosition.X - aiAnim.Position.X >= 0 && playerPosition.Y - aiAnim.Position.Y >= 0)
            {
                moveDir = new Vector2(1, 1);
                animation = "WalkSouthEast";
                moveVector += new Vector2(1, 1);
            }
            if (playerPosition.X - aiAnim.Position.X < 0 && playerPosition.Y - aiAnim.Position.Y < 0)
            {
                moveDir = new Vector2(-1, -1);
                animation = "WalkNorthWest";
                moveVector += new Vector2(-1, -1);
            }
            if (playerPosition.X - aiAnim.Position.X >= 0 && playerPosition.Y - aiAnim.Position.Y < 0)
            {
                moveDir = new Vector2(1, -1);
                animation = "WalkNorthEast";
                moveVector += new Vector2(1, -1);
            }
            if (playerPosition.X - aiAnim.Position.X < 0 && playerPosition.Y - aiAnim.Position.Y >= 0)
            {
                moveDir = new Vector2(-1, 1);
                animation = "WalkSouthWest";
                moveVector += new Vector2(-1, 1);
            }

            SetAnimationPosition(moveDir, animation);

            aiAnim.Position = SetAiPosition();
        }
        public Vector2 SetAiPosition()
        {
            float vladX = MathHelper.Clamp(
                aiAnim.Position.X, 0 + aiAnim.DrawOffset.X, Camera.WorldWidth);
            float vladY = MathHelper.Clamp(
                aiAnim.Position.Y, 0 + aiAnim.DrawOffset.Y, Camera.WorldHeight);

            return new Vector2(vladX, vladY);
        }
        public void SetAnimationPosition(Vector2 moveDir, string animation)
        {
            if (moveDir.Length() != 0)
            {
                aiAnim.MoveBy((int)moveDir.X, (int)moveDir.Y);
                if (aiAnim.CurrentAnimation != animation)
                {
                    aiAnim.CurrentAnimation = animation;
                }
            }
        }
    }
}
