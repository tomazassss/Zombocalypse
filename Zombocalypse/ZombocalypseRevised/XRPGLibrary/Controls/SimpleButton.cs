using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XRPGLibrary.Controls
{
    public class SimpleButton : Control
    {
        private Texture2D hoverTexture;
        private Vector2 textPosition;
        private Color selectedColor;

        public SimpleButton(Texture2D hoverTexture, SpriteFont font)
        {
            tabStop = true;
            enabled = true;
            position = Vector2.Zero;

            this.hoverTexture = hoverTexture;
            size = new Vector2(hoverTexture.Width, hoverTexture.Height);
            this.spriteFont = font;

            //Hardcoded colors
            this.color = Color.White;
            this.selectedColor = Color.Black;
        }

        public override string Text
        {
            set
            {
                base.Text = value;
                PositionText();
            }
        }

        public override Vector2 Position
        {
            set
            {
                base.Position = value;
                PositionText();
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)size.X,
                (int)size.Y);

            if (hasFocus)
            {
                spriteBatch.Draw(hoverTexture, destinationRectangle, Color.White);
                spriteBatch.DrawString(spriteFont, text, textPosition, selectedColor);
            }
            else
            {
                spriteBatch.DrawString(spriteFont, text, textPosition, color);
            }

        }

        public override void HandleInput()
        {
            if (!hasFocus)
            {
                return;
            }

            if (InputHandler.KeyReleased(Keys.Enter))
            {
                base.OnSelected(null);
            }
        }

        private void PositionText()
        {
            Vector2 textSize = spriteFont.MeasureString(text);

            Vector2 textPosition = new Vector2(this.position.X + (size.X - textSize.X) / 2,
                                               this.position.Y + (size.Y - textSize.Y) / 2);

            this.textPosition = textPosition;
        }
    }
}
