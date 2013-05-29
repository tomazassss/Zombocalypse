using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XRPGLibrary.Input;
using XRPGLibrary;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using XRPGLibrary.Util;

namespace ZombocalypseRevised.GameScreens
{
    public class ToBeContinuedScreen : BaseGameState
    {
        private Texture2D backgroundImage;
        private Rectangle rectangle;

        private CommandManager commandManager;

        public ToBeContinuedScreen(Game game, GameStateManager manager, Rectangle screenRectangle)
            : base(game, manager)
        {
            rectangle = screenRectangle;
            commandManager = new CommandManager();
            commandManager.RegisterCommand(Keys.Enter, ShowTitleScreen);
        }

        protected override void LoadContent()
        {
            ContentManager content = gameRef.Content;
            backgroundImage = content.Load<Texture2D>(@"Backgrounds\EndGameScreen");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            DrawingUtils.SpriteBatchBegin(gameRef.SpriteBatch);
            gameRef.SpriteBatch.Draw(
                backgroundImage,
                rectangle,
                Color.White);
            DrawingUtils.SpriteBatchEnd(gameRef.SpriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            commandManager.Update(gameTime);
            base.Update(gameTime);
        }

        private void ShowTitleScreen()
        {
            stateManager.ChangeState(gameRef.StartMenuScreen);
        }
    }
}
