using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace XRPGLibrary.Controls
{
    public class InventoryItem : PictureBox
    {
        protected Color hoverColor;
        protected bool isMouseInside;
        protected bool isPreviouslyMouseInside;
        protected bool isBeingDragged;

        public bool IsMouseInside
        {
            get { return isMouseInside; }
            set { isMouseInside = value; }
        }

        public bool IsPreviouslyMouseInside
        {
            get { return isPreviouslyMouseInside; }
        }

        public bool IsBeingDragged
        {
            get { return isBeingDragged; }
            set { isBeingDragged = value; }
        }

        public InventoryItem(Texture2D image, Rectangle destination)
            : base(image, destination)
        {
            trackMouse = true;
            isBeingDragged = false;
            isMouseInside = false;
            isPreviouslyMouseInside = isMouseInside;
            hoverColor = Color.Red;
        }

        public InventoryItem(Texture2D image, Rectangle destination, Rectangle source)
            : base(image, destination, source)
        {
            trackMouse = true;
            isBeingDragged = false;
            isMouseInside = false;
            isPreviouslyMouseInside = isMouseInside;
            hoverColor = Color.Red;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isMouseInside)
            {
                spriteBatch.Draw(
                    image,
                    destinationRectangle,
                    sourceRectangle,
                    hoverColor);
            }
            else
            {
                spriteBatch.Draw(
                    image,
                    destinationRectangle,
                    sourceRectangle,
                    color);
            }
        }

        public void UpdateMouse()
        {
            MouseState state = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(state.X, state.Y, 1, 1);

            if (mouseRectangle.Intersects(destinationRectangle))
            {
                isPreviouslyMouseInside = isMouseInside;
                isMouseInside = true;
            }
            else
            {
                isPreviouslyMouseInside = isMouseInside;
                isMouseInside = false;
            }
        }

        public void OnDrag(object sender, EventArgs args)
        {
            if (sender != this)
            {
                trackMouse = false;
            }
        }

        public void OnDrop(object sender, EventArgs args)
        {
            trackMouse = true;
        }
    }
}
