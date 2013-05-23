using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.Controls;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.Controls.Items;
using Microsoft.Xna.Framework;

namespace XRPGLibrary.Util
{
    public class ItemFactory
    {
        private static ContentManager content;
        private static string itemTexturePath;
        private static string itemDataPath;
        private static string borderResource;
        private static string backgroundResource;
        private static string fontResource;

        public static ContentManager Content
        {
            set { content = value; }
        }

        public static string ItemTexturePath
        {
            set { itemTexturePath = value; }
        }

        public static string ItemDataPath
        {
            set { itemDataPath = value; }
        }

        public static string BorderResource
        {
            set { borderResource = value; }
        }

        public static string BackgroundResource
        {
            set { backgroundResource = value; }
        }

        public static string FontResource
        {
            set { fontResource = value; }
        }

        public static InventoryItem CreateItem(string itemName)
        {
            Texture2D itemTexture = content.Load<Texture2D>(itemTexturePath + itemName);
            //borderTexture should be set from Game1 class
            Texture2D borderTexture = content.Load<Texture2D>(borderResource);
            //backgroundTexture should be set from Game1 class
            Texture2D backgroundTexture = content.Load<Texture2D>(backgroundResource);
            SpriteFont font = content.Load<SpriteFont>(fontResource);

            Item itemData = content.Load<Item>(itemDataPath + itemName);
            itemData.ConvertType();
            Rectangle itemRectangle = new Rectangle(0, 0, 0, 0);

            InventoryItem item = new InventoryItem(itemTexture, itemRectangle, borderTexture, backgroundTexture);
            item.ItemData = (Item)itemData.Clone();
            item.Font = font;

            return item;
        }
    }
}
