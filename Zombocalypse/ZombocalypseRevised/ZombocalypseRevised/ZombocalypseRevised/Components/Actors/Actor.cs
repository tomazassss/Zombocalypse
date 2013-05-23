using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.SpriteClasses;

namespace ZombocalypseRevised.Components.Actors
{
    public abstract class Actor
    {
        #region Property Region

        public abstract Rectangle BoundingBox
        {
            get;
        }
        public abstract Color[] TextureData
        {
            get;
        }

        #endregion
        #region Abstract Method Region

        public abstract void Update(GameTime gameTime, Player player);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract AnimatedSprite BindAnimations(Texture2D spriteSheet, int frameCount);
        public abstract void SetCurrentAnimation(AnimatedSprite sprite, int degrees);

        #endregion
    }
}
