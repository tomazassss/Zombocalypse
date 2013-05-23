using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XRPGLibrary.HUD
{
    public abstract class AHUDWindow : IHUDWindow
    {
        protected Vector2 size;
        protected Vector2 position;

        protected Texture2D borderTexture;

        protected bool visible;
        protected bool enabled;

        public Vector2 Size
        {
            get { return size; }
            set { this.size = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { this.position = value; }
        }

        public Texture2D BorderTexture
        {
            get { return borderTexture; }
            set { this.borderTexture = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { this.visible = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { this.enabled = value; }
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void Hide();
        public abstract void Show();
        public abstract bool IsMouseInside();
    }
}
