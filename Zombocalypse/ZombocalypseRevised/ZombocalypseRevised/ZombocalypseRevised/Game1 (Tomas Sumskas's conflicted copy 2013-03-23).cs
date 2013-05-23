using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XRPGLibrary;
using ZombocalypseRevised.GameScreens;

namespace ZombocalypseRevised
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region XNA Field Region

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        #endregion

        #region Game State Region

        private GameStateManager stateManager;
        private TitleScreen titleScreen;
        private StartMenuScreen startMenuScreen;
        private GamePlayScreen gamePlayScreen;
        private InventoryScreen inventoryScreen;

        #endregion

        #region Screen Field Region

        private const int SCREEN_WIDTH = 1024;
        private const int SCREEN_HEIGHT = 768;

        private readonly Rectangle screenRectangle;

        #endregion

        #region Property Region

        public SpriteBatch SpriteBatch 
        {
            get { return spriteBatch; }
        }

        public Rectangle ScreenRectangle
        {
            get { return screenRectangle; }
        }

        public TitleScreen TitleScreen
        {
            get { return titleScreen; }
            set { this.titleScreen = value; }
        }

        public StartMenuScreen StartMenuScreen
        {
            get { return startMenuScreen; }
            set { this.startMenuScreen = value; }
        }

        public GamePlayScreen GamePlayScreen
        {
            get { return gamePlayScreen; }
            set { this.gamePlayScreen = value; }
        }

        public InventoryScreen InventoryScreen
        {
            get { return inventoryScreen; }
            set { this.inventoryScreen = value; }
        }

        #endregion

        #region Constructor Region

        public Game1()
        {
            
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;

            screenRectangle = new Rectangle(
                0,
                0,
                SCREEN_WIDTH,
                SCREEN_HEIGHT);

            Content.RootDirectory = "Content";
            Components.Add(new InputHandler(this));

            stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            titleScreen = new TitleScreen(this, stateManager);
            startMenuScreen = new StartMenuScreen(this, stateManager);
            gamePlayScreen = new GamePlayScreen(this, stateManager);
            inventoryScreen = new InventoryScreen(this, stateManager);

            stateManager.ChangeState(titleScreen);
        }

        #endregion

        #region XNA Method Region

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            base.Draw(gameTime);
        }

        #endregion
    }
}
