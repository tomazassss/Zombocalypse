using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.Controls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using XRPGLibrary;
using XRPGLibrary.Controls.Managers;
using XRPGLibrary.Controls.Items;

namespace ZombocalypseRevised.GameScreens
{


    public class EquipmentScreen : BaseGameState
    {
        #region Field Region

        private const int OFFSET = 100;

        private PictureBox background;

        private ItemHolder headGearSlot;
        private ItemHolder torsoSlot;
        private ItemHolder leggingsSlot;
        private ItemHolder bootsSlot;
        private ItemHolder mainHandSlot;
        private ItemHolder offHandSlot;

        private Rectangle equipmentScreenRectangle;
        private InventoryManager inventoryManager;
        private ItemManager itemManager;

        private Vector2 size;
        private Vector2 position;

        #endregion

        public ItemManager ItemManager
        {
            get { return itemManager; }
        }

        public Vector2 Size
        {
            get { return size; }
            private set { this.size = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            private set { this.position = value; }
        }

        public EquipmentScreen(Game game, GameStateManager manager, InventoryManager inventoryManager)
            : base(game, manager)
        {
            this.inventoryManager = inventoryManager;
            this.itemManager = new ItemManager();
            Initialize();
        }

        //TODO: atskirti itemus nuo screenų, t.y. juos sukurti, update'int, draw'int ne equipment ar inventory screene (Done?)
        protected override void LoadContent()
        {
            base.LoadContent();

            Position = new Vector2(OFFSET, OFFSET);

            Size = new Vector2(gameRef.ScreenRectangle.Width / 2 - OFFSET,
                               gameRef.ScreenRectangle.Height - OFFSET * 2);

            ContentManager content = gameRef.Content;
            Vector2 positionOffset = new Vector2(OFFSET, OFFSET);

            Texture2D backgroundTexture = content.Load<Texture2D>(@"GUI\Inventory\equipment_background");
            equipmentScreenRectangle = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)Size.X,
                (int)Size.Y);

            background = new PictureBox(backgroundTexture, equipmentScreenRectangle);

            controlManager.Add(background);
            
            Vector2 size = new Vector2(background.Size.X / 3, background.Size.Y / 4);
            Vector2 position = new Vector2(size.X, 0) + positionOffset;

            Rectangle itemRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            headGearSlot = new ItemHolder(position, size, itemManager);
            headGearSlot.AcceptedItems.Add(ItemKind.HEAD_GEAR);
            inventoryManager.Add(headGearSlot);
            inventoryManager.ClickItem += headGearSlot.OnClickItem;
            inventoryManager.RightClick += headGearSlot.OnRightClick;

            position = new Vector2(0, size.Y) + positionOffset;

            Texture2D weaponTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\weapon");

            itemRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            mainHandSlot = new ItemHolder(position, size, itemManager);
            mainHandSlot.AcceptedItems.Add(ItemKind.MAIN_HAND);
            inventoryManager.Add(mainHandSlot);
            inventoryManager.ClickItem += mainHandSlot.OnClickItem;
            inventoryManager.RightClick += mainHandSlot.OnRightClick;

            position = new Vector2(size.X, size.Y) + positionOffset;

            Texture2D armorTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\armor");

            itemRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            torsoSlot = new ItemHolder(position, size, itemManager);
            torsoSlot.AcceptedItems.Add(ItemKind.TORSO);
            inventoryManager.Add(torsoSlot);
            inventoryManager.ClickItem += torsoSlot.OnClickItem;
            inventoryManager.RightClick += torsoSlot.OnRightClick;

            position = new Vector2(size.X * 2, size.Y) + positionOffset;

            Texture2D shieldTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\shield");

            itemRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            offHandSlot = new ItemHolder(position, size, itemManager);
            offHandSlot.AcceptedItems.Add(ItemKind.OFF_HAND);
            inventoryManager.Add(offHandSlot);
            inventoryManager.ClickItem += offHandSlot.OnClickItem;
            inventoryManager.RightClick += offHandSlot.OnRightClick;

            position = new Vector2(size.X, size.Y * 2) + positionOffset;

            Texture2D leggingsTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\leggings");

            itemRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            leggingsSlot = new ItemHolder(position, size, itemManager);
            leggingsSlot.AcceptedItems.Add(ItemKind.LEGGINGS);
            inventoryManager.Add(leggingsSlot);
            inventoryManager.ClickItem += leggingsSlot.OnClickItem;
            inventoryManager.RightClick += leggingsSlot.OnRightClick;

            position = new Vector2(size.X, size.Y * 3) + positionOffset;

            Texture2D bootsTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\boots");

            itemRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            bootsSlot = new ItemHolder(position, size, itemManager);
            bootsSlot.AcceptedItems.Add(ItemKind.BOOTS);
            inventoryManager.Add(bootsSlot);
            inventoryManager.ClickItem += bootsSlot.OnClickItem;
            inventoryManager.RightClick += bootsSlot.OnRightClick;
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
        }

        public void DrawItems(GameTime gameTime)
        {
            if (itemManager.IsActive)
            {
                itemManager.Draw(gameRef.SpriteBatch);
            }
        }

        public void SetEnabled(bool enabled)
        {
            headGearSlot.Enabled = enabled;
            torsoSlot.Enabled = enabled;
            mainHandSlot.Enabled = enabled;
            offHandSlot.Enabled = enabled;
            leggingsSlot.Enabled = enabled;
            bootsSlot.Enabled = enabled;
        }

        public void EquipHeadGear(InventoryItem headGear)
        {
            headGearSlot.ContainedItem = headGear;
            itemManager.Add(headGear);
        }

        public void EquipTorso(InventoryItem torso)
        {
            torsoSlot.ContainedItem = torso;
            itemManager.Add(torso);
        }

        public void EquipMainHand(InventoryItem mainHand)
        {
            mainHandSlot.ContainedItem = mainHand;
            itemManager.Add(mainHand);
        }

        public void EquipOffHand(InventoryItem offHand)
        {
            offHandSlot.ContainedItem = offHand;
            itemManager.Add(offHand);
        }

        public void EquipLeggings(InventoryItem leggings)
        {
            leggingsSlot.ContainedItem = leggings;
            itemManager.Add(leggings);
        }

        public void EquipBoots(InventoryItem boots)
        {
            bootsSlot.ContainedItem = boots;
            itemManager.Add(boots);
        }
    }
}
