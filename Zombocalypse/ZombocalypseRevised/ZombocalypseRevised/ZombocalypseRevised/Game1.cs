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
using XRPGLibrary.Controls;
using XRPGLibrary.Util;
using ZombocalypseRevised.Util;
using XRpgLibrary.MissionSystem;

namespace ZombocalypseRevised
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region XNA Field Region

        private AudioEngine audioEngine;
        private SoundBank soundBank;
        private WaveBank waveBank;
        private Cue sound;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        #endregion

        #region Game State Region

        private GameStateManager stateManager;

        private TitleScreen titleScreen;
        private StartMenuScreen startMenuScreen;
        private GamePlayScreen gamePlayScreen;
        private EquipmentScreen equipmentScreen;
        private InventoryScreen inventoryScreen;
        private GameOverScreen gameOverScreen;

        #endregion

        #region Screen Field Region

        private const int SCREEN_WIDTH = 1024;
        private const int SCREEN_HEIGHT = 768;

        private readonly Rectangle screenRectangle;

        #endregion

        private SpriteFont _spr_font;
        private int _total_frames = 0;
        private float _elapsed_time = 0.0f;
        private int _fps = 0;

        private SpriteFont damageTakenFont;

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

        public EquipmentScreen EquipmentScreen
        {
            get { return equipmentScreen; }
            set { this.equipmentScreen = value; }
        }

        public InventoryScreen InventoryScreen
        {
            get { return inventoryScreen; }
            set { this.inventoryScreen = value; }
        }

        public GameOverScreen GameOverScreen
        {
            get { return gameOverScreen; }
            set { gameOverScreen = value; }
        }

        public GameStateManager StateManager
        {
            get { return this.stateManager; }
        }

        public SpriteFont DamageTakenFont
        {
            get { return damageTakenFont; }
            set { damageTakenFont = value; }
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
            gameOverScreen = new GameOverScreen(this, stateManager, screenRectangle);


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
            base.Initialize();
            this.Window.Title = "Zombocalypse";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spr_font = Content.Load<SpriteFont>(@"Fonts\ItemDataFont");

            damageTakenFont = Content.Load<SpriteFont>(@"Fonts\DamageTaken");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Screen rectangle is saved into a static variable for easier access
            GameSettings.ScreenRectangle = screenRectangle;

            //Item factory is initialized
            ItemFactory.Content = Content;
            ItemFactory.ItemTexturePath = @"GUI\Inventory\Items\";
            ItemFactory.ItemDataPath = @"ItemData\";
            ItemFactory.BorderResource = @"GUI\Inventory\Tooltip\border-v6";
            ItemFactory.BackgroundResource = @"GUI\Inventory\Tooltip\background-v2";
            ItemFactory.FontResource = @"Fonts\ItemDataFont";

            //Control factory is initialized
            ComponentFactory.Content = Content;
            ComponentFactory.ButtonFontResource = @"Fonts\ButtonFont";
            ComponentFactory.ButtonHoverResource = @"GUI\Buttons\button_hover";
            
            //Window factory is initialized
            WindowFactory.Content = Content;
            WindowFactory.WindowFontResource = @"Fonts\ControlFont";
            WindowFactory.BorderResource = @"GUI\Inventory\Tooltip\border-v6";

            audioEngine = new AudioEngine(@"Content\Audio\zombiai.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");



            soundBank.PlayCue("zombies");
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
            // Update
            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
 
            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Only update total frames when drawing
            _total_frames++;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.DrawString(_spr_font, string.Format("FPS={0}", _fps),
                new Vector2(10.0f, 20.0f), Color.Red);
            spriteBatch.End();


        }

        #endregion
    }
}
