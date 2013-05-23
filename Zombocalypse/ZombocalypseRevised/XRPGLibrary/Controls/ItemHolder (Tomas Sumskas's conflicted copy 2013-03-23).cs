using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace XRPGLibrary.Controls
{
    public class ItemHolder : Control
    {
        protected InventoryItem containedItem;
        protected Rectangle destinationRectangle;

        public InventoryItem ContainedItem
        {
            get { return containedItem; }
            set { 
                containedItem = value;
                containedItem.DestinationRectangle = 
                    new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            }
        }

        public ItemHolder(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
            this.destinationRectangle =
                    new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            trackMouse = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (containedItem != null)
            {
                containedItem.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (containedItem != null)
            {
                containedItem.Update(gameTime);
            }
        }

        public override void HandleInput()
        {
            /*
            if (InputHandler.LeftButtonPressed())
            {
                if (containedItem.IsBeingDragged)
                {
                    containedItem.DestinationRectangle =
                        new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
                }
                containedItem.IsBeingDragged = !containedItem.IsBeingDragged;
            }
             * */
        }

        public void OnDisableTrack(object sender, EventArgs args)
        {
            if (sender != this)
            {
                trackMouse = false;
            }
        }

        public void OnClick(object sender, EventArgs args)
        {
            Console.WriteLine("ON CLICK()");
            if (isMouseInside())
            {
                if (containedItem != null)
                {
                    containedItem.IsBeingDragged = true;
                }

                if (sender != null &&
                    sender is InventoryItem)
                {
                    InventoryItem item = sender as InventoryItem;
                    item.IsBeingDragged = false;
                    ContainedItem = item;
                }
            }
        }

        public void OnClickItem(object sender, EventArgs args)
        {
            if (!(sender is InventoryManager))
            {
                return;
            }
            else if (isMouseInside())
            {
                InventoryManager manager = sender as InventoryManager;

                InventoryItem myItem = containedItem;

                if (manager.DraggedItem != null &&
                    myItem != null)
                {
                    manager.DraggedItem.IsBeingDragged = false;
                    this.ContainedItem = manager.DraggedItem;
                    manager.DraggedItem = myItem;
                    myItem.IsBeingDragged = true;
                    myItem.IsMouseInside = false;
                }
                else if (manager.DraggedItem != null &&
                         myItem == null)
                {
                    manager.DraggedItem.IsBeingDragged = false;
                    this.ContainedItem = manager.DraggedItem;
                    manager.DraggedItem = null;
                }
                else if (manager.DraggedItem == null &&
                  myItem != null)
                {
                    this.containedItem = null;
                    manager.DraggedItem = myItem;
                    myItem.IsBeingDragged = true;
                    myItem.IsMouseInside = false;
                }
            }
        }

        public void UpdateMouse()
        {
            if (containedItem == null)
            {
                return;
            }

            if (isMouseInside())
            {
                containedItem.IsMouseInside = true;
            }
            else
            {
                containedItem.IsMouseInside = false;
            }
        }

        public bool isMouseInside()
        {
            MouseState state = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(state.X, state.Y, 1, 1);

            if (mouseRectangle.Intersects(destinationRectangle))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
