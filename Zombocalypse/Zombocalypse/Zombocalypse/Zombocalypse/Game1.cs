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
using Zombocalypse;

namespace Zombocalypse
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SpriteFont pericles6;
        //Texture2D hilight;

        SpriteAnimation vlad;

        private List<Ai> kvailys;


        TileMap myMap;
        int squaresAcross = 17;
        int squaresDown = 37;

        int baseOffsetX = -32;
        int baseOffsetY = -64;
        float heightRowDepthMod = 0.0000001f;

        #region Game states

        private enum GameState
        {
            Play,
            Options,
            MainMenu
        }

        GameState gameState;

        #endregion

        #region Constants

        private static readonly int BUTTON_HEIGHT_RATIO = 18;
        private static readonly int BUTTON_WIDTH_RATIO = 6;

        #endregion

        #region Graphics stuff

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #endregion

        #region Background

        Rectangle backgroundRectangle;
        Texture2D mainMenuBackgroundTexture;
        Texture2D optionsBackgroundTexture;

        #endregion

        #region Menu controls

        Button playButton;
        Button optionsButton;
        Button quitButton;
        Button applyButton;
        Button backButton;
        Slider resolutionSlider;

        bool prevPressed;
        bool pressed;

        Vector2 startingMainMenuButtonsPosition;
        Vector2 optionsButtonsPosition;

        #endregion

        #region Other attributes

        Color color;

        int currentScreenWidth;
        int currentScreenHeight;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            gameState = GameState.MainMenu;
            graphics.IsFullScreen = true;
            color = Color.White;
            IsMouseVisible = true;
            prevPressed = false;

            currentScreenWidth = graphics.GraphicsDevice.DisplayMode.Width;
            currentScreenHeight = graphics.GraphicsDevice.DisplayMode.Height;
            
            graphics.PreferredBackBufferWidth = currentScreenWidth;
            graphics.PreferredBackBufferHeight = currentScreenHeight;
            graphics.ApplyChanges();

            CreateButtons();
            PositionMainMenuButtons();
            PositionOptionsButtons();

            backgroundRectangle = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            mainMenuBackgroundTexture = Content.Load<Texture2D>(@"Interface\Backgrounds\MainMenu");
            optionsBackgroundTexture = Content.Load<Texture2D>(@"Interface\Backgrounds\Options");

            base.Initialize();
        }

        private void CreateButtons()
        {
            playButton = new Button(Content, "Play");
            optionsButton = new Button(Content, "Options");
            quitButton = new Button(Content, "Quit");
            applyButton = new Button(Content, "Apply");
            backButton = new Button(Content, "Back");
            resolutionSlider = new Slider(Content);
        }

        private void PositionMainMenuButtons()
        {
            playButton.SetSize(graphics.GraphicsDevice, BUTTON_WIDTH_RATIO, BUTTON_HEIGHT_RATIO);

            startingMainMenuButtonsPosition = new Vector2((graphics.PreferredBackBufferWidth - playButton.Size.X) / 2,
                                                 (graphics.PreferredBackBufferHeight - playButton.Size.Y) / 3);

            playButton.Position = startingMainMenuButtonsPosition;

            optionsButton.SetSize(graphics.GraphicsDevice, BUTTON_WIDTH_RATIO, BUTTON_HEIGHT_RATIO);
            optionsButton.Position = startingMainMenuButtonsPosition + new Vector2(0, playButton.Size.Y + playButton.Size.Y / 5);

            quitButton.SetSize(graphics.GraphicsDevice, BUTTON_WIDTH_RATIO, BUTTON_HEIGHT_RATIO);
            quitButton.Position = startingMainMenuButtonsPosition +
                                  new Vector2(0,
                                              playButton.Size.Y + playButton.Size.Y / 5 +
                                              optionsButton.Size.Y + optionsButton.Size.Y / 5);
        }

        private void PositionOptionsButtons()
        {
            applyButton.SetSize(graphics.GraphicsDevice, BUTTON_WIDTH_RATIO, BUTTON_HEIGHT_RATIO);

            optionsButtonsPosition = new Vector2(graphics.PreferredBackBufferWidth / 2,
                                                        (graphics.PreferredBackBufferHeight - applyButton.Size.Y) * 2 / 3);

            applyButton.Position = optionsButtonsPosition - new Vector2(applyButton.Size.X, 0);

            backButton.SetSize(graphics.GraphicsDevice, BUTTON_WIDTH_RATIO, BUTTON_HEIGHT_RATIO);
            backButton.Position = optionsButtonsPosition;

            resolutionSlider.SetSize(graphics.GraphicsDevice);
            resolutionSlider.Position = new Vector2(400, 400);

            HashSet<Point> resolutionsSet = new HashSet<Point>();
            
            foreach (DisplayMode displayMode in graphics.GraphicsDevice.Adapter.SupportedDisplayModes)
            {
                resolutionsSet.Add(new Point(displayMode.Width, displayMode.Height));
            }

            resolutionSlider.Resolutions = resolutionsSet.ToList<Point>();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Tile.TileSetTexture = Content.Load<Texture2D>(@"Textures\TileSets\part4_tileset");
            pericles6 = Content.Load<SpriteFont>(@"Fonts\Pericles6");
            myMap = new TileMap(Content.Load<Texture2D>(@"Textures\TileSets\mousemap"));
            //hilight = Content.Load<Texture2D>(@"Textures\TileSets\hilight");

            Camera.ViewWidth = this.graphics.PreferredBackBufferWidth;
            Camera.ViewHeight = this.graphics.PreferredBackBufferHeight;
            Camera.WorldWidth = ((myMap.MapWidth - 2) * Tile.TileStepX);
            Camera.WorldHeight = ((myMap.MapHeight - 2) * Tile.TileStepY);
            Camera.DisplayOffset = new Vector2(baseOffsetX, baseOffsetY);

            vlad = new SpriteAnimation(Content.Load<Texture2D>(@"Textures\Characters\T_Vlad_Sword_Walking_48x48"));

            vlad.AddAnimation("WalkEast", 0, 48 * 0, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkNorth", 0, 48 * 1, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkNorthEast", 0, 48 * 2, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkNorthWest", 0, 48 * 3, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkSouth", 0, 48 * 4, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkSouthEast", 0, 48 * 5, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkSouthWest", 0, 48 * 6, 48, 48, 8, 0.1f);
            vlad.AddAnimation("WalkWest", 0, 48 * 7, 48, 48, 8, 0.1f);

            vlad.AddAnimation("IdleEast", 0, 48 * 0, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleNorth", 0, 48 * 1, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleNorthEast", 0, 48 * 2, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleNorthWest", 0, 48 * 3, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleSouth", 0, 48 * 4, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleSouthEast", 0, 48 * 5, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleSouthWest", 0, 48 * 6, 48, 48, 1, 0.2f);
            vlad.AddAnimation("IdleWest", 0, 48 * 7, 48, 48, 1, 0.2f);

            kvailys = new List<Ai>();
            kvailys.Add(new Ai(new SpriteAnimation(Content.Load<Texture2D>(@"Textures\Characters\T_Vlad_Sword_Walking_48x48"))));

            vlad.Position = new Vector2(100, 100);
            vlad.DrawOffset = new Vector2(-24, -38);
            vlad.CurrentAnimation = "WalkEast";
            vlad.IsAnimating = true;           
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;
            prevPressed = pressed;
            pressed = mouseState.LeftButton == ButtonState.Pressed;

            switch (gameState)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime, mouseX, mouseY);                    
                    break;
                case GameState.Options:
                    UpdateOptions(gameTime, mouseX, mouseY);
                    break;

                #region Play Update

                case GameState.Play:
                    // Allows the game to exit
                    //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    //    this.Exit();
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape) == true)
                    {
                        gameState = GameState.MainMenu;
                    }

                    Vector2 moveVector = Vector2.Zero;
                    Vector2 moveDir = Vector2.Zero;
                    string animation = "";

                    KeyboardState ks = Keyboard.GetState();

                    if (ks.IsKeyDown(Keys.NumPad7))
                    {
                        moveDir = new Vector2(-1, -1);
                        animation = "WalkNorthWest";
                        moveVector += new Vector2(-1, -1);
                    }

                    if (ks.IsKeyDown(Keys.NumPad8))
                    {
                        moveDir = new Vector2(0, -1);
                        animation = "WalkNorth";
                        moveVector += new Vector2(0, -1);
                    }

                    if (ks.IsKeyDown(Keys.NumPad9))
                    {
                        moveDir = new Vector2(1, -1);
                        animation = "WalkNorthEast";
                        moveVector += new Vector2(1, -1);
                    }

                    if (ks.IsKeyDown(Keys.NumPad4))
                    {
                        moveDir = new Vector2(-1, 0);
                        animation = "WalkWest";
                        moveVector += new Vector2(-1, 0);
                    }

                    if (ks.IsKeyDown(Keys.NumPad6))
                    {
                        moveDir = new Vector2(1, 0);
                        animation = "WalkEast";
                        moveVector += new Vector2(1, 0);
                    }

                    if (ks.IsKeyDown(Keys.NumPad1))
                    {
                        moveDir = new Vector2(-1, 1);
                        animation = "WalkSouthWest";
                        moveVector += new Vector2(-1, 1);
                    }

                    if (ks.IsKeyDown(Keys.NumPad2))
                    {
                        moveDir = new Vector2(0, 1);
                        animation = "WalkSouth";
                        moveVector += new Vector2(0, 1);
                    }

                    if (ks.IsKeyDown(Keys.NumPad3))
                    {
                        moveDir = new Vector2(1, 1);
                        animation = "WalkSouthEast";
                        moveVector += new Vector2(1, 1);
                    }

                    if (moveDir.Length() != 0)
                    {
                        vlad.MoveBy((int)moveDir.X, (int)moveDir.Y);
                        if (vlad.CurrentAnimation != animation)
                            vlad.CurrentAnimation = animation;
                    }
                    else
                    {
                        vlad.CurrentAnimation = "Idle" + vlad.CurrentAnimation.Substring(4);
                    }

                    float vladX = MathHelper.Clamp(
                        vlad.Position.X, 0 + vlad.DrawOffset.X, Camera.WorldWidth);
                    float vladY = MathHelper.Clamp(
                        vlad.Position.Y, 0 + vlad.DrawOffset.Y, Camera.WorldHeight);

                    vlad.Position = new Vector2(vladX, vladY);
                    Vector2 testPosition = Camera.WorldToScreen(vlad.Position);

                    if (testPosition.X < 100)
                    {
                        Camera.Move(new Vector2(testPosition.X - 100, 0));
                    }

                    if (testPosition.X > (Camera.ViewWidth - 100))
                    {
                        Camera.Move(new Vector2(testPosition.X - (Camera.ViewWidth - 100), 0));
                    }

                    if (testPosition.Y < 100)
                    {
                        Camera.Move(new Vector2(0, testPosition.Y - 100));
                    }

                    if (testPosition.Y > (Camera.ViewHeight - 100))
                    {
                        Camera.Move(new Vector2(0, testPosition.Y - (Camera.ViewHeight - 100)));
                    }
                    vlad.Update(gameTime);

                    foreach (Ai ai in kvailys)
                    {
                        ai.MoveAi(vlad.Position);
                    }

                    break;

                #endregion
            }

            base.Update(gameTime);
        }

        #region Update methods

        private void UpdateMainMenu(GameTime gameTime, int mouseX, int mouseY)
        {
            playButton.Update(gameTime, mouseX, mouseY, pressed, prevPressed);
            optionsButton.Update(gameTime, mouseX, mouseY, pressed, prevPressed);
            quitButton.Update(gameTime, mouseX, mouseY, pressed, prevPressed);

            if (playButton.IsJustReleased)
            {
                gameState = GameState.Play;
            }

            if (optionsButton.IsJustReleased)
            {
                gameState = GameState.Options;
            }

            if (quitButton.IsJustReleased)
            {
                Exit();
            }
        }

        private void UpdateOptions(GameTime gameTime, int mouseX, int mouseY)
        {
            applyButton.Update(gameTime, mouseX, mouseY, pressed, prevPressed);
            backButton.Update(gameTime, mouseX, mouseY, pressed, prevPressed);
            resolutionSlider.Update(gameTime, mouseX, mouseY, pressed, prevPressed);

            if (backButton.IsJustReleased)
            {
                gameState = GameState.MainMenu;
            }

            if (applyButton.IsJustReleased)
            {
                Point resolution = resolutionSlider.Selected;
                graphics.PreferredBackBufferWidth = resolution.X;
                graphics.PreferredBackBufferHeight = resolution.Y;
                graphics.ApplyChanges();
                PositionOptionsButtons();
                PositionMainMenuButtons();
            }
        }

        #endregion

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            switch (gameState)
            {
                case GameState.MainMenu:
                    DrawMainMenuScreen(gameTime);
                    break;
                case GameState.Options:
                    DrawOptionsScreen(gameTime);
                    break;

                #region Draw Play

                case GameState.Play:

                    GraphicsDevice.Clear(Color.Black);

                    spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

                    Vector2 firstSquare = new Vector2(Camera.Location.X / Tile.TileStepX, Camera.Location.Y / Tile.TileStepY);
                    int firstX = (int)firstSquare.X;
                    int firstY = (int)firstSquare.Y;

                    Vector2 squareOffset = new Vector2(Camera.Location.X % Tile.TileStepX, Camera.Location.Y % Tile.TileStepY);
                    int offsetX = (int)squareOffset.X;
                    int offsetY = (int)squareOffset.Y;

                    float maxdepth = ((myMap.MapWidth + 1) + ((myMap.MapHeight + 1) * Tile.TileWidth)) * 10;
                    float depthOffset;

                    for (int y = 0; y < squaresDown; y++)
                    {
                        int rowOffset = 0;
                        if ((firstY + y) % 2 == 1)
                            rowOffset = Tile.OddRowXOffset;

                        for (int x = 0; x < squaresAcross; x++)
                        {
                            int mapx = (firstX + x);
                            int mapy = (firstY + y);
                            depthOffset = 0.7f - ((mapx + (mapy * Tile.TileWidth)) / maxdepth);

                            if ((mapx >= myMap.MapWidth) || (mapy >= myMap.MapHeight))
                                continue;
                            foreach (int tileID in myMap.Rows[mapy].Columns[mapx].BaseTiles)
                            {
                                spriteBatch.Draw(

                                    Tile.TileSetTexture,
                                    Camera.WorldToScreen(

                                        new Vector2((mapx * Tile.TileStepX) + rowOffset, mapy * Tile.TileStepY)),
                                    Tile.GetSourceRectangle(tileID),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    1.0f,
                                    SpriteEffects.None,
                                    1.0f);
                            }
                            int heightRow = 0;

                            foreach (int tileID in myMap.Rows[mapy].Columns[mapx].HeightTiles)
                            {
                                spriteBatch.Draw(
                                    Tile.TileSetTexture,
                                    Camera.WorldToScreen(
                                        new Vector2(
                                            (mapx * Tile.TileStepX) + rowOffset,
                                            mapy * Tile.TileStepY - (heightRow * Tile.HeightTileOffset))),
                                    Tile.GetSourceRectangle(tileID),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    1.0f,
                                    SpriteEffects.None,
                                    depthOffset - ((float)heightRow * heightRowDepthMod));
                                heightRow++;
                            }

                            foreach (int tileID in myMap.Rows[y + firstY].Columns[x + firstX].TopperTiles)
                            {
                                spriteBatch.Draw(
                                    Tile.TileSetTexture,
                                    Camera.WorldToScreen(

                                        new Vector2((mapx * Tile.TileStepX) + rowOffset, mapy * Tile.TileStepY)),
                                    Tile.GetSourceRectangle(tileID),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    1.0f,
                                    SpriteEffects.None,
                                    depthOffset - ((float)heightRow * heightRowDepthMod));
                            }

                            //Tile coordinates
                            //spriteBatch.DrawString(pericles6, (x + firstX).ToString() + ", " + (y + firstY).ToString(),
                            //new Vector2((x * Tile.TileStepX) - offsetX + rowOffset + baseOffsetX + 24,
                            //    (y * Tile.TileStepY) - offsetY + baseOffsetY + 48), Color.White, 0f, Vector2.Zero,
                            //    1.0f, SpriteEffects.None, 0.0f);
                        }
                    }

                    //Draws character - Vlad
                    Point vladStandingOn = myMap.WorldToMapCell(new Point((int)vlad.Position.X, (int)vlad.Position.Y));
                    int vladHeight = myMap.Rows[vladStandingOn.Y].Columns[vladStandingOn.X].HeightTiles.Count * Tile.HeightTileOffset;
                    vlad.Draw(spriteBatch, 0, -vladHeight);

                    //Draws highlighter
                    //Vector2 hilightLoc = Camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                    //Point hilightPoint = myMap.WorldToMapCell(new Point((int)hilightLoc.X, (int)hilightLoc.Y));

                    //int hilightrowOffset = 0;
                    //if ((hilightPoint.Y) % 2 == 1)
                    //    hilightrowOffset = Tile.OddRowXOffset;

                    //spriteBatch.Draw(
                    //                hilight,
                    //                Camera.WorldToScreen(

                    //                    new Vector2(

                    //                        (hilightPoint.X * Tile.TileStepX) + hilightrowOffset,

                    //                        (hilightPoint.Y + 2) * Tile.TileStepY)),
                    //                new Rectangle(0, 0, 64, 32),
                    //                Color.White * 0.3f,
                    //                0.0f,
                    //                Vector2.Zero,
                    //                1.0f,
                    //                SpriteEffects.None,
                    //                0.0f);

                    //Draw ai (laikinai)

                    foreach (Ai ai in kvailys)
                    {
                        ai.DrawAi(myMap, spriteBatch);
                    }

                    spriteBatch.End();

                    break;

                #endregion
            }

            base.Draw(gameTime);
        }

        #region Draw methods

        private void DrawMainMenuScreen(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(mainMenuBackgroundTexture, backgroundRectangle, color);

            playButton.Draw(spriteBatch);
            optionsButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void DrawOptionsScreen(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(optionsBackgroundTexture, backgroundRectangle, color);

            applyButton.Draw(spriteBatch);
            backButton.Draw(spriteBatch);
            resolutionSlider.Draw(spriteBatch);

            spriteBatch.End();
        }

        #endregion
    }
}
