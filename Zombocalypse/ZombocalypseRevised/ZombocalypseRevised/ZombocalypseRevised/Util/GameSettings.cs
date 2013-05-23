using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZombocalypseRevised.Util
{
    public class GameSettings
    {
        private static Rectangle screenRectangle;

        public static Rectangle ScreenRectangle
        {
            get { return screenRectangle; }
            set { screenRectangle = value; }
        }
    }
}
