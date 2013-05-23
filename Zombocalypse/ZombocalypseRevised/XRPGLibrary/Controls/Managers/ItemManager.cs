using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XRPGLibrary.Controls.Managers
{
    public class ItemManager : List<InventoryItem>
    {
        #region Field Region

        private bool isActive;

        #endregion

        #region Property Region

        public bool IsActive
        {
            get { return isActive; }
            set { this.isActive = value; }
        }

        #endregion

        #region Constructor Region

        public ItemManager()
            : base()
        {
            isActive = false;
        }

        public ItemManager(int capacity)
            : base(capacity)
        {
            isActive = false;
        }

        public ItemManager(IEnumerable<InventoryItem> collection)
            : base(collection)
        {
            isActive = false;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            foreach (InventoryItem item in this)
            {
                if (item.Enabled)
                {
                    item.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            InventoryItem toDrawLast = null;

            foreach (InventoryItem item in this)
            {
                if (item.Visible && !item.IsMouseInside)
                {
                    item.Draw(spriteBatch);
                }
                else if (item.IsMouseInside)
                {
                    toDrawLast = item;
                }
            }

            if (toDrawLast != null)
            {
                toDrawLast.Draw(spriteBatch);
                //toDrawLast.DrawItemData(spriteBatch);
            }
        }

        #endregion

        public void SetActive(bool active)
        {
            isActive = active;
            foreach (InventoryItem item in this)
            {
                item.Enabled = active;
            }
        }
    }
}
