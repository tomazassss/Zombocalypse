using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XRPGLibrary.Controls
{
    public interface IControl
    {
        bool TabStop
        {
            get;
            set;
        }

        bool HasFocus
        {
            get;
            set;
        }

        bool Enabled
        {
            get;
            set;
        }

        bool Visible
        {
            get;
            set;
        }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void HandleInput();
    }
}
