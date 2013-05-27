using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary;
using Microsoft.Xna.Framework.Content;
using XRPGLibrary.Util;
using XRPGLibrary.Controls;
using XRPGLibrary.Input;
using Microsoft.Xna.Framework.Input;

namespace ZombocalypseRevised.GameScreens
{
    public class GameOverScreen : BaseGameState
    {
        private Texture2D backgroundImage;
        private Rectangle rectangle;

        private CommandManager commandManager;

        public GameOverScreen(Game game, GameStateManager manager, Rectangle screenRectangle) : base(game, manager)
        {
            rectangle = screenRectangle;
            commandManager = new CommandManager();
            commandManager.RegisterCommand(Keys.Enter, ShowTitleScreen);
        }

        protected override void LoadContent()
        {
            ContentManager content = gameRef.Content;
            backgroundImage = content.Load<Texture2D>(@"Backgrounds\titlescreen");
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
