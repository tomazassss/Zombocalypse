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
using XRPGLibrary.Util;

namespace ZombocalypseRevised.GameScreens
{
    public class StartMenuScreen : BaseGameState
    {
        #region Field Region

        private PictureBox backgroundImage;

        private AnimatedButton startGameButton;
        private AnimatedButton loadGameButton;
        private AnimatedButton exitGameButton;

        private SimpleButton simpleNewGameButton;
        private SimpleButton simpleLoadGameButton;
        private SimpleButton simpleOptionsButton;
        private SimpleButton simpleQuitButton;

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

            Texture2D backgroundTexture = content.Load<Texture2D>(@"Backgrounds\tomo_titlescreen");

            backgroundImage = new PictureBox(
                backgroundTexture,
                gameRef.ScreenRectangle);
            controlManager.Add(backgroundImage);

            CreateButtons();
        }

        private void CreateButtons()
        {
            /*
            ContentManager content = Game.Content;

            Texture2D buttonTexture = content.Load<Texture2D>(@"GUI\Buttons\Start");
            Texture2D selectedButtonTexture = content.Load<Texture2D>(@"GUI\Buttons\SelectedButton");

            startGameButton = new AnimatedButton(buttonTexture, selectedButtonTexture);
            startGameButton.Selected += new EventHandler(MenuItemOnSelected);

            buttonTexture = content.Load<Texture2D>(@"GUI\Buttons\Load");

            loadGameButton = new AnimatedButton(buttonTexture, selectedButtonTexture);
            loadGameButton.Selected += new EventHandler(MenuItemOnSelected);

            buttonTexture = content.Load<Texture2D>(@"GUI\Buttons\Exit");

            exitGameButton = new AnimatedButton(buttonTexture, selectedButtonTexture);
            exitGameButton.Selected += new EventHandler(MenuItemOnSelected);

            controlManager.Add(startGameButton);
            controlManager.Add(loadGameButton);
            controlManager.Add(exitGameButton);
            */
            simpleNewGameButton = ComponentFactory.CreateSimpleButton("New Game");
            simpleNewGameButton.Selected += MenuItemOnSelected;
            controlManager.Add(simpleNewGameButton);

            simpleLoadGameButton = ComponentFactory.CreateSimpleButton("Load Game");
            //TODO: priskirti eventa kai pasirenki Load Game
            //controlManager.Add(simpleLoadGameButton);

            simpleOptionsButton = ComponentFactory.CreateSimpleButton("Options");
            //TODO: priskirti eventa kai pasirenki Options
            //controlManager.Add(simpleOptionsButton);

            simpleQuitButton = ComponentFactory.CreateSimpleButton("Quit Game");
            simpleQuitButton.Selected += MenuItemOnSelected;
            controlManager.Add(simpleQuitButton);

            controlManager.NextControl();
            controlManager.FocusChanged += new EventHandler(ControlManagerOnFocusChanged);

            //TODO: hardcoded position
            Vector2 position = new Vector2(350, 500);

            foreach (Control control in controlManager)
            {
                if (control is SimpleButton)
                {
                    SimpleButton button = control as SimpleButton;
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
            DrawingUtils.SpriteBatchBegin(gameRef.SpriteBatch);

            base.Draw(gameTime);

            controlManager.Draw(gameRef.SpriteBatch);

            DrawingUtils.SpriteBatchEnd(gameRef.SpriteBatch);
        }

        #endregion

        #region Game State Method Region

        [Obsolete("Animated buttons are no longer used")]
        private void ControlManagerOnFocusChanged(object sender, EventArgs e)
        {
            if (sender is AnimatedButton)
            {
                AnimatedButton button = sender as AnimatedButton;
                button.Reset();
            }
        }

        private void MenuItemOnSelected(object sender, EventArgs e)
        {
            if (sender == simpleNewGameButton)
            {
                LoadGamePlayScreen();
            }

            if (sender == simpleQuitButton)
            {
                gameRef.Exit();
            }
        }

        private void LoadGamePlayScreen()
        {
            //TODO: isimti, kai inventory bus iskeltas is GamePlayScreen
            InventoryManager inventoryManager = new InventoryManager();
            inventoryManager.Visible = true;
            inventoryManager.Enabled = true;
            gameRef.GamePlayScreen = new GamePlayScreen(gameRef, gameRef.StateManager, inventoryManager);
            gameRef.EquipmentScreen = new EquipmentScreen(gameRef, gameRef.StateManager, inventoryManager);
            gameRef.InventoryScreen = new InventoryScreen(gameRef, gameRef.StateManager, inventoryManager);
            stateManager.PushState(gameRef.GamePlayScreen);
        }

        #endregion
    }
}
