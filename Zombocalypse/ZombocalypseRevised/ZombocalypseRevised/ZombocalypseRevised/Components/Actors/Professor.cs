using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.TileEngine;

namespace ZombocalypseRevised.Components.Actors
{
    class Professor : NPC
    {
        #region Constructor Region

            public Professor(Texture2D readSprite, Texture2D talkSprite, Vector2 position, Game game, Camera cam) 
            : base(readSprite, talkSprite, position, game, cam)
            {

            }

        #endregion
        #region Method Region

        public override void Update(GameTime gameTime, Player player)
        {
            base.Update(gameTime, player);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }


        #endregion
    }
}
