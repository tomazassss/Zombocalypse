using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XRPGLibrary.Controls
{
    public class InventoryManager : List<Control>
    {
        private InventoryItem draggedItem;
        private ItemHolder draggedItemLastSpot;
        private bool enabled;
        private bool visible;

        public event EventHandler ClickItem;
        public event EventHandler RightClick;
        public event EventHandler RightClickNull;
        public event EventHandler RightClickEquip;
        public event EventHandler DisposeItem;

        /// <summary>
        /// Number of changed item holders after an event
        /// Used to determine, whether an item should be disposed of
        /// </summary>
        private int changedHolders;

        public bool Enabled
        {
            get { return enabled; }
            set { this.enabled = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { this.visible = value; }
        }

        public InventoryItem DraggedItem
        {
            get { return draggedItem; }
            set { this.draggedItem = value; }
        }

        public ItemHolder DraggedItemLastSpot
        {
            get { return draggedItemLastSpot; }
            set { this.draggedItemLastSpot = value; }
        }

        public InventoryManager()
            : base()
        {
            this.enabled = false;
            this.visible = false;
            this.draggedItem = null;
        }

        public InventoryManager(int capacity)
            : base(capacity)
        {
            this.enabled = false;
            this.visible = false;
            this.draggedItem = null;
        }

        public InventoryManager(IEnumerable<Control> collection)
            : base(collection)
        {
            this.enabled = false;
            this.visible = false;
            this.draggedItem = null;
        }

        public void Update(GameTime gameTime)
        {
            if (enabled)
            {
                if (base.Count == 0)
                {
                    return;
                }

                foreach (Control control in this)
                {
                    if (control.Enabled)
                    {
                        control.Update(gameTime);
                    }

                    if (control.HasFocus)
                    {
                        control.HandleInput();
                    }


                    if (control is ItemHolder)
                    {
                        ItemHolder holder = control as ItemHolder;

                        holder.UpdateMouse();

                        /*
                        if (holder.isMouseInside())
                        {
                            holder.HasFocus = true;
                        }
                        else
                        {
                            holder.HasFocus = false;
                        }
                        */
                    }

                }

                if (InputHandler.LeftButtonPressed())
                {
                    if (ClickItem != null)
                    {
                        changedHolders = 0;
                        ClickItem(this, null);
                        //If no ItemHolder was modified, fire an event
                        //that checks whether draggedItem should be dropped/disposed of
                        if ((changedHolders == 0) && 
                            (DisposeItem != null))
                        {
                            DisposeItem(this, null);
                        }
                    }
                }

                if (InputHandler.RightButtonPressed())
                {
                    if (draggedItem != null)
                    {
                        if (draggedItemLastSpot != null &&
                            RightClick != null)
                        {
                            RightClick(this, null);
                        }
                        else if (RightClickNull != null)
                        {
                            RightClickNull(this, null);
                        }
                    }
                    else
                    {
                        if (RightClickEquip != null)
                        {
                            RightClickEquip(this, null);
                        }
                    }
                }

                if (draggedItem != null)
                {
                    draggedItem.Update(gameTime);
                    if (draggedItem.IsBeingDragged)
                    {
                        Vector2 newPosition = new Vector2(InputHandler.MouseState.X - draggedItem.Size.X / 2,
                                                          InputHandler.MouseState.Y - draggedItem.Size.Y / 2);
                        draggedItem.SetPosition(newPosition);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                foreach (Control control in this)
                {
                    if (control.Visible && control != draggedItem)
                    {
                        control.Draw(spriteBatch);
                    }
                }
                if (draggedItem != null)
                {
                    draggedItem.Draw(spriteBatch);
                }
            }
        }

        public void DropDraggedItem()
        {
            if (draggedItemLastSpot != null &&
                RightClick != null)
            {
                RightClick(this, null);
            }
            else if (RightClickNull != null)
            {
                RightClickNull(this, null);
            }
        }

        /// <summary>
        /// If this method is called, then an ItemHolder's contents were modified
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnChangedItemHolder(object sender, EventArgs args)
        {
            changedHolders++;
        }

        public override string ToString()
        {
            return string.Format("Item: {0}\n draggedItemLastSpot {1}\n clickItem: {2}\n rightClick: {3}\n rightClickNull: {4}",
                                 draggedItem, draggedItemLastSpot, ClickItem, RightClick, RightClickNull);
        }
    }
}
