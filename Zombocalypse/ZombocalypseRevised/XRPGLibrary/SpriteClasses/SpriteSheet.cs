using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace XRPGLibrary.SpriteClasses
{
    public class SpriteSheet
    {
        private Texture2D spriteSheet;
        private int animationFrames;

        public Texture2D SpriteSheet1
        {
            get { return spriteSheet; }
            set { spriteSheet = value; }
        }

        public int AnimationFrames
        {
            get { return animationFrames; }
            set { animationFrames = value; }
        }

        public SpriteSheet(Texture2D sheet, int frameCount)
        {
            this.spriteSheet = sheet;
            this.animationFrames = frameCount;
        }
    }
}
