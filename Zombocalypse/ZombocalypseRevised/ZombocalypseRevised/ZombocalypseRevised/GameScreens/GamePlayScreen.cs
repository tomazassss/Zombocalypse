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
using Microsoft.Xna.Framework.Content;
using XRPGLibrary.SpriteClasses;
using XRPGLibrary.Controls;
using XRPGLibrary.Controls.Items;
using XRPGLibrary.WorldClasses;
using XRPGLibrary.Controls.Managers;
using XRPGLibrary.Input;
using ZombocalypseRevised.HUD;
using XRPGLibrary.HUD;
using XRPGLibrary.Util;
using ZombocalypseRevised.Components.Actors;
using XRPGLibrary.Collisions;
using XRpgLibrary.MissionSystem;
using XRpgLibrary.DialogueSystem;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using ZombocalypseRevised.DialogueSystem;

namespace ZombocalypseRevised.GameScreens
{
    public class GamePlayScreen : BaseGameState
    {
        #region Field Region

        private const int OFFSET = 100;

        //TODO: Remove once items are loaded from file
        private InventoryItem helmet;
        private InventoryItem armor;
        private InventoryItem leggings;
        private InventoryItem boots;
        private InventoryItem mainHand;
        private InventoryItem offHand;
        //TODO: Remove to here

        private Gun gun;

        private Engine engine;

        private ATileMap map;

        private Player player;

        private Zombie zombie;

        private Professor professor;

        private List<Enemy> enemies;

        private List<NPC> npcs;

        private List<IChattable> chattables;

        private MapData newMap;

        private ObstaclesMapGenerator oMap;

        private AnimatedSprite sprite;
        private AnimatedSprite shootingSprite;

        private SoundEffect deagleShot;
        private Song walkingSound;

        private bool isPaused;
        private bool drawEquipment;

        private ButtonState oldState;



        private CollisionsManager collisionsManager;
        //TODO: padaryti inventory manager draw/update priklausoma nuo EquipmentScreen/InventoryScreen visible/enabled
        private InventoryManager inventoryManager;
        private HUDManager hudManager;
        private CommandManager commandManager;
        private PopUpManager popUpManager;
        private MissionManager missionManager;
        private DialogueManager dialogueManager;

        private MissionFactory missions;

        private NotificationPopup notifications;
        private GameInterface gameInterface;

        #endregion

        public event EventHandler ItemPickedUp;
        public event EventHandler PlayerPositionChanged;

        #region Property Region

        public bool IsPaused
        {
            get { return isPaused; }
            set { this.isPaused = value; }
        }

        public CommandManager CommandManager
        {
            get { return commandManager; }
        }

        #endregion

        #region Constructor Region

        public GamePlayScreen(Game game, GameStateManager manager, InventoryManager inventoryManager)
            : base(game, manager)
        {
            this.inventoryManager = inventoryManager;
            this.hudManager = new HUDManager();
            this.commandManager = new CommandManager();
            this.popUpManager = PopUpManager.CreateInstance(game);
            this.collisionsManager = new CollisionsManager();
            this.missionManager = MissionManager.GetInstance();
            this.dialogueManager = DialogueManager.GetInstance();

            PlayerPositionChanged += this.missionManager.OnUpdateEvent;

            player = new Player(game);
            isPaused = false;
            drawEquipment = false;
        }

        #endregion

        #region Window Method Region

        private void TestDialogueSystem()
        {
            foreach (NPC npc in npcs)
            {
                if (npc.canInteract())
                {
                    ContentManager content = gameRef.Content;

                    Texture2D tooltipBackground = content.Load<Texture2D>(@"GUI\Inventory\Tooltip\background-v2");
                    Texture2D tooltipBorder = content.Load<Texture2D>(@"GUI\Inventory\Tooltip\border-v6");
                    SpriteFont font = content.Load<SpriteFont>(@"Fonts\ItemDataFont");

                    Rectangle screenRectangle = gameRef.ScreenRectangle;
                    SimpleHUDWindow dialogueWindow = new SimpleHUDWindow(font);
                    DialogueComponent dialogueComponent = new DialogueComponent(font, gameRef.Content, npc);
                    dialogueComponent.RegisterCommand(Keys.Escape, CloseDialogueWindow, ClickState.RELEASED);
                    dialogueComponent.RegisterCommand(Keys.E, CloseDialogueWindow, ClickState.RELEASED);

                    dialogueWindow.Size = new Vector2(screenRectangle.Width * (2f / 3f), (screenRectangle.Height / 2f) - 100);
                    dialogueWindow.Position = new Vector2(
                        (screenRectangle.Width - dialogueWindow.Size.X) / 2f,
                        screenRectangle.Height / 2f + (screenRectangle.Height / 2f - dialogueWindow.Size.Y) / 2f);
                    dialogueWindow.Title = professor.Name;
                    dialogueWindow.BorderTexture = tooltipBorder;

                    dialogueWindow.Component = dialogueComponent;
                    dialogueWindow.Show();
                    dialogueComponent.DialogueSelected += dialogueManager.OnSelected;

                    hudManager.Put(Window.DIALOGUE, dialogueWindow);
                    isPaused = true;
                    break;
                }
            }
        }

        //TODO: remove after done testing
        private void PickupItem()
        {
            int itemNumber = RandomUtils.RANDOM.Next(0, 2);
            InventoryItem item = null;

            switch (itemNumber)
            {
                case 0:
                    item = ItemFactory.CreateItem("desert_eagle");
                    break;
                case 1:
                    item = ItemFactory.CreateItem("desert_eagle_ammo");
                    break;
            }

            if (ItemPickedUp != null)
            {
                ItemPickedUp(item, null);
            }
        }

        private void CloseWindow(object sender, EventArgs args)
        {
            stateManager.PopState();
            missionManager.Clear();
        }

        private void OpenPlayerInfoWindow()
        {
            IHUDWindow playerInfoWindow = hudManager.GetWindow(Window.PLAYER_INFO);
            playerInfoWindow.Show();
            isPaused = true;
        }

        private void ClosePlayerInfoWindow()
        {
            IHUDWindow playerInfoWindow = hudManager.GetWindow(Window.PLAYER_INFO);
            playerInfoWindow.Hide();
            isPaused = false;
        }

        private void OpenPauseMenuWindow()
        {
            IHUDWindow pauseWindow = hudManager.GetWindow(Window.PAUSE);
            pauseWindow.Show();
            isPaused = true;
        }

        private void ClosePauseMenuWindow(object sender, EventArgs args)
        {
            IHUDWindow pauseWindow = hudManager.GetWindow(Window.PAUSE);
            pauseWindow.Hide();
            isPaused = false;
        }

        private void OpenMissionLogWindow()
        {
            IHUDWindow missionLogWindow = hudManager.GetWindow(Window.MISSION_LOG);
            missionLogWindow.Show();
            isPaused = true;
        }

        private void CloseMissionLogWindow()
        {
            IHUDWindow missionLogWindow = hudManager.GetWindow(Window.MISSION_LOG);
            missionLogWindow.Hide();
            isPaused = false;
        }

        private void ClosePauseMenuWindow()
        {
            ClosePauseMenuWindow(null, null);
        }

        private void OpenDialogueWindow()
        {
            TestDialogueSystem();
        }

        private void CloseDialogueWindow()
        {
            IHUDWindow dialogueWindow = hudManager.GetWindow(Window.DIALOGUE);
            dialogueWindow.Hide();
            isPaused = false;
        }

        private void ChangePlayerStats(object sender, EventArgs args)
        {
            if (!(sender is Stats))
            {
                return;
            }
            else
            {
                Stats stats = (Stats)sender;

                player.Armor = stats.Armor;
                gun.Damage = stats.Damage;
            }
        }

        /*
        private void OpenInventoryWindow()
        {
            IHUDWindow inventoryWindow = hudManager.GetWindow(Window.INVENTORY);
            inventoryWindow.Show();
            isPaused = true;
        }

        private void CloseInventoryWindow()
        {
            IHUDWindow inventoryWindow = hudManager.GetWindow(Window.INVENTORY);
            inventoryWindow.Hide();
            isPaused = false;
        }

        private void OpenEquipmentWindow()
        {
            IHUDWindow equipmentWindow = hudManager.GetWindow(Window.EQUIPMENT);
            equipmentWindow.Show();
            isPaused = true;
        }

        private void CloseEquipmentWindow()
        {
            IHUDWindow equipmentWindow = hudManager.GetWindow(Window.EQUIPMENT);
            equipmentWindow.Hide();
            isPaused = false;
        }

        private void OpenEquipmentScreen()
        {
            if (ChildComponents.Contains(gameRef.EquipmentScreen))
            {
                gameRef.EquipmentScreen.ItemManager.SetActive(false);
                gameRef.EquipmentScreen.SetEnabled(false);
                ChildComponents.Remove(gameRef.EquipmentScreen);
                if (!ChildComponents.Contains(gameRef.InventoryScreen))
                {
                    //isPaused = false;
                    drawEquipment = false;
                }

                //gameRef.IsMouseVisible = false;
            }
            else
            {
                gameRef.EquipmentScreen.ItemManager.SetActive(true);
                gameRef.EquipmentScreen.SetEnabled(true);
                ChildComponents.Add(gameRef.EquipmentScreen);
                if (!ChildComponents.Contains(gameRef.InventoryScreen))
                {
                    //isPaused = true;
                    drawEquipment = true;
                }
                //gameRef.IsMouseVisible = true;
            }
        }

        private void OpenInventoryScreen()
        {
            if (ChildComponents.Contains(gameRef.InventoryScreen))
            {
                gameRef.InventoryScreen.ItemManager.SetActive(false);
                gameRef.InventoryScreen.Inventory.SetEnabled(false);
                ChildComponents.Remove(gameRef.InventoryScreen);
                if (!ChildComponents.Contains(gameRef.EquipmentScreen))
                {
                   // isPaused = false;
                    drawEquipment = false;
                }
            }
            else
            {
                gameRef.InventoryScreen.ItemManager.SetActive(true);
                gameRef.InventoryScreen.Inventory.SetEnabled(true);
                ChildComponents.Add(gameRef.InventoryScreen);
                if (!ChildComponents.Contains(gameRef.EquipmentScreen))
                {
                    //isPaused = true;
                    drawEquipment = true;
                }
            }
        }
        */

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
            LoadNpcs();
            LoadSounds();
            base.LoadContent();
            OpenMap();
            //Load2DTileMap();
            LoadIsoTileMap();
            LoadGun();
            LoadWindows();
            LoadItems();
            LoadEnemies();
            oldState = new ButtonState();

            missions = gameRef.Content.Load<MissionFactory>(@"Missions\AllMissions");
            missionManager.AddMission(missions.GetMissionById(0));
            missionManager.AddMission(missions.GetMissionById(1));

            dialogueManager.DialogueSelected += missionManager.OnUpdateEvent;

            AllDialogues allDialogues = gameRef.Content.Load<AllDialogues>(@"Dialogues\AllDialogues");
            DialogueService.GetInstance().AllDialogues = allDialogues.Dialogues;
            DialogueService.GetInstance().RegisterDialogueProcessor(OnDialogueSelected);

            //commandManager.RegisterCommand(Keys.I, OpenEquipmentScreen);
            //commandManager.RegisterCommand(Keys.U, OpenInventoryScreen);
            commandManager.RegisterCommand(Keys.Escape, OpenPauseMenuWindow);
            commandManager.RegisterCommand(Keys.L, OpenMissionLogWindow);
            commandManager.RegisterCommand(Keys.E, OpenDialogueWindow);
            commandManager.RegisterCommand(Keys.C, OpenPlayerInfoWindow);

            //TODO: remove after done testing
            //commandManager.RegisterCommand(Keys.E, PickupItem);
        }

        public override void Update(GameTime gameTime)
        {
            if (drawEquipment)
            {
                inventoryManager.Update(gameTime);
            }
            
            if (!isPaused)
            {

                player.Update(gameTime, player);
                sprite.Update(gameTime);


                foreach (Enemy e in enemies)
                    e.Update(gameTime, player);

                foreach (NPC n in npcs)
                    n.Update(gameTime, player);

                int x= enemies.Count;
                for (int i = 0; i < x; i++)
                {
                    if (enemies[i].RemoveThisElement == true)
                    {
                        enemies.Remove(enemies[i]);
                        x--;
                    }
                }

                UpdateMouse();

                Vector2 motion = new Vector2();
                Vector2 direction = new Vector2();

                int rotationAngle = 0;
                //TODO: Iškelti į veikėjo klasę. Bindinti mygtukus su CommandManager
                if (InputHandler.KeyDown(Keys.W))
                {
                    motion.Y = -1;
                }
                else if (InputHandler.KeyDown(Keys.S))
                {
                    motion.Y = 1;
                }
                if (InputHandler.KeyDown(Keys.A))
                {
                    motion.X = -1;
                }
                else if (InputHandler.KeyDown(Keys.D))
                {
                    motion.X = 1;
                }
                if (InputHandler.LeftMouseButtonState == ButtonState.Pressed)
                {
                    oldState = ButtonState.Pressed;            
                }
                if (InputHandler.LeftMouseButtonState == ButtonState.Released && oldState == ButtonState.Pressed)
                {
                    direction = new Vector2(InputHandler.MouseState.X, InputHandler.MouseState.Y);
                    rotationAngle = InputHandler.GetDegrees(sprite.Origin, player.Camera.Position);
                    gun.Add(direction, sprite.Origin - player.Camera.Position, 50, rotationAngle);
                    oldState = ButtonState.Released;
                    player.IsShooting = true;
                    //Console.WriteLine(sprite.Position);
                    deagleShot.Play();
                }
                gun.Update(enemies);

                if (motion != Vector2.Zero)
                {
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(walkingSound);

                    }
                    if (!player.IsShooting)
                    {
                        sprite.IsAnimating = true;
                        shootingSprite.IsAnimating = false;
                    }
                    else
                    {
                        sprite.IsAnimating = false;
                        shootingSprite.IsAnimating = true;
                        shootingSprite.AnimateOnceAndResume = true;
                        shootingSprite.Update(gameTime);
                        if (!shootingSprite.AnimateOnceAndResume)
                        {
                            player.IsShooting = false;
                        }
                    }
                    motion.Normalize();

                    sprite.Position = collisionsManager.DetectCollisions(motion, sprite.Position);
                    sprite.LockToMap();
                    shootingSprite.Position = collisionsManager.DetectCollisions(motion, shootingSprite.Position);
                    shootingSprite.LockToMap();

                    if (player.Camera.CameraMode == CameraMode.Follow)
                        player.Camera.LockToSprite(sprite);

                    if (PlayerPositionChanged != null)
                    {
                        Point playerPos = Engine.VectorToCellIso(sprite.Position);
                        PlayerPositionChanged(playerPos, null);
                    }
                }
                else
                {
                    MediaPlayer.Stop();
                    //sprite.IsAnimating = false;
                    //shootingSprite.IsAnimating = false;
                    if (!player.IsShooting)
                    {
                        sprite.IsAnimating = false;
                        shootingSprite.IsAnimating = false;
                    }
                    else
                    {
                        sprite.IsAnimating = false;
                        shootingSprite.IsAnimating = true;
                        shootingSprite.AnimateOnceAndResume = true;
                        shootingSprite.Update(gameTime);
                        if (!shootingSprite.AnimateOnceAndResume)
                        {
                            player.IsShooting = false;
                        }
                    }
                }

                //if (InputHandler.KeyReleased(Keys.C))
                //{
                //    player.Camera.ToggleCameraMode();
                //    if (player.Camera.CameraMode == CameraMode.Follow)
                //        player.Camera.LockToSprite(sprite);
                //}

                missionManager.Update(gameTime);
            }

            dialogueManager.Update(gameTime);
            hudManager.Update(gameTime);
            if (!isPaused)
            {
                commandManager.Update(gameTime);
            }

            popUpManager.Update(gameTime);
            gameInterface.Update(gameTime);
            notifications.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawingUtils.SpriteBatchBegin(gameRef.SpriteBatch);


            map.Draw(gameRef.SpriteBatch, player.Camera);
            player.Draw(gameTime, gameRef.SpriteBatch);
            if (!player.IsShooting)
            {
                sprite.Draw(gameTime, gameRef.SpriteBatch, player.Camera);
            }
            else
            {
                shootingSprite.Draw(gameTime, gameRef.SpriteBatch, player.Camera);
            }
            gun.Draw(gameRef.SpriteBatch);

            foreach (Enemy e in enemies)
                e.Draw(gameTime, gameRef.SpriteBatch);

            foreach (NPC n in npcs)
                n.Draw(gameTime, gameRef.SpriteBatch);

            base.Draw(gameTime);
            
            //TODO: perdaryti EquipmentScreen ir InventoryScreen kaip HUDComponent. Isimti is Game1 klases
            gameRef.EquipmentScreen.DrawItems(gameTime);
            gameRef.InventoryScreen.DrawItems(gameTime);

            if (drawEquipment)
            {
                inventoryManager.Draw(gameRef.SpriteBatch);
            }

            hudManager.Draw(gameRef.SpriteBatch);

            popUpManager.Draw(gameTime);
            gameInterface.Draw(gameRef.SpriteBatch);
            notifications.Draw(gameRef.SpriteBatch);

            DrawingUtils.SpriteBatchEnd(gameRef.SpriteBatch);
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

            MapLayer layer = new MapLayer(40, 40, 0);

            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    Tile tile = new Tile(0, (int)Tilesets.Base);

                    layer.SetTile(x, y, tile);
                }
            }

            MapLayer splatter = new MapLayer(40, 40, 0);

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

            List<Tileset> tileSets = new List<Tileset>();
            List<MapLayer> mapLayers = new List<MapLayer>();

            engine = new Engine(64, 32);

            foreach (TilesetData data in newMap.Tilesets)
            { 
                Texture2D tilesetTexture = null;
                int pos = data.TilesetImageName.IndexOf("TileSets\\");
                //if (pos == -1)
                //{
                //    //tilesetTexture = Game.Content.Load<Texture2D>(@data.TilesetImageName);

                //}
                //else
                //{
                    int lenght = data.TilesetImageName.Length - 4 - pos;

                    tilesetTexture = Game.Content.Load<Texture2D>(@data.TilesetImageName.Substring(pos, lenght).Trim());
                //}




                //using (Stream stream = new FileStream(data.TilesetImageName, FileMode.Open, FileAccess.Read))
                //{
                //    texture = Texture2D.FromStream(GraphicsDevice, stream);
                    tileSets.Add(
                        new Tileset(
                            tilesetTexture,
                            data.TilesWide,
                            data.TilesHigh,
                            data.TileWidthInPixels,
                            data.TileHeightInPixels));
                //}
            }

            foreach (MapLayerData data in newMap.Layers)
            {
                
                mapLayers.Add(MapLayer.FromMapLayerData(data));
            }

            map = new TileMapIso(tileSets[0], (MapLayer)mapLayers[0]);

            for (int i = 1; i < tileSets.Count; i++)
                map.AddTileset(tileSets[i]);

            for (int i = 1; i < mapLayers.Count; i++)
                map.AddLayer(mapLayers[i]);
        }

        private void OpenMap()
        {
            ContentManager content = gameRef.Content;
            newMap = content.Load<MapData>(@"Maps\Map1");
            //newMap = content.Load<MapData>(@"Maps\Map_Test");
            
            collisionsManager.SetMap(newMap);
            oMap = new ObstaclesMapGenerator(newMap);
            
        }

        private void LoadCharacter()
        {
            //TODO: reikia kažkokiu būdu padaryt ne hardcodintą animacijų nuskaitymą. Simas.
            Texture2D spriteSheet = Game.Content.Load<Texture2D>(@"PlayerSprites\T_Vlad_Gun_Walking");
            player.SpriteSheet = spriteSheet;
            Texture2D shootingSpriteSheet = Game.Content.Load<Texture2D>(@"PlayerSprites\Vlad_Shooting");
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

            animations.Clear();

            animation = new Animation(1, 48, 48, 0, 0);
            animations.Add(AnimationKey.Right, animation);

            animation = new Animation(1, 48, 48, 0, 48);
            animations.Add(AnimationKey.Up, animation);

            animation = new Animation(1, 48, 48, 0, 96);
            animations.Add(AnimationKey.UpRight, animation);

            animation = new Animation(1, 48, 48, 0, 144);
            animations.Add(AnimationKey.UpLeft, animation);

            animation = new Animation(1, 48, 48, 0, 192);
            animations.Add(AnimationKey.Down, animation);

            animation = new Animation(1, 48, 48, 0, 240);
            animations.Add(AnimationKey.DownRight, animation);

            animation = new Animation(1, 48, 48, 0, 288);
            animations.Add(AnimationKey.DownLeft, animation);

            animation = new Animation(1, 48, 48, 0, 336);
            animations.Add(AnimationKey.Left, animation);

            shootingSprite = new AnimatedSprite(shootingSpriteSheet, animations);

            player.Sprite = sprite;
            player.PlayerDead += OnPlayerDeath;

            collisionsManager.SetSpriteSpeed(sprite.Speed);
        }

        private void LoadEnemies()
        {
            enemies = new List<Enemy>();

            AudioEngine enemyAudioEngine = new AudioEngine(@"Content\Audio\ZombieSounds\ZombieAttack.xgs");
            WaveBank enemyWaveBank = new WaveBank(enemyAudioEngine, @"Content\Audio\ZombieSounds\ZombieAttackWaveBank.xwb");

            //2 Priesai prie tako i miesteli
            Populate(new Vector2(376.718f, 266.718f), 2, 40, enemyAudioEngine, enemyWaveBank);
            //8 Priesai lauke siaure rytuose nuo miestelio
            Populate(new Vector2(2131.857f, 118.0393f), 8, 100, enemyAudioEngine, enemyWaveBank);
            //20 Priesu lauke rytuose nuo miestelio (Boso chebra) 
            Populate(new Vector2(2801.631f, 792.6567f), 20, 100, enemyAudioEngine, enemyWaveBank);

            //Sukuriame Mister Death
            Texture2D deathWalkingSpriteSheet = Game.Content.Load<Texture2D>(@"EnemySprites\MisterDeath\MisterDeath_Walking");
            SpriteSheet deathWalkingSS = new SpriteSheet(deathWalkingSpriteSheet, 8);
            Texture2D deathAttackingSpriteSheet = Game.Content.Load<Texture2D>(@"EnemySprites\MisterDeath\MisterDeath_Attacking");
            SpriteSheet deathAttackingSS = new SpriteSheet(deathAttackingSpriteSheet, 13);
            Death death = new Death(deathWalkingSS,
                                    deathWalkingSS,
                                    deathWalkingSS,
                                    deathAttackingSS,
                                    new Vector2(2890.631f, 792.6567f),
                                    gameRef,
                                    player.Camera,
                                    enemyAudioEngine,
                                    enemyWaveBank);

            enemies.Add(death);

            foreach (Enemy e in enemies)
                e.CreatePathFinder(oMap.Grid);

        }

        private void CreateEnemy(Vector2 position, AudioEngine audioEngine, WaveBank waveBank)
        {
            Texture2D zombieSpriteSheet = Game.Content.Load<Texture2D>(@"EnemySprites\GreenZombie");
            SpriteSheet zombieSS = new SpriteSheet(zombieSpriteSheet, 8);
            Texture2D zombieDisintegrateSpriteSheet = Game.Content.Load<Texture2D>(@"EnemySprites\GreenZombie_Disintegrate");
            SpriteSheet zombieDisintegrateSS = new SpriteSheet(zombieDisintegrateSpriteSheet, 8);
            Texture2D zombieBeenHitSpriteSheet = Game.Content.Load<Texture2D>(@"EnemySprites\GreenZombie_BeenHit");
            SpriteSheet zombieBeenHitSS = new SpriteSheet(zombieBeenHitSpriteSheet, 8);
            Texture2D zombieAttackSpriteSheet = Game.Content.Load<Texture2D>(@"EnemySprites\GreenZombie_Attack");
            SpriteSheet zombieAttackSS = new SpriteSheet(zombieAttackSpriteSheet, 8);
            zombie = new Zombie(zombieSS,
                                zombieDisintegrateSS,
                                zombieBeenHitSS,
                                zombieAttackSS,
                                position,
                                gameRef,
                                player.Camera,
                                audioEngine,
                                waveBank);

            zombie.Dead += missionManager.OnUpdateEvent;

            enemies.Add(zombie);
        }

        private void Populate(Vector2 spawnPoint, int count, int radius, AudioEngine audioEngine, WaveBank waveBank)
        {
            for (int i = 0; i < count; i++)
            {
                bool repeat = true;
                while (repeat)
                {
                    Vector2 position = RandomPositionInCircle(spawnPoint, radius);
                    Point pointInMap = Engine.VectorToCellIso(position);

                    if (oMap.Grid[pointInMap.X, pointInMap.Y].IsWall == false)
                    {
                        CreateEnemy(position, audioEngine, waveBank);
                        repeat = false;
                    }
                }
            }
        }

        private Vector2 RandomPositionInCircle(Vector2 center, int radius)
        {
            Vector2 position = Vector2.Zero;
            double angle = (double)RandomUtils.RANDOM.Next(0, 360);
            int range = RandomUtils.RANDOM.Next(0, radius);
            position = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            position *= range;
            position += center;
            return position;
        }

        private void LoadNpcs()
        {
            npcs = new List<NPC>();

            //Needed for dialogue system. A hack pretty much...
            chattables = new List<IChattable>();
            EntityService.GetInstance().Npcs = chattables;

            Texture2D professorReadingSpriteSheet = Game.Content.Load<Texture2D>(@"NPCSprites\Professor_Reading");
            SpriteSheet professorReadingSS = new SpriteSheet(professorReadingSpriteSheet, 13);
            Texture2D professorTalkingSpriteSheet = Game.Content.Load<Texture2D>(@"NPCSprites\Professor_Talking");
            SpriteSheet professorTalkingSS = new SpriteSheet(professorTalkingSpriteSheet, 8);
            professor = new Professor(professorReadingSS,
                                        professorTalkingSS,
                                        new Vector2(250, 50),
                                        gameRef,
                                        player.Camera);
            //TODO: Hardcoded ID
            professor.Id = 0;
            professor.Name = "Zweistein";

            npcs.Add(professor);
            chattables.Add(professor);
        }

        private void LoadSounds()
        {
            deagleShot = Game.Content.Load<SoundEffect>(@"Audio\PlayerSounds\DeagleShot");
            walkingSound = Game.Content.Load<Song>(@"Audio\PlayerSounds\Footsteps_Grass_Faster");
            MediaPlayer.IsRepeating = true;
        }

        private void UpdateMouse()
        {
            int degrees = InputHandler.GetDegrees(sprite.Origin, player.Camera.Position);

            if (degrees > 338 || degrees <= 23)
            {
                sprite.CurrentAnimation = AnimationKey.Right;
                shootingSprite.CurrentAnimation = AnimationKey.Right;
            }
            else if (degrees > 23 && degrees <= 68)
            {
                sprite.CurrentAnimation = AnimationKey.DownRight;
                shootingSprite.CurrentAnimation = AnimationKey.DownRight;
            }
            else if (degrees > 68 && degrees <= 113)
            {
                sprite.CurrentAnimation = AnimationKey.Down;
                shootingSprite.CurrentAnimation = AnimationKey.Down;
            }
            else if (degrees > 113 && degrees <= 158)
            {
                sprite.CurrentAnimation = AnimationKey.DownLeft;
                shootingSprite.CurrentAnimation = AnimationKey.DownLeft;
            }
            else if (degrees > 158 && degrees <= 203)
            {
                sprite.CurrentAnimation = AnimationKey.Left;
                shootingSprite.CurrentAnimation = AnimationKey.Left;
            }
            else if (degrees > 203 && degrees <= 248)
            {
                sprite.CurrentAnimation = AnimationKey.UpLeft;
                shootingSprite.CurrentAnimation = AnimationKey.UpLeft;
            }
            else if (degrees > 248 && degrees <= 293)
            {
                sprite.CurrentAnimation = AnimationKey.Up;
                shootingSprite.CurrentAnimation = AnimationKey.Up;
            }
            else if (degrees > 293 && degrees <= 338)
            {
                sprite.CurrentAnimation = AnimationKey.UpRight;
                shootingSprite.CurrentAnimation = AnimationKey.UpRight;
            }
            /*
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
            */
            //Console.WriteLine("{0}", degrees);
        }

        private void LoadWindows()
        {
            Rectangle screenRectangle = gameRef.ScreenRectangle;
            ContentManager content = Game.Content;
            SpriteFont font = content.Load<SpriteFont>(@"Fonts\ItemDataFont");

            notifications = NotificationPopup.GetInstance();
            notifications.Font = font;
            notifications.Size = new Vector2(300, 0);
            notifications.Position = new Vector2((screenRectangle.Width - notifications.Size.X) / 2, screenRectangle.Height * (3f / 4f));

            Texture2D tooltipBackground = content.Load<Texture2D>(@"GUI\Inventory\Tooltip\background-v2");
            Texture2D tooltipBorder = content.Load<Texture2D>(@"GUI\Inventory\Tooltip\border-v6");

            SimpleHUDWindow pauseWindow = new SimpleHUDWindow(font);
            PauseGameComponent pauseMenu = new PauseGameComponent(font, gameRef.Content);
            pauseMenu.RegisterExitButtonEvent(new EventHandler(CloseWindow));
            pauseMenu.RegisterResumeButtonEvent(new EventHandler(ClosePauseMenuWindow));
            pauseMenu.RegisterCommand(Keys.Escape, ClosePauseMenuWindow, ClickState.RELEASED);
            //pauseMenu.BackgroundTexture = tooltipBackground;

            //TODO: hardcoded offset
            pauseMenu.ButtonOffset = new Vector2(100, 50);
            pauseWindow.BorderTexture = tooltipBorder;
            //TODO: hardcoded size
            pauseWindow.Size = new Vector2(400, 400);

            Vector2 position = new Vector2((screenRectangle.Width - pauseWindow.Size.X) / 2, (screenRectangle.Height - pauseWindow.Size.Y) / 2);

            pauseWindow.Position = position;
            pauseWindow.Component = pauseMenu;
            hudManager.Put(Window.PAUSE, pauseWindow);

            InventoryHUDWindow inventoryEquipmentWindow = new InventoryHUDWindow(this, font, gameRef.Content, tooltipBorder, screenRectangle);
            ItemPickedUp += inventoryEquipmentWindow.OnItemPickup;

            hudManager.Put(Window.INVENTORY, inventoryEquipmentWindow);
            commandManager.RegisterCommand(Keys.I, inventoryEquipmentWindow.ShowEquipment);
            commandManager.RegisterCommand(Keys.U, inventoryEquipmentWindow.ShowInventory);
            inventoryEquipmentWindow.RegisterCommand(Keys.I, inventoryEquipmentWindow.ToggleEquipment);
            inventoryEquipmentWindow.RegisterCommand(Keys.U, inventoryEquipmentWindow.ToggleInventory);
            inventoryEquipmentWindow.RegisterCommand(Keys.Escape, inventoryEquipmentWindow.Close);
            inventoryEquipmentWindow.RegisterStatChangeEvent(ChangePlayerStats);
            
            SimpleHUDWindow missionLogWindow = new SimpleHUDWindow(font);
            MissionLogComponent missionLogComponent = new MissionLogComponent(font, gameRef.Content);
            //TODO: hardcoded position and size
            missionLogWindow.Size = new Vector2(800, 600);

            position = new Vector2((screenRectangle.Width - missionLogWindow.Size.X) / 2, (screenRectangle.Height - missionLogWindow.Size.Y) / 2);

            missionLogWindow.Position = position;
            missionLogWindow.Component = missionLogComponent;
            missionLogWindow.BorderTexture = tooltipBorder;
            //TODO: hardcoded title
            missionLogWindow.Title = "Mission Log";
            missionLogComponent.RegisterCommand(Keys.L, CloseMissionLogWindow, ClickState.RELEASED);
            missionLogComponent.RegisterCommand(Keys.Escape, CloseMissionLogWindow, ClickState.RELEASED);
            hudManager.Put(Window.MISSION_LOG, missionLogWindow);

            SimpleHUDWindow playerInfoWindow = new SimpleHUDWindow(font);
            PlayerInfoComponent playerInfoComponent = new PlayerInfoComponent(font, content, player);
            //TODO: hardcoded size and test position
            playerInfoWindow.Size = new Vector2(400, 300);

            position = new Vector2((screenRectangle.Width - playerInfoWindow.Size.X) / 2, (screenRectangle.Height - playerInfoWindow.Size.Y) / 2);

            playerInfoWindow.Position = position;
            playerInfoWindow.Component = playerInfoComponent;
            playerInfoWindow.BorderTexture = tooltipBorder;
            //TODO: hardcoded title
            playerInfoWindow.Title = "Player Stats";
            playerInfoComponent.RegisterCommand(Keys.C, ClosePlayerInfoWindow, ClickState.RELEASED);
            playerInfoComponent.RegisterCommand(Keys.Escape, ClosePlayerInfoWindow, ClickState.RELEASED);
            hudManager.Put(Window.PLAYER_INFO, playerInfoWindow);

            gameInterface = new GameInterface(player, font);
            //TODO: hardcoded position
            gameInterface.Position = new Vector2(screenRectangle.Width - 200, screenRectangle.Height - 60);

        }

        /// <summary>
        /// For development purposes only
        /// Items will/must be loaded from a save file/initial items file
        /// </summary>
        private void LoadItems()
        {
            InventoryHUDWindow inventory = hudManager.GetWindow(Window.INVENTORY) as InventoryHUDWindow;
            ContentManager content = gameRef.Content;
            
            helmet = ItemFactory.CreateItem("night_vision_goggles");
            inventory.EquipItem(helmet);

            mainHand = ItemFactory.CreateItem("desert_eagle");
            inventory.EquipItem(mainHand);

            armor = ItemFactory.CreateItem("kevlar_vest");
            inventory.EquipItem(armor);

            offHand = ItemFactory.CreateItem("barrel_bottom");
            inventory.EquipItem(offHand);

            leggings = ItemFactory.CreateItem("jeans");
            inventory.EquipItem(leggings);

            boots = ItemFactory.CreateItem("farmers_boots");
            inventory.EquipItem(boots);
        }

        private void LoadGun()
        {
            ContentManager content = gameRef.Content;

            //TODO: perkelk kitur, kai baigsi saudymo sistema
            Texture2D bulletTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\bullet");
            Texture2D bulletTrailTexture = content.Load<Texture2D>(@"GUI\Misc\bullet_trail");
            gun = new Gun(bulletTexture, bulletTrailTexture);
        }

        public void Reset()
        {
            ChildComponents.Clear();
            isPaused = false;
            drawEquipment = false;
        }

        private void OnDialogueSelected(object sender, EventArgs args)
        {
            if (!(sender is NewMissionData))
            {
                return;
            }

            NewMissionData newMission = (NewMissionData)sender;
            missionManager.AddMission(missions.GetMissionById(newMission.MissionId));
        }

        private void OnPlayerDeath(object sender, EventArgs args)
        {
            stateManager.ChangeState(gameRef.GameOverScreen);
        }

        #endregion
    }


}
