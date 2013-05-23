using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using XRPGLibrary.TileEngine.TileMaps;
using XRPGLibrary.TileEngine;

namespace XRPGLibrary.SpriteClasses
{
    public class AnimatedSprite
    {
        #region Field Region

        Dictionary<AnimationKey, Animation> animations;
        AnimationKey currentAnimation;
        bool isAnimating;
        bool animateOnce;
        bool animateOnceAndResume;

        Texture2D texture;

        Vector2 position;
        Vector2 velocity;
        protected Vector2 size;
        float speed = 2.0f;

        #endregion

        #region Property Region

        public AnimationKey CurrentAnimation
        {
            get { return currentAnimation; }
            set { currentAnimation = value; }
        }

        public bool IsAnimating
        {
            get { return isAnimating; }
            set { isAnimating = value; }
        }

        public bool AnimateOnce
        {
            get { return animateOnce; }
            set { animateOnce = value; }
        }

        public bool AnimateOnceAndResume
        {
            get { return animateOnceAndResume; }
            set { animateOnceAndResume = value; }
        }

        public int Width
        {
            get { return animations[currentAnimation].FrameWidth; }
        }

        public Rectangle CurrentAnimationRectangle
        {
            get { return animations[currentAnimation].CurrentFrameRect; }
        }

        public int Height
        {
            get { return animations[currentAnimation].FrameHeight; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = MathHelper.Clamp(speed, 1.0f, 16.0f); }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set
            {
                velocity = value;
                if (velocity != Vector2.Zero)
                {
                    velocity.Normalize();
                }
            }
        }

        public Vector2 Origin
        {
            get
            {
                Vector2 origin = position + new Vector2(size.X / 2, size.Y / 2);
                return origin;
            }
        }

        #endregion

        #region Constructor Region

        public AnimatedSprite(Texture2D sprite, Dictionary<AnimationKey, Animation> animation)
        {
            this.animateOnce = false;
            this.animateOnceAndResume = false;
            texture = sprite;
            animations = new Dictionary<AnimationKey, Animation>();
            foreach (AnimationKey key in animation.Keys)
            {
                if (size == Vector2.Zero)
                {
                    size = new Vector2(animation[key].FrameWidth, animation[key].FrameHeight);
                }
                animations.Add(key, (Animation)animation[key].Clone());
            }
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            if (isAnimating)
            {
                if (!animateOnce && !animateOnceAndResume)
                {
                    animations[currentAnimation].Update(gameTime);
                }
                else if (animateOnce)
                {
                    animations[currentAnimation].AnimateOnce = true;
                    animations[currentAnimation].Update(gameTime);
                }
                else if (animateOnceAndResume)
                {
                    animations[currentAnimation].AnimateOnceAndResume = true;
                    animations[currentAnimation].Update(gameTime);
                    if (animations[currentAnimation].AnimateOnceAndResume != true)
                        animateOnceAndResume = false;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(
                texture,
                position - camera.Position,
                animations[currentAnimation].CurrentFrameRect,
                Color.White);
        }

        public void LockToMap()
        {
            position.X = MathHelper.Clamp(position.X, 0, ATileMap.WidthInPixels - Width);
            position.Y = MathHelper.Clamp(position.Y, 0, ATileMap.HeightInPixels - Height);
        }

        #endregion
    }
}
