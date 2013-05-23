using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.HUD;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using XRPGLibrary.Controls;
using XRPGLibrary.Controls.Managers;

namespace ZombocalypseRevised.HUD
{
    public abstract class AInventoryComponent : AHUDComponent
    {
        protected InventoryManager inventoryManager;
        protected ItemManager itemManager;

        public InventoryManager InventoryManager
        {
            get { return inventoryManager; }
            set { this.inventoryManager = value; }
        }

        public ItemManager ItemManager
        {
            get { return itemManager; }
            set { this.itemManager = value; }
        }

        public AInventoryComponent(SpriteFont spriteFont, ContentManager content, InventoryManager inventoryManager)
            : base(spriteFont, content)
        {
            this.inventoryManager = inventoryManager;
            this.itemManager = new ItemManager();
        }
    }
}
