using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using XRPGLibrary.Controls;
using XRPGLibrary.Input;
using Microsoft.Xna.Framework.Input;

namespace XRPGLibrary.HUD
{
    public abstract class AHUDComponent
    {
        protected Vector2 size;
        protected Vector2 position;
        protected Texture2D backgroundTexture;
        protected bool visible;
        protected bool enabled;

        protected ContentManager content;
        protected ControlManager controlManager;
        protected CommandManager commandManager;

        public bool Visible
        {
            get { return visible; }
            set { this.visible = value; }
        }

        public virtual bool Enabled
        {
            get { return enabled; }
            set { this.enabled = value; }
        }

        public abstract Vector2 Size
        {
            get;
            set;
        }

        public abstract Vector2 Position
        {
            get;
            set;
        }

        public Texture2D BackgroundTexture
        {
            get { return backgroundTexture; }
            set { this.backgroundTexture = value; }
        }

        public AHUDComponent(SpriteFont font, ContentManager content)
        {
            this.commandManager = new CommandManager();
            this.controlManager = new ControlManager(font);
            this.content = content;
            this.visible = true;
            this.enabled = true;
            LoadContent();
        }

        public void RegisterCommand(Keys key, Action command, ClickState clickState)
        {
            this.commandManager.RegisterCommand(key, command, clickState);
        }

        public abstract void LoadContent();
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
    }
}
