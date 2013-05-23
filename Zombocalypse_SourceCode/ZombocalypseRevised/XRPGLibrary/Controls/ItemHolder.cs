using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.Controls.Managers;
using XRPGLibrary.Controls.Items;

namespace XRPGLibrary.Controls
{
    public class ItemHolder : Control
    {
        protected InventoryItem containedItem;
        protected Rectangle destinationRectangle;
        protected ItemManager parentItemManager;
        protected List<ItemKind> acceptedItems;

        public event EventHandler RightClick;
        public event EventHandler ReplaceItem;
        public event EventHandler Changed;

        public List<ItemKind> AcceptedItems
        {
            get { return acceptedItems; }
        }

        /// <summary>
        /// Caution: The property's setter may send an event
        /// </summary>
        public InventoryItem ContainedItem
        {
            get { return containedItem; }
            set {
                //TODO: "paemus" itema nesujungia (reikia keisti Inventory klasej)
                //TODO: perkeliant paciam, palieka itemus praeitam langelyje
                if (containedItem != null &&
                   value != null &&
                   containedItem.ItemData.IsStackable &&
                   value.ItemData.IsStackable &&
                   value.ItemData.ID == containedItem.ItemData.ID)
                {
                    containedItem.ItemData.AddToStack(value.ItemData.GetStackSize());
                    parentItemManager.Remove(value);
                }
                else
                {
                    //TODO: padaryti, kad neperdetu, jei containedItem != null && ReplaceItem == null

                    //If this ItemHolder already has an item inside
                    //and it's being replaced by another item
                    if (containedItem != null)
                    {
                        if (ReplaceItem != null && value != null)
                        {
                            ReplaceItem(containedItem, null);
                        }
                        parentItemManager.Remove(containedItem);
                    }

                    containedItem = value;
                    if (value != null)
                    {
                        containedItem.DestinationRectangle =
                            new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
                        parentItemManager.Add(containedItem);
                    }
                }
                //Notify registered objects, that this ItemHolder's containedItem has been changed
                if (Changed != null)
                {
                    Changed(containedItem, null);
                }
            }
        }

        public ItemHolder(Vector2 position, Vector2 size, ItemManager parentItemManager)
        {
            this.acceptedItems = new List<ItemKind>();
            this.position = position;
            this.size = size;
            this.destinationRectangle =
                    new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            //TODO: ar sitas isvis naudojamas?
            trackMouse = true;
            this.parentItemManager = parentItemManager;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void HandleInput()
        {
        }

        /// <summary>
        /// Handles a mouse click on this ItemHolder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnClickItem(object sender, EventArgs args)
        {
            if (!(sender is InventoryManager) || !enabled)
            {
                return;
            }
            else if (IsMouseInside())
            {
                InventoryManager manager = sender as InventoryManager;

                InventoryItem myItem = containedItem;
                InventoryItem managerItem = manager.DraggedItem;

                //If both and item is being dragged and this ItemHolder holds an item
                if (managerItem != null &&
                    myItem != null &&
                    (acceptedItems.Contains(managerItem.ItemData.ItemKind) ||
                     acceptedItems.Contains(ItemKind.ANY)))
                {
                    //If the contained item can be stacked with the item being dropped into this slot
                    if (containedItem.ItemData.IsStackable &&
                       managerItem.ItemData.IsStackable &&
                       managerItem.ItemData.ID == containedItem.ItemData.ID)
                    {
                        manager.DraggedItem = null;
                    }
                    else
                    {
                        manager.DraggedItem = myItem;
                        myItem.IsBeingDragged = true;
                        myItem.IsMouseInside = false;
                    }
                    managerItem.IsBeingDragged = false;
                    this.ContainedItem = managerItem;
                    manager.DraggedItemLastSpot = null;
                }
                //If an item is dragged and this ItemHolder doesn't hold an item
                else if (managerItem != null &&
                         myItem == null &&
                         (acceptedItems.Contains(managerItem.ItemData.ItemKind) ||
                         acceptedItems.Contains(ItemKind.ANY)))
                {
                    managerItem.IsBeingDragged = false;
                    this.ContainedItem = managerItem;
                    manager.DraggedItem = null;
                    manager.DraggedItemLastSpot = null;
                }
                //If an item is not being dragged and this ItemHolde holds an item
                else if (managerItem == null &&
                  myItem != null)
                {
                    this.ContainedItem = null;
                    manager.DraggedItem = myItem;
                    myItem.IsBeingDragged = true;
                    myItem.IsMouseInside = false;
                    manager.DraggedItemLastSpot = this;
                }
            }
        }

        /// <summary>
        /// Place an item from the InventoryManager Dragged into this ItemHolder's containedItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnRightClick(object sender, EventArgs args)
        {
            if (!(sender is InventoryManager))
            {
                return;
            }
            else
            {
                InventoryManager manager = sender as InventoryManager;

                if (manager.DraggedItemLastSpot != this)
                {
                    return;
                }
                else
                {
                    this.ContainedItem = manager.DraggedItem;
                    manager.DraggedItem = null;
                    manager.DraggedItemLastSpot = null;
                }
            }
        }

        /// <summary>
        /// Sends this ItemHolder's containedItem into another ItemHolder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnRightClickEquip(object sender, EventArgs args){
            if (!IsMouseInside() ||
                !(sender is InventoryManager) ||
                this.containedItem == null)
            {
                return;
            }
            else
            {
                if (RightClick != null)
                {
                    if (containedItem.ItemData.IsEquipable)
                    {
                        this.ContainedItem.IsMouseInside = false;
                        RightClick(containedItem, args);
                        this.ContainedItem = null;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the containedItem's property IsMouseInside if it is inside this ItemHolder
        /// </summary>
        public void UpdateMouse()
        {
            if (containedItem == null)
            {
                return;
            }

            containedItem.IsMouseInside = IsMouseInside();
        }

        public bool IsMouseInside()
        {
            MouseState state = InputHandler.MouseState;
            Rectangle mouseRectangle = new Rectangle(state.X, state.Y, 1, 1);
            Rectangle destinationRectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)size.X,
                (int)size.Y);

            return mouseRectangle.Intersects(destinationRectangle);
        }
    }
}
