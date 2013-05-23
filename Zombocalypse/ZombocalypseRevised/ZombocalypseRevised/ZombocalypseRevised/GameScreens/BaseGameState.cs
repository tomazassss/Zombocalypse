using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary;
using Microsoft.Xna.Framework;
using XRPGLibrary.Controls;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombocalypseRevised.GameScreens
{
    public abstract partial class BaseGameState : GameState
    {
        #region Field Region

        protected Game1 gameRef;

        protected ControlManager controlManager;

        #endregion

        #region Constructor Region

        public BaseGameState(Game game, GameStateManager manager)
            : base(game, manager)
        {
            gameRef = (Game1)game;
        }

        #endregion

        #region XNA Method Region

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            SpriteFont menuFont = content.Load<SpriteFont>(@"Fonts\ControlFont");
            controlManager = new ControlManager(menuFont); 

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion
    }
}
