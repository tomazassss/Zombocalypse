using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XRPGLibrary.Controls
{
    public class ItemList : Control
    {
        private List<AListItem> items;
        private Vector2 positionToAdd;
        private SpriteFont font;

        public event EventHandler SelectionChange;

        public ItemList(SpriteFont font)
        {
            this.font = font;
            SizeChanged += OnSizeChange;
            PositionChanged += OnPositionChange;
            items = new List<AListItem>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (AListItem item in items)
            {
                item.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (AListItem item in items)
            {
                item.Draw(spriteBatch);
            }
        }

        public override void HandleInput()
        {
        }

        private void PositionItems()
        {
            Vector2 currentPosition = position;
            float itemWidth = size.X;

            foreach (AListItem item in items)
            {
                item.Size = new Vector2(itemWidth, item.Size.Y);
                item.Position = currentPosition;

                currentPosition.Y += item.Size.Y;
            }
            positionToAdd = currentPosition;
        }

        public void AddItem(AListItem item)
        {
            items.Add(item);
            item.Selected += OnChangeSelection;
            item.Position = positionToAdd;
            item.Font = font;
            item.Size = new Vector2(size.X, item.Size.Y);
            positionToAdd.Y += item.Size.Y;
        }

        public void RemoveItem(AListItem item)
        {
            items.Remove(item);
            PositionItems();
        }

        private void OnSizeChange(object sender, EventArgs args)
        {
            PositionItems();
        }

        private void OnPositionChange(object sender, EventArgs args)
        {
            PositionItems();
        }

        private void OnChangeSelection(object sender, EventArgs args)
        {
            foreach (AListItem item in items)
            {
                if (item != sender)
                {
                    item.IsSelected = false;
                }
                else
                {
                    item.IsSelected = true;
                }
            }

            if (SelectionChange != null)
            {
                SelectionChange(sender, args);
            }
        }

        public void Reset()
        {
            foreach (AListItem item in items)
            {
                item.IsSelected = false;
            }
        }

        public void Clear()
        {
            items.Clear();
            positionToAdd = position;
        }
    }
}
