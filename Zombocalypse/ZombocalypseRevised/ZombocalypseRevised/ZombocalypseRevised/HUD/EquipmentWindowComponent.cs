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
using XRPGLibrary.Controls.Items;
using ZombocalypseRevised.Components;

namespace ZombocalypseRevised.HUD
{
    public class EquipmentWindowComponent : AInventoryComponent
    {
        private ItemHolder headGearSlot;
        private ItemHolder torsoSlot;
        private ItemHolder leggingsSlot;
        private ItemHolder bootsSlot;
        private ItemHolder mainHandSlot;
        private ItemHolder offHandSlot;

        private List<ItemHolder> holders;

        public event EventHandler Equipped;

        public override Vector2 Position
        {
            get { return position; }
            set { this.position = value; }
        }
        public override Vector2 Size
        {
            get { return size; }
            set { this.size = value; }
        }

        public EquipmentWindowComponent(SpriteFont spriteFont, ContentManager content, InventoryManager inventoryManager)
            : base(spriteFont, content, inventoryManager)
        {
            Initialize();
        }

        //TODO: Size ir position nenurodyti is karto. Padaryti, kad inicializuotu su ju pakeitimu
        public void Initialize()
        {
            holders = new List<ItemHolder>();
            backgroundTexture = content.Load<Texture2D>(@"GUI\Inventory\equipment_background");

            //TODO: hardcoded positionOffset
            Vector2 positionOffset = new Vector2(100, 100);

            Vector2 size = new Vector2(this.size.X / 3, this.size.Y / 4);
            Vector2 position = new Vector2(size.X, 0) + positionOffset;


            headGearSlot = new ItemHolder(position, size, itemManager);
            headGearSlot.AcceptedItems.Add(ItemKind.HEAD_GEAR);
            inventoryManager.Add(headGearSlot);

            position = new Vector2(0, size.Y) + positionOffset;

            Texture2D weaponTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\weapon");

            mainHandSlot = new ItemHolder(position, size, itemManager);
            mainHandSlot.AcceptedItems.Add(ItemKind.MAIN_HAND);
            inventoryManager.Add(mainHandSlot);

            position = new Vector2(size.X, size.Y) + positionOffset;

            Texture2D armorTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\armor");

            torsoSlot = new ItemHolder(position, size, itemManager);
            torsoSlot.AcceptedItems.Add(ItemKind.TORSO);
            inventoryManager.Add(torsoSlot);

            position = new Vector2(size.X * 2, size.Y) + positionOffset;

            Texture2D shieldTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\shield");

            offHandSlot = new ItemHolder(position, size, itemManager);
            offHandSlot.AcceptedItems.Add(ItemKind.OFF_HAND);
            inventoryManager.Add(offHandSlot);

            position = new Vector2(size.X, size.Y * 2) + positionOffset;

            Texture2D leggingsTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\leggings");

            leggingsSlot = new ItemHolder(position, size, itemManager);
            leggingsSlot.AcceptedItems.Add(ItemKind.LEGGINGS);
            inventoryManager.Add(leggingsSlot);

            position = new Vector2(size.X, size.Y * 3) + positionOffset;

            Texture2D bootsTexture = content.Load<Texture2D>(@"GUI\Inventory\Items\boots");

            bootsSlot = new ItemHolder(position, size, itemManager);
            bootsSlot.AcceptedItems.Add(ItemKind.BOOTS);
            inventoryManager.Add(bootsSlot);

            holders.Add(headGearSlot);
            holders.Add(mainHandSlot);
            holders.Add(torsoSlot);
            holders.Add(offHandSlot);
            holders.Add(leggingsSlot);
            holders.Add(bootsSlot);
            RegisterEvents();
        }

        public void RepositionHolders()
        {
            Vector2 positionOffset = Position;

            Vector2 size = new Vector2(this.size.X / 3, this.size.Y / 4);
            Vector2 position = new Vector2(size.X, 0) + positionOffset;
            headGearSlot.Position = position;
            headGearSlot.Size = size;

            position = new Vector2(0, size.Y) + positionOffset;
            mainHandSlot.Position = position;
            mainHandSlot.Size = size;

            position = new Vector2(size.X, size.Y) + positionOffset;
            torsoSlot.Position = position;
            torsoSlot.Size = size;

            position = new Vector2(size.X * 2, size.Y) + positionOffset;
            offHandSlot.Position = position;
            offHandSlot.Size = size;

            position = new Vector2(size.X, size.Y * 2) + positionOffset;
            leggingsSlot.Position = position;
            leggingsSlot.Size = size;

            position = new Vector2(size.X, size.Y * 3) + positionOffset;
            bootsSlot.Position = position;
            bootsSlot.Size = size;
        }

        public override void LoadContent()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (backgroundTexture != null)
            {
                Rectangle destinationRectangle = new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    (int)size.X,
                    (int)size.Y);

                spriteBatch.Draw(backgroundTexture, destinationRectangle, Color.White);
            }
            itemManager.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            commandManager.Update(gameTime);
            itemManager.Update(gameTime);
        }

        private void RegisterEvents()
        {
            foreach (ItemHolder holder in holders)
            {
                inventoryManager.ClickItem += holder.OnClickItem;
                inventoryManager.RightClick += holder.OnRightClick;
                inventoryManager.RightClickEquip += holder.OnRightClickEquip;
                holder.Changed += inventoryManager.OnChangedItemHolder;
                holder.Changed += OnItemChanged;
            }
        }

        public void EquipItem(InventoryItem item)
        {
            foreach (ItemHolder holder in holders)
            {
                if (holder.AcceptedItems.Contains(item.ItemData.ItemKind))
                {
                    holder.ContainedItem = item;
                    break;
                }
            }
        }

        private Stats CalculateStats()
        {
            Stats stats = new Stats();

            foreach (ItemHolder holder in holders)
            {
                if (holder.ContainedItem != null)
                {
                    foreach (ItemStat stat in holder.ContainedItem.ItemData.Stats)
                    {
                        //TODO: suskaiciuoti visus status (ir lanksciau)
                        if (stat.StatType == StatType.ARMOR)
                        {
                            stats.Armor += stat.Val;
                        }
                        if (stat.StatType == StatType.MAX_DAMAGE)
                        {
                            stats.Damage = stat.Val;
                        }
                    }
                }
            }

            return stats;
        }

        private void OnItemChanged(object sender, EventArgs args)
        {
            if (Equipped != null)
            {
                Stats playerStats = CalculateStats();
                Equipped(playerStats, null);
            }

        }

        [Obsolete("Use EquipItem")]
        public void EquipHeadGear(InventoryItem headGear)
        {
            headGearSlot.ContainedItem = headGear;
            itemManager.Add(headGear);
        }

        [Obsolete("Use EquipItem")]
        public void EquipTorso(InventoryItem torso)
        {
            torsoSlot.ContainedItem = torso;
            itemManager.Add(torso);
        }

        [Obsolete("Use EquipItem")]
        public void EquipMainHand(InventoryItem mainHand)
        {
            mainHandSlot.ContainedItem = mainHand;
            itemManager.Add(mainHand);
        }

        [Obsolete("Use EquipItem")]
        public void EquipOffHand(InventoryItem offHand)
        {
            offHandSlot.ContainedItem = offHand;
            itemManager.Add(offHand);
        }

        [Obsolete("Use EquipItem")]
        public void EquipLeggings(InventoryItem leggings)
        {
            leggingsSlot.ContainedItem = leggings;
            itemManager.Add(leggings);
        }

        [Obsolete("Use EquipItem")]
        public void EquipBoots(InventoryItem boots)
        {
            bootsSlot.ContainedItem = boots;
            itemManager.Add(boots);
        }

        public void SetEnabled(bool enabled)
        {
            foreach (ItemHolder holder in holders)
            {
                holder.Enabled = enabled;
            }
        }

        public void RegisterUnequip(EventHandler unequip)
        {
            foreach (ItemHolder holder in holders)
            {
                holder.RightClick += unequip;
            }
        }

        public void RegisterReplace(EventHandler replace)
        {
            foreach (ItemHolder holder in holders)
            {
                holder.ReplaceItem += replace;
            }
        }
    }
}
