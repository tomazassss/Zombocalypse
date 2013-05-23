using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XRPGLibrary;
using XRPGLibrary.TileEngine;
using Microsoft.Xna.Framework.Graphics;
using ZombocalypseRevised.Components;
using ZombocalypseRevised.Enums;
using XRPGLibrary.TileEngine.TileMaps;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Content;
using XRPGLibrary.Maps;
using XRPGLibrary.SpriteClasses;

namespace ZombocalypseRevised.GameScreens
{
    public class GamePlayScreen : BaseGameState
    {
        #region Field Region

        private Engine engine;

        private ATileMap map;

        private Player player;

        private Map newMap;

        AnimatedSprite sprite;

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        public GamePlayScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
            player = new Player(game);
        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            gameRef.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadCharacter();
            base.LoadContent();
            OpenMap();
            //Load2DTileMap();
            LoadIsoTileMap();


        }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.KeyReleased(Keys.Escape))
            {
                stateManager.PopState();
            }
            
            player.Update(gameTime);
            sprite.Update(gameTime);

            UpdateMouse();

            Vector2 motion = new Vector2();
            if (InputHandler.KeyDown(Keys.W))
            {
                sprite.CurrentAnimation = AnimationKey.Up;
                motion.Y = -1;
            }
            else if (InputHandler.KeyDown(Keys.S))
            {
                sprite.CurrentAnimation = AnimationKey.Down;
                motion.Y = 1;
            }
            if (InputHandler.KeyDown(Keys.A))
            {
                sprite.CurrentAnimation = AnimationKey.Left;
                motion.X = -1;
            }
            else if (InputHandler.KeyDown(Keys.D))
            {
                sprite.CurrentAnimation = AnimationKey.Right;
                motion.X = 1;
            }

            if (InputHandler.KeyDown(Keys.NumPad7))
            {
                sprite.CurrentAnimation = AnimationKey.UpLeft;
                motion.X = -1;
                motion.Y = -1;
            }
            if (InputHandler.KeyDown(Keys.NumPad9))
            {
                sprite.CurrentAnimation = AnimationKey.UpRight;
                motion.X = 1;
                motion.Y = -1;
            }
            if (InputHandler.KeyDown(Keys.NumPad1))
            {
                sprite.CurrentAnimation = AnimationKey.DownLeft;
                motion.X = -1;
                motion.Y = 1;
            }
            if (InputHandler.KeyDown(Keys.NumPad3))
            {
                sprite.CurrentAnimation = AnimationKey.DownRight;
                motion.X = 1;
                motion.Y = 1;
            }

            if (motion != Vector2.Zero)
            {
                sprite.IsAnimating = true;
                motion.Normalize();
                sprite.Position += motion * sprite.Speed;
                sprite.LockToMap();
                if (player.Camera.CameraMode == CameraMode.Follow)
                    player.Camera.LockToSprite(sprite);
            }
            else
            {
                sprite.IsAnimating = false;
            }
            if (InputHandler.KeyReleased(Keys.C))
            {
                player.Camera.ToggleCameraMode();
                if (player.Camera.CameraMode == CameraMode.Follow)
                    player.Camera.LockToSprite(sprite);
            }


            base.Update(gameTime);

            if (InputHandler.KeyReleased(Keys.I))
            {
                if (ChildComponents.Contains(gameRef.InventoryScreen))
                {
                    ChildComponents.Remove(gameRef.InventoryScreen);
                    //gameRef.IsMouseVisible = false;
                }
                else
                {
                    ChildComponents.Add(gameRef.InventoryScreen);
                    //gameRef.IsMouseVisible = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            gameRef.SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                Matrix.Identity);

            map.Draw(gameRef.SpriteBatch, player.Camera);
            sprite.Draw(gameTime, gameRef.SpriteBatch, player.Camera);

            base.Draw(gameTime);

            gameRef.SpriteBatch.End();
        }

        #endregion

        #region Abstract Method Region

        #endregion

        #region Method Region

        private void Load2DTileMap()
        {
            engine = new Engine(32, 32);

            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\tileset1");
            Tileset tileset1 = new Tileset(tilesetTexture, 8, 8, 32, 32);

            tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\tileset2");
            Tileset tileset2 = new Tileset(tilesetTexture, 8, 8, 32, 32);

            List<Tileset> tilesets = new List<Tileset>();
            tilesets.Add(tileset1);
            tilesets.Add(tileset2);

            MapLayer layer = new MapLayer(40, 40);

            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    Tile tile = new Tile(0, (int)Tilesets.Base);

                    layer.SetTile(x, y, tile);
                }
            }

            MapLayer splatter = new MapLayer(40, 40);

            Random random = new Random();

            for (int i = 0; i < 80; i++)
            {
                int x = random.Next(0, splatter.Width);
                int y = random.Next(0, splatter.Height);
                int index = random.Next(2, 14);

                Tile tile = new Tile(index, 0);
                splatter.SetTile(x, y, tile);
            }

            splatter.SetTile(1, 0, 0, (int)Tilesets.Buildings);
            splatter.SetTile(2, 0, 2, (int)Tilesets.Buildings);
            splatter.SetTile(3, 0, 0, (int)Tilesets.Buildings);

            List<MapLayer> mapLayers = new List<MapLayer>();
            mapLayers.Add(layer);
            mapLayers.Add(splatter);

            map = new TileMap2D(tilesets, mapLayers);
        }

        private void LoadIsoTileMap()
        {
            engine = new Engine(64, 32);

            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\grassland_tiles");
            Tileset tileset1 = new Tileset(tilesetTexture, 16, 2, 64, 32);

            List<Tileset> tilesets = new List<Tileset>();
            tilesets.Add(tileset1);

            MapLayer mapLayer = new MapLayer((int)newMap.MapSize.X, (int)newMap.MapSize.Y);
            //Random random = new Random();

            //for (int y = 0; y < mapLayer.Height; y++)
            //{
            //    for (int x = 0; x < mapLayer.Width; x++)
            //    {
            //        //0-31
            //        Tile tile = new Tile(15, (int)Tilesets.Base);

            //        mapLayer.SetTile(x, y, tile);
            //    }
            //}

            fillMap(mapLayer);
            List<MapLayer> mapLayers = new List<MapLayer>();
            mapLayers.Add(mapLayer);

            map = new TileMapIso(tilesets, mapLayers);
        }

        private void OpenMap()
        {
            ContentManager content = gameRef.Content;
            newMap = content.Load<Map>(@"Maps\Map1");
        }

        private void fillMap(MapLayer mapLayer)
        {
            foreach (Layer layer in newMap.MapLayers)
            {
                foreach (MapCell mapCell in layer.MapCells)
                {
                    Tile tile = new Tile(mapCell.TileIndex, layer.LayerLevel);
                    mapLayer.SetTile((int)mapCell.Position.X, (int)mapCell.Position.Y, tile);
                }
            }
        }

        private void LoadCharacter()
        {
            Texture2D spriteSheet = Game.Content.Load<Texture2D>(@"PlayerSprites\T_Vlad_Sword_Walking_48x48");
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(8, 48, 48, 0, 0);
            animations.Add(AnimationKey.Right, animation);

            animation = new Animation(8, 48, 48, 0, 48);
            animations.Add(AnimationKey.Up, animation);

            animation = new Animation(8, 48, 48, 0, 96);
            animations.Add(AnimationKey.UpRight, animation);

            animation = new Animation(8, 48, 48, 0, 144);
            animations.Add(AnimationKey.UpLeft, animation);

            animation = new Animation(8, 48, 48, 0, 192);
            animations.Add(AnimationKey.Down, animation);

            animation = new Animation(8, 48, 48, 0, 240);
            animations.Add(AnimationKey.DownRight, animation);

            animation = new Animation(8, 48, 48, 0, 288);
            animations.Add(AnimationKey.DownLeft, animation);

            animation = new Animation(8, 48, 48, 0, 336);
            animations.Add(AnimationKey.Left, animation);

            sprite = new AnimatedSprite(spriteSheet, animations);
        }

        private void UpdateMouse()
        {

            int deg = InputHandler.GetDegrees(sprite.Position);
            if (deg > 337)
            {
                sprite.CurrentAnimation = AnimationKey.Right;
            }
            if (deg < 23)
            {
                sprite.CurrentAnimation = AnimationKey.Right;
            }
            if (deg > 157 && deg < 203)
            {
                sprite.CurrentAnimation = AnimationKey.Left;
            }
            if (deg > 247 && deg < 292)
            {
                sprite.CurrentAnimation = AnimationKey.Up;
            }
            if (deg > 67 && deg < 113)
            {
                sprite.CurrentAnimation = AnimationKey.Down;
            }
            if (deg > 22 && deg < 68)
            {
                sprite.CurrentAnimation = AnimationKey.DownRight;
            }
            if (deg > 202 && deg < 248)
            {
                sprite.CurrentAnimation = AnimationKey.UpLeft;
            }
            if (deg > 112 && deg < 158)
            {
                sprite.CurrentAnimation = AnimationKey.DownLeft;
            }
            if (deg > 292 && deg < 338)
            {
                sprite.CurrentAnimation = AnimationKey.UpRight;
            }
            //Console.WriteLine("{0}", deg);
        }
        #endregion
    }


}
