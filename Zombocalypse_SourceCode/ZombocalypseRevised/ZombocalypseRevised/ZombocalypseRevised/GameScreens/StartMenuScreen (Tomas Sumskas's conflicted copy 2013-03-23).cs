using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XRPGLibrary;
using Microsoft.Xna.Framework.Input;
using XRPGLibrary.Controls;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombocalypseRevised.GameScreens
{
    public class StartMenuScreen : BaseGameState
    {
        #region Field Region

        private PictureBox backgroundImage;
        private PictureBox arrowImage;
        private LinkLabel startGame;
        private LinkLabel loadGame;
        private LinkLabel exitGame;

        private Button startGameButton;
        private Button loadGameButton;
        private Button exitGameButton;

        private float maxItemWidth = 0f;

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        public StartMenuScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ContentManager content = Game.Content;

            Texture2D backgroundTexture = content.Load<Texture2D>(@"Backgrounds\titlescreen");

            backgroundImage = new PictureBox(
                backgroundTexture,
                gameRef.ScreenRectangle);
            controlManager.Add(backgroundImage);

            Texture2D arrowTexture = content.Load<Texture2D>(@"GUI\leftarrowUp");

            arrowImage = new PictureBox(
                arrowTexture,
                new Rectangle(
                    0,
                    0,
                    arrowTexture.Width,
                    arrowTexture.Height));
            //controlManager.Add(arrowImage);

            CreateButtons();

            /*
            startGame = new LinkLabel();
            startGame.Text = "The story begins";
            startGame.Size = startGame.SpriteFont.MeasureString(startGame.Text);
            startGame.Selected += new EventHandler(MenuItemOnSelected);

            controlManager.Add(startGame);

            loadGame = new LinkLabel();
            loadGame.Text = "The story continues";
            loadGame.Size = loadGame.SpriteFont.MeasureString(loadGame.Text);
            loadGame.Selected += new EventHandler(MenuItemOnSelected);

            controlManager.Add(loadGame);

            exitGame = new LinkLabel();
            exitGame.Text = "The story ends";
            exitGame.Size = exitGame.SpriteFont.MeasureString(exitGame.Text);
            exitGame.Selected += new EventHandler(MenuItemOnSelected);

            controlManager.Add(exitGame);

            controlManager.NextControl();

            controlManager.FocusChanged += new EventHandler(ControlManagerOnFocusChanged);
            */

            /*
            Vector2 position = new Vector2(350, 500);

            foreach (Control control in controlManager)
            {
                if (control is LinkLabel)
                {
                    if (control.Size.X > maxItemWidth)
                    {
                        maxItemWidth = control.Size.X;
                    }

                    control.Position = position;
                    position.Y += control.Size.Y + 5f;
                }
            }
            */

            ControlManagerOnFocusChanged(startGame, null);
        }

        private void CreateButtons()
        {
            ContentManager content = Game.Content;

            Texture2D buttonTexture = content.Load<Texture2D>(@"GUI\Buttons\Start");
            Texture2D selectedButtonTexture = content.Load<Texture2D>(@"GUI\Buttons\SelectedButton");

            startGameButton = new Button(buttonTexture, selectedButtonTexture);
            startGameButton.Selected += new EventHandler(MenuItemOnSelected);

            buttonTexture = content.Load<Texture2D>(@"GUI\Buttons\Load");

            loadGameButton = new Button(buttonTexture, selectedButtonTexture);
            loadGameButton.Selected += new EventHandler(MenuItemOnSelected);

            buttonTexture = content.Load<Texture2D>(@"GUI\Buttons\Exit");

            exitGameButton = new Button(buttonTexture, selectedButtonTexture);
            exitGameButton.Selected += new EventHandler(MenuItemOnSelected);

            controlManager.Add(startGameButton);
            controlManager.Add(loadGameButton);
            controlManager.Add(exitGameButton);
            controlManager.NextControl();
            controlManager.FocusChanged += new EventHandler(ControlManagerOnFocusChanged);

            Vector2 position = new Vector2(350, 500);

            foreach (Control control in controlManager)
            {
                if (control is Button)
                {
                    Button button = control as Button;
                    button.Position = position;
                    position.Y += button.Size.Y + 5f;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            controlManager.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            gameRef.SpriteBatch.Begin();

            base.Draw(gameTime);

            controlManager.Draw(gameRef.SpriteBatch);

            gameRef.SpriteBatch.End();
        }

        #endregion

        #region Game State Method Region

        private void ControlManagerOnFocusChanged(object sender, EventArgs e)
        {
            /*
            Control control = sender as Control;
            Vector2 position = new Vector2(control.Position.X + maxItemWidth + 10f,
                control.Position.Y);

            arrowImage.SetPosition(position);
            */

            if (sender is Button)
            {
                Button button = sender as Button;
                button.Reset();
            }
        }

        private void MenuItemOnSelected(object sender, EventArgs e)
        {
            if (sender == startGameButton)
            {
                stateManager.PushState(gameRef.GamePlayScreen);
            }

            if (sender == loadGameButton)
            {
                stateManager.PushState(gameRef.GamePlayScreen);
            }

            if (sender == exitGameButton)
            {
                gameRef.Exit();
            }
        }

        #endregion
    }
}
