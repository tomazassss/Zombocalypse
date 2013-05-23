using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace XRPGLibrary.Controls.Items
{
    public class Item : ICloneable
    {
        private ItemKind itemKind;
        private String temp;
        private String itemName;
        private bool isEquipable;
        private bool isStackable;
        private int maxStack;
        private int currentStack;
        private int id;
        private List<ItemStat> stats;
        
        public String ItemType
        {
            get { return temp; }
            set { this.temp = value; }
        }

        [ContentSerializerIgnore]
        public ItemKind ItemKind
        {
            get { return itemKind; }
            set { this.itemKind = value; }
        }

        [ContentSerializerIgnore]
        public ItemKind Type
        {
            get { return itemKind; }
        }

        public String ItemName
        {
            get { return itemName; }
            set { this.itemName = value; }
        }

        public bool IsEquipable
        {
            get { return isEquipable; }
            set { this.isEquipable = value; }
        }

        public bool IsStackable
        {
            get { return isStackable; }
            set { this.isStackable = value; }
        }

        public int MaxStack
        {
            get { return maxStack; }
            set { this.maxStack = value; }
        }

        public int ID
        {
            get { return id; }
            set { this.id = value; }
        }

        public List<ItemStat> Stats
        {
            get { return stats; }
            set { this.stats = value; }
        }

        public Item()
        {
            currentStack = 1;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(itemName + "\n");
            builder.Append(itemKind.NiceValue + "\n");
            //builder.Append("Stackable: " + isStackable + "\n");
            //builder.Append("Equipable: " + isEquipable + "\n");
            //builder.Append("Max Stack: " + maxStack + "\n");
            foreach (ItemStat stat in stats)
            {
                builder.Append(stat + "\n");
            }
            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        /// <summary>
        /// Converts the string format item kind read by the content loader into an ItemKind object
        /// </summary>
        public void ConvertType()
        {
            itemKind = ItemKind.FromString(temp);
        }

        /// <summary>
        /// Adds the specified number of items into this stack
        /// </summary>
        /// <param name="numberOfItems">Number of items to add</param>
        /// <returns>0 if there is enough place for all items
        /// otherwise returns the number of items not added to this stack.
        /// Returns -1 if the item is not stackable</returns>
        public int AddToStack(int numberOfItems)
        {
            if (isStackable)
            {
                int placeLeft = maxStack - (currentStack + numberOfItems);
                int itemsToAdd = Math.Min(maxStack - currentStack, numberOfItems);
                if (placeLeft >= 0)
                {
                    currentStack += itemsToAdd;
                    return 0;
                }
                else
                {
                    currentStack += itemsToAdd;
                    return Math.Abs(placeLeft);
                }
            }
            else
            {
                return -1;
            }
        }

        public bool RemoveFromStack(int numberOfItems)
        {
            if (isStackable)
            {
                if (numberOfItems > currentStack)
                {
                    return false;
                }
                else
                {
                    currentStack -= numberOfItems;
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsStackFull()
        {
            return maxStack == currentStack;
        }

        //A getter is used instead of property to hide it from XML reader
        //If a property was used, you would have to have an entry for currentStack in every xml
        public int GetStackSize()
        {
            return currentStack;
        }

        //TODO: implementuoti Clone funkcija, nes contentManager sukuria vienam itemo tipui/failui viena objekta
        public object Clone()
        {
            Item clone = new Item();
            clone.ItemType = this.ItemType;
            clone.ItemName = this.ItemName;
            clone.ItemKind = this.ItemKind;
            clone.MaxStack = this.MaxStack;
            clone.Stats = new List<ItemStat>(this.Stats);
            clone.currentStack = 1;
            clone.ID = this.ID;
            clone.IsStackable = this.IsStackable;
            clone.IsEquipable = this.IsEquipable;

            return clone;
        }
    }
}
