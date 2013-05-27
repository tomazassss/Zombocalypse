using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XRPGLibrary.Animations
{
    public class FloatTextAnimation : SimpleAnimation
    {
        #region Field Region

        private SpriteFont textFont;
        private Color color;
        private String text;

        private Vector2 position;
        private Vector2 startPosition;
        private Vector2 endPosition;

        private bool isDrawing;

        private float drawTime;
        private float drawTimeLeft;

        private float alphaValue;
        private float fadeIncrement;

        #endregion

        #region Property Region

        public SpriteFont TextFont
        {
            get { return textFont; }
            set { textFont = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public String Text
        {
            get { return text; }
            set { text = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        public Vector2 EndPosition
        {
            get { return endPosition; }
            set { endPosition = value; }
        }

        public bool IsDrawing
        {
            get { return isDrawing; }
            set { isDrawing = value; }
        }

        public float DrawTime
        {
            get { return drawTime; }
            set { 
                    drawTime = value;
                    DrawTimeLeft = value;
                }
        }

        public float DrawTimeLeft
        {
            get { return drawTimeLeft; }
            set { 
                    drawTimeLeft = value;
                    if (drawTimeLeft <= (float)0.0)
                    {
                        drawTimeLeft = 0.0f;
                        alphaValue = 0f;
                        isDrawing = false;
                    }
                }
        }

        #endregion

        #region Constructor Region

        public FloatTextAnimation()
        {
            this.textFont = null;
            this.color = Color.Red;
            this.text = "";
            this.position = Vector2.Zero;
            this.startPosition = Vector2.Zero;
            this.endPosition = Vector2.Zero;
            this.isDrawing = false;
            this.drawTime = 0.0f;
            this.drawTimeLeft = 0.0f;
            this.alphaValue = 0f;
            this.fadeIncrement = 0.005f;
        }

        #endregion

        #region Method Region

        public override void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Vector2.Zero);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (isDrawing)
            {
                spriteBatch.DrawString(textFont,
                                        text,
                                        new Vector2(position.X - offset.X, position.Y - offset.Y),
                                        Color.Lerp(color, Color.Transparent, alphaValue));
            }
        }

        public override void Update(GameTime gameTime)
        {
            DrawTimeLeft -= gameTime.ElapsedGameTime.Milliseconds;

            if (isDrawing)
            {
                alphaValue += fadeIncrement;
                float dx = (float)Math.Abs(startPosition.X - endPosition.X);
                float dy = (float)Math.Abs(startPosition.Y - endPosition.Y);

                dx *= ((DrawTime - DrawTimeLeft) / DrawTime);

                if (endPosition.X > position.X)
                {
                    position.X = startPosition.X + dx;
                    position.X = MathHelper.Clamp(position.X, startPosition.X, endPosition.X);
                }
                else
                {
                    position.X = startPosition.X - dx;
                    position.X = MathHelper.Clamp(position.X, endPosition.X, startPosition.X);
                }

                dy *= ((DrawTime - DrawTimeLeft) / DrawTime);

                if (startPosition.Y <= endPosition.Y)
                {
                    if (endPosition.Y > position.Y)
                    {
                        position.Y = startPosition.Y + dy;
                        position.Y = MathHelper.Clamp(position.Y, startPosition.Y, endPosition.Y);
                    }
                    else
                    {
                        position.Y = startPosition.Y - dy;
                        position.Y = MathHelper.Clamp(position.Y, endPosition.Y, startPosition.Y);
                    }
                }
                else
                {
                    if (startPosition.Y > position.Y)
                    {
                        position.Y = startPosition.Y - dy;
                        position.Y = MathHelper.Clamp(position.Y, endPosition.Y, startPosition.Y);
                    }
                    else
                    {
                        position.Y = startPosition.Y + dy;
                        position.Y = MathHelper.Clamp(position.Y, startPosition.Y, endPosition.Y);
                    }
                }
                if (endPosition.Equals(position) && !endPosition.Equals(startPosition))
                {
                    isDrawing = false;
                    alphaValue = 0f;
                }
            }
        }

        public void StartDrawing(bool isDrawing, string text, Vector2 startPos, Vector2 endPos, float duration)
        {
            IsDrawing = isDrawing;
            Text = text;
            StartPosition = startPos;
            EndPosition = endPos;
            DrawTime = duration;
            alphaValue = 0f;
        }

        #endregion
    }
}
