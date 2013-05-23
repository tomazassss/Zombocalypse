using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XRPGLibrary.HUD
{
    public interface IHUDWindow
    {
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
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
        void Hide();
        void Show();
    }
}
