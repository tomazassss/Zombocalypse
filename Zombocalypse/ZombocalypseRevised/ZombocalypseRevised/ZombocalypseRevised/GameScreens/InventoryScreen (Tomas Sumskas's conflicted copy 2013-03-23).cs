using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.Controls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using XRPGLibrary;

namespace ZombocalypseRevised.GameScreens
{
    public class InventoryScreen : BaseGameState
    {
        #region Field Region

        private static readonly int OFFSET = 100;

        private PictureBox background;
        private InventoryItem helmet;
        private InventoryItem armor;
        private InventoryItem leggings;
        private InventoryItem boots;
        private InventoryItem mainHand;
        private InventoryItem offHand;

        private ItemHolder helmetSlot;
        private ItemHolder armorSlot;
        private ItemHolder leggingsSlot;
        private ItemHolder bootsSlot;
        private ItemHolder mainHandSlot;
        private ItemHolder offHandSlot;

        private Rectangle inventoryScreenRectangle;
        private InventoryManager inventoryManager;

        #endregion

        public InventoryScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            inventoryManager = new InventoryManager();
            ContentManager content = gameRef.Content;
            Vector2 positionOffset = new Vector2(OFFSET, OFFSET);

            Texture2D backgroundTexture = content.Load<Texture2D>(@"GUI\Inventory\background");
            inventoryScreenRectangle = new Rectangle(
                OFFSET,
                OFFSET,
                gameRef.ScreenRectangle.Width / 2 - OFFSET,
                gameRef.ScreenRectangle.Height - OFFSET * 2);

            background = new PictureBox(backgroundTexture, inventoryScreenRectangle);

            controlManager.Add(background);
            
            Vector2 size = new Vector2(background.Size.X / 3, background.Size.Y / 4);
            Vector2 position = new Vector2(size.X, 0) + positionOffset;

            Texture2D helmetTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\helmet");
            Rectangle itemRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            helmet = new InventoryItem(helmetTexture, itemRectangle);
            helmetSlot = new ItemHolder(position, size);
            helmetSlot.ContainedItem = helmet;
            inventoryManager.Add(helmetSlot);
            inventoryManager.ClickItem += helmetSlot.OnClickItem;
            controlManager.Add(helmet);
            //controlManager.Drag += helmet.OnDrag;
            //controlManager.Drop += helmet.OnDrop;

            position = new Vector2(0, size.Y) + positionOffset;

            Texture2D weaponTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\weapon");

            itemRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            mainHand = new InventoryItem(weaponTexture, itemRectangle);
            mainHandSlot = new ItemHolder(position, size);
            mainHandSlot.ContainedItem = mainHand;
            inventoryManager.Add(mainHandSlot);
            inventoryManager.ClickItem += mainHandSlot.OnClickItem;
            controlManager.Add(mainHand);
            //controlManager.Drag += mainHand.OnDrag;
            //controlManager.Drop += mainHand.OnDrop;

            position = new Vector2(size.X, size.Y) + positionOffset;

            Texture2D armorTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\armor");

            itemRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            armor = new InventoryItem(armorTexture, itemRectangle);
            armorSlot = new ItemHolder(position, size);
            armorSlot.ContainedItem = armor;
            inventoryManager.Add(armorSlot);
            inventoryManager.ClickItem += armorSlot.OnClickItem;
            controlManager.Add(armor);
            //controlManager.Drag += armor.OnDrag;
            //controlManager.Drop += armor.OnDrop;

            position = new Vector2(size.X * 2, size.Y) + positionOffset;

            Texture2D shieldTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\shield");

            itemRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            offHand = new InventoryItem(shieldTexture, itemRectangle);
            offHandSlot = new ItemHolder(position, size);
            offHandSlot.ContainedItem = offHand;
            inventoryManager.Add(offHandSlot);
            inventoryManager.ClickItem += offHandSlot.OnClickItem;
            controlManager.Add(offHand);
            //controlManager.Drag += offHand.OnDrag;
            //controlManager.Drop += offHand.OnDrop;

            position = new Vector2(size.X, size.Y * 2) + positionOffset;

            Texture2D leggingsTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\leggings");

            itemRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            leggings = new InventoryItem(leggingsTexture, itemRectangle);
            leggingsSlot = new ItemHolder(position, size);
            leggingsSlot.ContainedItem = leggings;
            inventoryManager.Add(leggingsSlot);
            inventoryManager.ClickItem += leggingsSlot.OnClickItem;
            controlManager.Add(leggings);
            //controlManager.Drag += leggings.OnDrag;
            //controlManager.Drop += leggings.OnDrop;

            position = new Vector2(size.X, size.Y * 3) + positionOffset;

            Texture2D bootsTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\boots");

            itemRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            boots = new InventoryItem(bootsTexture, itemRectangle);
            bootsSlot = new ItemHolder(position, size);
            bootsSlot.ContainedItem = boots;
            inventoryManager.Add(bootsSlot);
            inventoryManager.ClickItem += bootsSlot.OnClickItem;
            controlManager.Add(boots);
            //controlManager.Drag += boots.OnDrag;
            //controlManager.Drop += boots.OnDrop;
        }

        public override void Update(GameTime gameTime)
        {
            controlManager.Update(gameTime);
            inventoryManager.Update(gameTime);

            base.Draw(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //gameRef.SpriteBatch.Begin();

            base.Update(gameTime);

            controlManager.Draw(gameRef.SpriteBatch);
            inventoryManager.Draw(gameRef.SpriteBatch);

            //gameRef.SpriteBatch.End();
        }
    }
}
