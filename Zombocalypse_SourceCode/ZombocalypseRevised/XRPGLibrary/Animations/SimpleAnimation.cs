using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XRPGLibrary.Animations
{
    public abstract class SimpleAnimation
    {
        protected Texture2D animationTexture;
        protected Vector2 size;
        protected Vector2 position;
        protected int currentFrame;
        protected int frameCount;
        protected int frameLength;
        protected int elapsedTime;
        
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
    }
}
