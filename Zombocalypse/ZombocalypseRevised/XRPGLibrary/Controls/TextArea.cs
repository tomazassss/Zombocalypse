using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.Util;
using Microsoft.Xna.Framework.Content;

namespace XRPGLibrary.Controls
{
    public class TextArea : IControl
    {
        private ScrollBar scrollBar;

        private SpriteFont font;

        private String text;
        private Vector2 size;
        private Vector2 position;

        private bool tabStop;
        private bool hasFocus;
        private bool enabled;
        private bool visible;

        private bool useScrollBar;
        private float textOffset;

        public String Text
        {
            get { return text; }
            set
            {
                if (size != Vector2.Zero)
                {
                    scrollBar.Value = 0;
                    this.text = value;
                    this.text = StringUtils.WrapText(this.text, font, this.size.X);
                    Vector2 textSize = font.MeasureString(this.text);
                    if (textSize.Y > this.size.Y)
                    {
                        useScrollBar = true;
                    }
                    else
                    {
                        useScrollBar = false;
                    }
                }
            }
        }

        public Vector2 Size
        {
            get { return size; }
            set 
            {
                this.size = value;
                text = StringUtils.WrapText(text, font, this.size.X);
                Vector2 scrollBarSize = new Vector2(scrollBar.Size.X, value.Y);
                scrollBar.Size = scrollBarSize;
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                this.position = value;
                scrollBar.Position = value + new Vector2(this.size.X, 0);
            }
        }

        public bool TabStop
        {
            get { return tabStop; }
            set { this.tabStop = value; }
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

        public TextArea(SpriteFont font, ContentManager content)
        {
            this.font = font;
            this.text = "";

            Texture2D scrollerTexture = content.Load<Texture2D>(@"GUI\Misc\ScrollBar\scroller");
            Texture2D scrollTrackTexture = content.Load<Texture2D>(@"GUI\Misc\ScrollBar\scrollerTrack");

            this.scrollBar = new ScrollBar(scrollerTexture, scrollTrackTexture);
            this.useScrollBar = false;
            this.scrollBar.ValueChanged += OnScrollerValueChanged;
        }

        public void Update(GameTime gameTime)
        {
            if (useScrollBar)
            {
                scrollBar.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawClipping(spriteBatch);
            if (useScrollBar)
            {
                scrollBar.Draw(spriteBatch);
            }
        }

        private void DrawClipping(SpriteBatch spriteBatch)
        {
            Rectangle clipRectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)size.X,
                (int)size.Y);

            DrawingUtils.SpriteBatchEnd(spriteBatch);
            DrawingUtils.SpriteBatchBeginClipped(spriteBatch, clipRectangle);
            
            Vector2 positionToDraw = new Vector2(position.X, position.Y - textOffset);

            spriteBatch.DrawString(font, text, positionToDraw, Color.White);

            DrawingUtils.SpriteBatchEndClipped(spriteBatch);
            DrawingUtils.SpriteBatchBegin(spriteBatch);
        }

        public void HandleInput()
        {
        }

        private void OnScrollerValueChanged(object sender, EventArgs args)
        {
            if (sender is ScrollBar)
            {
                ScrollBar scrollBar = (ScrollBar)sender;
                float value = scrollBar.Value;

                Vector2 textSize = font.MeasureString(text);

                textOffset = (textSize.Y - size.Y) * value;
            }
        }
    }
}
