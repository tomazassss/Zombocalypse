using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XRPGLibrary.Controls
{
    public class ScrollBar : IControl
    {
        private const float DEFAULT_MIN_VALUE = 0;
        private const float DEFAULT_MAX_VALUE = 1;

        private Texture2D scrollerTexture;
        private Texture2D scrollTrackTexture;

        private Vector2 size;
        private Vector2 position;
        private Vector2 scrollerPosition;
        private Vector2 scrollerSize;

        private float value;
        private float maxValue;
        private float minValue;

        private bool hasFocus;
        private bool enabled;
        private bool visible;

        private bool isDragged;

        public event EventHandler ValueChanged;

        public Vector2 Size
        {
            get { return size; }
            set
            {
                this.size = value;
                PositionScroller();
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                this.position = value;
                PositionScroller();
            }
        }

        public float Value
        {
            get { return value; }
            set
            {
                this.value = value;

                float offset = (size.Y - scrollerSize.Y) * (value / maxValue);
                scrollerPosition.Y = position.Y + offset;
                if (ValueChanged != null)
                {
                    ValueChanged(this, null);
                }
            }
        }

        public bool TabStop
        {
            get { return false; }
            set { }
        }

        public bool HasFocus
        {
            get { return hasFocus; }
            set { this.hasFocus = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { this.enabled = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { this.visible = value; }
        }

        public ScrollBar(Texture2D scrollerTexture, Texture2D scrollTrackTexture)
            : this(scrollerTexture, scrollTrackTexture,
                   DEFAULT_MIN_VALUE, DEFAULT_MAX_VALUE)
        {
        }


        public ScrollBar(Texture2D scrollerTexture, Texture2D scrollTrackTexture,
                         float minValue, float maxValue)
        {
            if (minValue > maxValue)
            {
                throw new Exception("Minimum value cannot be larger than maximum value");
            }
            if (minValue < 0 || maxValue < 0)
            {
                throw new Exception("Minimum/maximum value cannot be less than zero");
            }

            this.scrollerTexture = scrollerTexture;
            this.scrollTrackTexture = scrollTrackTexture;

            this.scrollerSize = new Vector2(scrollerTexture.Width, scrollerTexture.Height);

            this.minValue = minValue;
            this.maxValue = maxValue;
            this.value = minValue;

            this.Size = new Vector2(scrollerTexture.Width, 0);
            this.isDragged = false;
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = InputHandler.MouseState;
            if (IsMouseInsideScroller(mouseState) && InputHandler.LeftButtonPressed())
            {
                isDragged = true;
            }
            else if (isDragged && !InputHandler.LeftButtonDown())
            {
                isDragged = false;
            }

            if (isDragged)
            {
                Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
                mousePosition.Y = MathHelper.Clamp(
                    mousePosition.Y,
                    position.Y,
                    position.Y + size.Y - scrollerSize.Y);

                Value = CalculateValue(mousePosition);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle scrollTrackRectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)size.X,
                (int)size.Y);

            spriteBatch.Draw(scrollTrackTexture, scrollTrackRectangle, Color.White);

            Rectangle scrollerRectangle = new Rectangle(
                (int)scrollerPosition.X,
                (int)scrollerPosition.Y,
                (int)scrollerSize.X,
                (int)scrollerSize.Y);

            spriteBatch.Draw(scrollerTexture, scrollerRectangle, Color.White);
        }

        public void HandleInput()
        {
        }

        private void PositionScroller()
        {
            float offset = (size.Y - scrollerSize.Y) * (value / maxValue);

            scrollerPosition.Y = position.Y + offset;
            scrollerPosition.X = position.X;
        }

        private float CalculateValue(Vector2 newPosition)
        {
            float scrollableHeight = size.Y - scrollerSize.Y;
            float value = ((scrollableHeight - (position.Y + scrollableHeight - newPosition.Y)) / (scrollableHeight)) * maxValue;
            value = MathHelper.Clamp(value, minValue, maxValue);

            return value;
        }

        private bool IsMouseInsideScroller(MouseState mouseState)
        {
            Rectangle mouseRectangle = new Rectangle(
                mouseState.X,
                mouseState.Y,
                1, 
                1);

            Rectangle scrollerRectangle = new Rectangle(
                (int)scrollerPosition.X,
                (int)scrollerPosition.Y,
                (int)scrollerSize.X,
                (int)scrollerSize.Y);

            return mouseRectangle.Intersects(scrollerRectangle);
        }
    }
}
