using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XRPGLibrary.Controls.Managers;
using XRPGLibrary.Controls.Items;

namespace XRPGLibrary.Controls
{
    public class Inventory
    {
        private const int COLUMNS = 6;
        private const int ROWS = 8;

        private ItemHolder[,] itemSlots;
        private ItemManager itemManager;

        public ItemHolder[,] ItemSlots
        {
            get { return itemSlots; }
            set { this.itemSlots = value; }
        }

        public Inventory(Vector2 position, Vector2 size, InventoryManager inventoryManager, ItemManager itemManager)
        {
            itemSlots = new ItemHolder[COLUMNS, ROWS];
            this.itemManager = itemManager;
            InitializeInventory(position, size, inventoryManager, itemManager);
        }

        private void InitializeInventory(Vector2 position, Vector2 size, InventoryManager inventoryManager, ItemManager itemManager)
        {
            Vector2 holderPosition = position;
            Vector2 holderSize = new Vector2(size.X / COLUMNS, size.Y / ROWS);

            for (int y = 0; y < ROWS; y++)
            {
                holderPosition.X = position.X;
                for (int x = 0; x < COLUMNS; x++)
                {
                    ItemHolder itemHolder = new ItemHolder(holderPosition, holderSize, itemManager);
                    itemHolder.AcceptedItems.Add(ItemKind.ANY);
                    itemHolder.Enabled = false;
                    inventoryManager.Add(itemHolder);
                    inventoryManager.ClickItem += itemHolder.OnClickItem;
                    inventoryManager.RightClick += itemHolder.OnRightClick;
                    inventoryManager.RightClickEquip += itemHolder.OnRightClickEquip;
                    itemHolder.Changed += inventoryManager.OnChangedItemHolder;
                    itemSlots[x, y] = itemHolder;
                    holderPosition.X += holderSize.X;
                }
                holderPosition.Y += holderSize.Y;
            }
        }

        public void OnSizeChange(Vector2 position, Vector2 size)
        {
            Vector2 holderPosition = position;
            Vector2 holderSize = new Vector2(size.X / COLUMNS, size.Y / ROWS);

            for (int y = 0; y < ROWS; y++)
            {
                holderPosition.X = position.X;
                for (int x = 0; x < COLUMNS; x++)
                {
                    ItemHolder itemHolder = itemSlots[x, y];
                    itemHolder.Position = holderPosition;
                    itemHolder.Size = holderSize;
                    holderPosition.X += holderSize.X;
                }
                holderPosition.Y += holderSize.Y;
            }
        }

        //Used when items are picked up
        public bool AddItemToFirstFreeSlot(InventoryItem item)
        {
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLUMNS; x++)
                {
                    if (itemSlots[x, y].ContainedItem != null &&
                        itemSlots[x, y].ContainedItem.ItemData.IsStackable &&
                        itemSlots[x, y].ContainedItem.ItemData.ID == item.ItemData.ID &&
                        !itemSlots[x, y].ContainedItem.ItemData.IsStackFull())
                    {
                        itemSlots[x, y].ContainedItem = item;
                        return true;
                    }
                }
            }
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLUMNS; x++)
                {
                    if (itemSlots[x, y].ContainedItem == null)
                    {
                        itemSlots[x, y].ContainedItem = item;
                        return true;
                    }
                }
            }

            return false;
        }

        //Used when putting back a dragged item
        public void AddItemToFirstFreeSlot(object sender, EventArgs args)
        {
            if (!(sender is InventoryManager))
            {
                return;
            }
            else
            {
                InventoryManager inventoryManager = sender as InventoryManager;


                if (inventoryManager.DraggedItem == null)
                {
                    return;
                }

                for (int y = 0; y < ROWS; y++)
                {
                    for (int x = 0; x < COLUMNS; x++)
                    {
                        if (itemSlots[x, y].ContainedItem == null)
                        {
                            itemSlots[x, y].ContainedItem = inventoryManager.DraggedItem;
                            inventoryManager.DraggedItem = null;
                            return;
                        }
                    }
                }
            }
        }

        public void SetEnabled(bool enabled)
        {
            foreach (ItemHolder holder in itemSlots)
            {
                holder.Enabled = enabled;
            }
        }

        public void RegisterEquip(EventHandler equip)
        {
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLUMNS; x++)
                {
                    itemSlots[x, y].RightClick += equip;
                }
            }
        }

        public void RegisterChanged(EventHandler changed)
        {
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLUMNS; x++)
                {
                    itemSlots[x, y].Changed += changed;
                }
            }
        }
    }
}
