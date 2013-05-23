using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.Controls.Items;
using XRPGLibrary.Util;

namespace XRPGLibrary.Controls
{
    public class InventoryItem : PictureBox
    {
        protected Color hoverColor;
        protected bool isMouseInside;
        protected bool isPreviouslyMouseInside;
        protected bool isBeingDragged;
        protected Item itemData;
        protected SpriteFont font;
        protected Texture2D tooltipBorder;
        protected Texture2D tooltipBackground;

        public bool IsMouseInside
        {
            get { return isMouseInside; }
            set { this.isMouseInside = value; }
        }

        public bool IsPreviouslyMouseInside
        {
            get { return isPreviouslyMouseInside; }
        }

        public bool IsBeingDragged
        {
            get { return isBeingDragged; }
            set { this.isBeingDragged = value; }
        }

        public Item ItemData
        {
            get { return itemData; }
            set { this.itemData = value; }
        }

        public SpriteFont Font
        {
            get { return font; }
            set { this.font = value; }
        }

        public InventoryItem(Texture2D image, Rectangle destination, Texture2D tooltipBorder, Texture2D tooltipBackground)
            : base(image, destination)
        {
            this.trackMouse = true;
            this.isBeingDragged = false;
            this.isMouseInside = false;
            this.isPreviouslyMouseInside = isMouseInside;
            //TODO: hardcoded Color
            this.hoverColor = Color.LightBlue;
            this.tooltipBorder = tooltipBorder;
            this.tooltipBackground = tooltipBackground;
        }

        public InventoryItem(Texture2D image, Rectangle destination, Rectangle source, Texture2D tooltipBorder, Texture2D tooltipBackground)
            : base(image, destination, source)
        {
            this.trackMouse = true;
            this.isBeingDragged = false;
            this.isMouseInside = false;
            this.isPreviouslyMouseInside = isMouseInside;
            //TODO: hardcoded Color
            this.hoverColor = Color.LightBlue;
            this.tooltipBorder = tooltipBorder;
            this.tooltipBackground = tooltipBackground;            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isMouseInside)
            {
                DrawItemData(spriteBatch);
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
            if (itemData.IsStackable)
            {
                Vector2 textPosition = new Vector2(destinationRectangle.X, destinationRectangle.Y);
                spriteBatch.DrawString(
                    font,
                    itemData.GetStackSize().ToString(),
                    textPosition,
                    color);
            }
        }

        //TODO: tooltip piešimą perkelti į kitą klasę (greičiausiai ItemHolder)
        public void DrawItemData(SpriteBatch spriteBatch)
        {
            if (font != null && itemData != null)
            {
                String text = itemData.ToString();

                Vector2 textSize = SpriteFont.MeasureString(text);
                Vector2 textPosition = new Vector2(destinationRectangle.X, destinationRectangle.Y + destinationRectangle.Height);

                float borderWidth = (destinationRectangle.Width > textSize.X ? destinationRectangle.Width : textSize.X);
                Vector2 borderSize = new Vector2(borderWidth, destinationRectangle.Height + textSize.Y);
                float borderOffsetX = (borderWidth - destinationRectangle.Width) / 2;

                textPosition.X -= borderOffsetX;

                Vector2 borderPosition = new Vector2(destinationRectangle.X - borderOffsetX, destinationRectangle.Y);

                DrawingUtils.DrawBorder(tooltipBorder, spriteBatch, borderPosition, borderSize);

                spriteBatch.DrawString(font, text, textPosition, Color.White);
            }
        }

        [Obsolete("isMouseInside is set by ItemHolder")]
        public void UpdateMouse()
        {
            MouseState state = InputHandler.MouseState;
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
    }
}
