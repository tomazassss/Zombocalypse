using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XRPGLibrary.Animations;

namespace XRPGLibrary.Controls
{
    public class Button : Control
    {
        private ButtonAnimation buttonSelected;
        private Texture2D buttonTexture;

        public new Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                position.Y = (int)position.Y;
                buttonSelected.Position = value;
            }
        }

        public new Vector2 Size
        {
            get { return size; }
            set
            {
                size = value;
                buttonSelected.Size = size;
            }
        }

        public Button(Texture2D buttonTexture, Texture2D buttonSelectedTexture)
        {
            tabStop = true;
            enabled = true;
            position = Vector2.Zero;

            this.buttonTexture = buttonTexture;
            this.size = new Vector2(buttonTexture.Width, buttonTexture.Height);
            buttonSelected = new ButtonAnimation(buttonSelectedTexture, this.size, this.position, 75);

        }

        public override void Update(GameTime gameTime)
        {
            if (hasFocus)
            {
                buttonSelected.Update(gameTime);
            }
        }

        //TODO: pritaikyti kintanciam teksturos dydziui
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            if (hasFocus)
            {
                spriteBatch.Draw(
                    buttonTexture,
                    destinationRectangle,
                    Color.White);

                buttonSelected.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.Draw(
                    buttonTexture,
                    destinationRectangle,
                    Color.White);
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

        public void Reset()
        {
            buttonSelected.Reset();
        }
    }
}
