using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zombocalypse
{
    public class CollisionUtilities
    {
        public static bool IsColliding(Rectangle rectangleA, Rectangle rectangleB)
        {
            bool isColliding = rectangleA.Intersects(rectangleB);
            return isColliding;
        }
    }
}
