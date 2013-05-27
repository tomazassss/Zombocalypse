using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XRPGLibrary.Controls
{
    public class Label : Control
    {
        public override string Text
        {
            set
            {
                this.text = value;
                this.size = spriteFont.MeasureString(text);
            }
        }

        #region Constructor Region

        public Label()
        {
            tabStop = false;
        }

        #endregion

        #region Abstract Method Region

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, text, position, color);
        }

        public override void HandleInput()
        {
        }

        #endregion
    }
}
