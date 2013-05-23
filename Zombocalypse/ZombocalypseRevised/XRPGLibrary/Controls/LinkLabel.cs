using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XRPGLibrary.Controls
{
    public class LinkLabel : Control
    {
        #region Field Region

        private Color selectedColor = Color.Red;

        #endregion

        #region Property Region

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        #endregion

        #region Constructor Region

        public LinkLabel()
        {
            tabStop = true;
            enabled = true;
            position = Vector2.Zero;
        }

        #endregion

        #region Abstract Method Region

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (hasFocus)
            {
                spriteBatch.DrawString(spriteFont, text, position, selectedColor);
            }
            else
            {
                spriteBatch.DrawString(spriteFont, text, position, color);
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

        #endregion
    }
}
