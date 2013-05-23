using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XRPGLibrary;
using XRPGLibrary.Controls;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.Controls.Managers;

namespace ZombocalypseRevised.GameScreens
{
    public class InventoryScreen : BaseGameState
    {
        private const int OFFSET = 100;
        private const int COLUMNS = 6;
        private const int ROWS = 8;

        private PictureBox background;
        private InventoryManager inventoryManager;
        private ItemManager itemManager;
        private Inventory inventory;

        private ItemHolder[,] itemSlots;
        private Vector2 size;
        private Vector2 position;

        public ItemManager ItemManager
        {
            get { return itemManager; }
        }

        public Vector2 Size
        {
            get { return size; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Inventory Inventory
        {
            get { return inventory; }
        }

        public InventoryScreen(Game game, GameStateManager manager, InventoryManager inventoryManager)
            : base(game, manager)
        {
            this.inventoryManager = inventoryManager;
            this.itemManager = new ItemManager();
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            
            position = new Vector2(gameRef.ScreenRectangle.Width / 2, OFFSET);

            size = new Vector2(gameRef.ScreenRectangle.Width / 2 - OFFSET,
                               gameRef.ScreenRectangle.Height - OFFSET * 2);

            itemSlots = new ItemHolder[COLUMNS, ROWS];
            //InitializeItemSlots();

            inventory = new Inventory(position, size, inventoryManager, itemManager);
            inventoryManager.RightClickNull += inventory.AddItemToFirstFreeSlot;
            ContentManager content = Game.Content;

            Texture2D backgroundTexture = content.Load<Texture2D>(@"GUI\Inventory\inventory_background");
            Rectangle inventoryRectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)size.X,
                (int)size.Y);

            background = new PictureBox(backgroundTexture, inventoryRectangle);

            controlManager.Add(background);
        }

        public override void Update(GameTime gameTime)
        {
            controlManager.Update(gameTime);
            if (itemManager.IsActive)
            {
                itemManager.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            controlManager.Draw(gameRef.SpriteBatch);
            /*
            if (itemManager.IsActive)
            {
                itemManager.Draw(gameRef.SpriteBatch);
            }
            */
        }

        public void DrawItems(GameTime gameTime)
        {
            if (itemManager.IsActive)
            {
                itemManager.Draw(gameRef.SpriteBatch);
            }
        }

        /*
        private void InitializeItemSlots()
        {
            Vector2 holderPosition = position;
            Vector2 holderSize = new Vector2(size.X / COLUMNS, size.Y / ROWS);
            
            for (int y = 0; y < ROWS; y++)
            {
                holderPosition.X = position.X;
                for (int x = 0; x < COLUMNS; x++)
                {
                    ItemHolder itemHolder = new ItemHolder(holderPosition, holderSize, itemManager);
                    inventoryManager.Add(itemHolder);
                    inventoryManager.ClickItem += itemHolder.OnClickItem;
                    inventoryManager.RightClick += itemHolder.OnRightClick;
                    itemSlots[x, y] = itemHolder;
                    holderPosition.X += holderSize.X;
                }
                holderPosition.Y += holderSize.Y;
            }
        }
        */
    }
}
