using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XRPGLibrary.Controls
{
    public class InventoryManager : List<Control>
    {
        private InventoryItem draggedItem;

        public event EventHandler ClickItem;

        public InventoryItem DraggedItem
        {
            get { return draggedItem; }
            set { draggedItem = value; }
        }

        public InventoryManager()
            : base()
        {
            draggedItem = null;
        }

        public InventoryManager(int capacity)
            : base(capacity)
        {
            draggedItem = null;
        }

        public InventoryManager(IEnumerable<Control> collection)
            : base(collection)
        {
            draggedItem = null;
        }

        public void Update(GameTime gameTime)
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
                    ClickItem(this, null);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
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
}
