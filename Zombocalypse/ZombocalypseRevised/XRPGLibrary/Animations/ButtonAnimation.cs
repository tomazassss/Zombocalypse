using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XRPGLibrary.Animations
{
    public class ButtonAnimation : SimpleAnimation
    {
        public Vector2 Size
        {
            get { return this.size; }
            set
            {
                this.size = value;
                this.frameCount = animationTexture.Height / (int)size.Y;
            }
        }

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public Texture2D AnimationTexture
        {
            get { return this.animationTexture; }
            set
            {
                this.animationTexture = value;
                this.frameCount = animationTexture.Height / (int)size.Y;
            }
        }

        public ButtonAnimation(Texture2D animationTexture, Vector2 size, Vector2 position, int frameLength)
        {
            this.animationTexture = animationTexture;
            this.size = size;
            this.position = position;
            this.currentFrame = 0;
            this.elapsedTime = 0;
            this.frameCount = animationTexture.Height / (int)size.Y;
            this.frameLength = frameLength;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            Rectangle sourceRectangle = new Rectangle(0, (int)size.Y * currentFrame, (int)size.X, (int)size.Y);

            spriteBatch.Draw(
                animationTexture,
                destinationRectangle,
                sourceRectangle,
                Color.White);

        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime >= frameLength)
            {
                currentFrame++;
                elapsedTime = 0;
                if (currentFrame >= frameCount)
                {
                    currentFrame = 0;
                }
            }
        }

        public void Reset()
        {
            if (elapsedTime != 0)
            {
                elapsedTime = 0;
            }

            if (currentFrame != 0)
            {
                currentFrame = 0;
            }
        }
    }
}
