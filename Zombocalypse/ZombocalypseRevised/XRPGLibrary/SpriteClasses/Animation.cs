using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace XRPGLibrary.SpriteClasses
{
    public enum AnimationKey { Down, DownLeft, UpLeft, Left, DownRight, UpRight, Right, Up }

    public class Animation : ICloneable
    {
        #region Field Region
        Rectangle[] frames;
        int framesPerSecond;
        TimeSpan frameLength;
        TimeSpan frameTimer;
        int currentFrame;
        int frameWidth;
        int frameHeight;
        bool animateOnce;
        bool animateOnceAndResume;
        #endregion

        #region Property Region

        public int FramesPerSecond
        {
            get { return framesPerSecond; }
            set
            {
                if (value < 1)
                    framesPerSecond = 1;
                else if (value > 60)
                    framesPerSecond = 60;
                else
                    framesPerSecond = value;
                frameLength = TimeSpan.FromSeconds(1 / (double)framesPerSecond);
            }
        }

        public Rectangle CurrentFrameRect
        {
            get { return frames[currentFrame]; }
        }

        public int CurrentFrame
        {
            get { return currentFrame; }
            set
            {
                currentFrame = (int)MathHelper.Clamp(value, 0, frames.Length - 1);
            }
        }

        public int FrameWidth
        {
            get { return frameWidth; }
        }

        public int FrameHeight
        {
            get { return frameHeight; }
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

        #endregion

        #region Constructor Region

        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset)
        {
            frames = new Rectangle[frameCount];
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.animateOnce = false;
            this.animateOnceAndResume = false;
            for (int i = 0; i < frameCount; i++)
            {
                frames[i] = new Rectangle(
                xOffset + (frameWidth * i),
                yOffset,
                frameWidth,
                frameHeight);
            }
            FramesPerSecond = 12;    
            Reset();
        }

        private Animation(Animation animation)
        {
            this.frames = animation.frames;
            FramesPerSecond = 12;
        }
        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            frameTimer += gameTime.ElapsedGameTime;
            if (frameTimer >= frameLength)
            {
                if (!animateOnce && !animateOnceAndResume)
                {
                    frameTimer = TimeSpan.Zero;
                    currentFrame = (currentFrame + 1) % frames.Length;
                }
                else if (animateOnce)
                {
                    if ((currentFrame + 1) == frames.Length)
                    {
                        frameTimer = TimeSpan.Zero;
                        currentFrame = frames.Length - 1;
                    }
                    else
                    {
                        frameTimer = TimeSpan.Zero;
                        currentFrame = (currentFrame + 1) % frames.Length;
                    }
                } else if (animateOnceAndResume)
                {
                    if ((currentFrame + 1) == frames.Length)
                    {
                        frameTimer = TimeSpan.Zero;
                        currentFrame = (currentFrame + 1) % frames.Length;
                        animateOnceAndResume = false;
                    }
                    else
                    {
                        frameTimer = TimeSpan.Zero;
                        currentFrame = (currentFrame + 1) % frames.Length;
                    }
                }
            }
        }

        public void Reset()
        {
            currentFrame = 0;
            frameTimer = TimeSpan.Zero;
        }

        #endregion

        #region Interface Method Region

        public object Clone()
        {
            Animation animationClone = new Animation(this);
            animationClone.frameWidth = this.frameWidth;
            animationClone.frameHeight = this.frameHeight;
            animationClone.Reset();
            return animationClone;
        }

        #endregion
    }
}
