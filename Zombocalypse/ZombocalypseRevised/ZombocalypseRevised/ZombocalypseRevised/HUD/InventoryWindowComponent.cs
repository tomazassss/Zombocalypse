using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.HUD;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using XRPGLibrary.Controls;
using XRPGLibrary.Controls.Managers;

namespace ZombocalypseRevised.HUD
{
    public class InventoryWindowComponent : AInventoryComponent
    {
        private const int OFFSET = 100;
        private const int COLUMNS = 6;
        private const int ROWS = 8;

        private Inventory inventory;

        public override Vector2 Position
        {
            get { return position; }
            set 
            {
                this.position = value;
                inventory.OnSizeChange(position, size);
            }
        }

        public override Vector2 Size
        {
            get { return size; }
            set
            {
                this.size = value;
                inventory.OnSizeChange(position, size);
            }
        }

        public Inventory Inventory
        {
            get { return inventory; }
        }

        public InventoryWindowComponent(SpriteFont spriteFont, ContentManager content, InventoryManager inventoryManager)
            : base(spriteFont, content, inventoryManager)
        {
            Initialize();
        }

        public void Initialize()
        {
            backgroundTexture = content.Load<Texture2D>(@"GUI\Inventory\inventory_background");

            inventory = new Inventory(position, size, inventoryManager, itemManager);
            inventoryManager.RightClickNull += inventory.AddItemToFirstFreeSlot;
        }

        public override void LoadContent()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)size.X,
                (int)size.Y);

            spriteBatch.Draw(backgroundTexture, destinationRectangle, Color.White);
            itemManager.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            commandManager.Update(gameTime);
            itemManager.Update(gameTime);
        }

        public void RegisterEquip(EventHandler equip)
        {
            inventory.RegisterEquip(equip);
        }

        public void RegisterChanged(EventHandler changed)
        {
            inventory.RegisterChanged(changed);
        }
    }
}
