using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XRPGLibrary.Util;
using XRPGLibrary.Input;
using Microsoft.Xna.Framework.Input;

//TODO: padaryti komponentų išdėstymą, title bar, button menu
namespace XRPGLibrary.HUD
{
    public class SimpleHUDWindow : AHUDWindow
    {
        private AHUDComponent component;

        protected Vector2 offset;

        private SpriteFont font;
        private String title;
        private bool showTitle;

        public String Title
        {
            get { return title; }
            set
            {
                this.title = value;
                if (this.title != null || this.title != "")
                {
                    this.showTitle = true;
                }
            }
        }

        public Vector2 Offset
        {
            get { return offset; }
            set { this.offset = value; }
        }

        public AHUDComponent Component
        {
            get { return component; }
            set
            {
                this.component = value;
                if (value != null)
                {
                    this.component.Size = Size - Offset * 2;
                    this.component.Position = Position + Offset;
                }
            }
        }

        public SimpleHUDWindow(SpriteFont font)
        {
            this.font = font;
            this.enabled = false;
            this.visible = false;
            this.showTitle = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                if (showTitle)
                {
                    Vector2 borderPosition = position;
                    Vector2 borderSize = size;
                    Vector2 titleSize = font.MeasureString(title);

                    borderPosition.Y -= titleSize.Y;
                    borderSize.Y += titleSize.Y;

                    Vector2 titlePosition = new Vector2(
                        borderPosition.X + (borderSize.X - titleSize.X) / 2,
                        borderPosition.Y);

                    DrawingUtils.DrawBorder(borderTexture, spriteBatch, borderPosition, borderSize);
                    spriteBatch.DrawString(font, title, titlePosition, Color.White);
                }
                else
                {
                    DrawingUtils.DrawBorder(borderTexture, spriteBatch, position, size);
                }

                if (component.Visible)
                {
                    component.Draw(spriteBatch);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (enabled)
            {
                if (component.Enabled)
                {
                    component.Update(gameTime);
                }
            }
        }

        public override void Hide()
        {
            visible = false;
            enabled = false;
            component.Visible = false;
            component.Enabled = false;
            InputHandler.Flush();
        }

        public override void Show()
        {
            visible = true;
            enabled = true;
            component.Visible = true;
            component.Enabled = true;
        }

        public override bool IsMouseInside()
        {
            MouseState mouse = InputHandler.MouseState;
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);
            Rectangle windowRectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)size.X,
                (int)size.Y);

            return mouseRectangle.Intersects(windowRectangle);
        }
    }
}
