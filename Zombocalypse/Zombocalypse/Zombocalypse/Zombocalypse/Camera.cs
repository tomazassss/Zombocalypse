using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zombocalypse
{
    public static class Camera
    {
        static public Vector2 location = Vector2.Zero;
        public static int ViewWidth { get; set; }
        public static int ViewHeight { get; set; }
        public static int WorldWidth { get; set; }
        public static int WorldHeight { get; set; }

        public static Vector2 DisplayOffset { get; set; }

        public static Vector2 Location
        {
            get
            {
                return location;
            }
            set
            {
                location = new Vector2(
                    MathHelper.Clamp(value.X, 0f, WorldWidth - ViewWidth),
                    MathHelper.Clamp(value.Y, 0f, WorldHeight - ViewHeight));
            }
        }

        public static Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return WorldToScreen(worldPosition, Location, DisplayOffset);
        }

        public static Vector2 WorldToScreen(Vector2 worldPosition, Vector2 loc, Vector2 offset)
        {
            return worldPosition - loc + offset;
        }

        public static Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return ScreenToWorld(screenPosition, Location, DisplayOffset);
        }

        public static Vector2 ScreenToWorld(Vector2 screenPosition, Vector2 loc, Vector2 offset)
        {
            return screenPosition + loc - offset;
        }

        public static void Move(Vector2 offset)
        {
            Location += offset;
        }
    }
}
