﻿using System;
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
        Texture2D texture;
        Vector2 position;
        Vector2 velocity;
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
        public int Width
        {
            get { return animations[currentAnimation].FrameWidth; }
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
            set
            {
                position = value;
            }
        }
        public Vector2 Velocity
        {
            get { return velocity; }
            set
            {
                velocity = value;
                if (velocity != Vector2.Zero)
                    velocity.Normalize();
            }
        }
        #endregion

        #region Constructor Region
        public AnimatedSprite(Texture2D sprite, Dictionary<AnimationKey, Animation> animation)
        {
            texture = sprite;
            animations = new Dictionary<AnimationKey, Animation>();
            foreach (AnimationKey key in animation.Keys)
                animations.Add(key, (Animation)animation[key].Clone());
        }
        #endregion

        #region Method Region
        public void Update(GameTime gameTime)
        {
            if (isAnimating)
                animations[currentAnimation].Update(gameTime);
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
