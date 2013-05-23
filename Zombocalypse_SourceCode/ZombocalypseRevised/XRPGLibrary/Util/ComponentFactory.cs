using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.Controls;
using Microsoft.Xna.Framework.Content;
using XRPGLibrary.HUD;

namespace XRPGLibrary.Util
{
    public class ComponentFactory
    {
        private static ContentManager content;
        private static string buttonFontResource;
        private static string buttonHoverResource;

        public static ContentManager Content
        {
            set { content = value; }
        }

        public static string ButtonFontResource
        {
            set { buttonFontResource = value; }
        }

        public static string ButtonHoverResource
        {
            set { buttonHoverResource = value; }
        }

        public static SimpleButton CreateSimpleButton(string text)
        {
            Texture2D buttonHoverTexture = content.Load<Texture2D>(buttonHoverResource);
            SpriteFont buttonFont = content.Load<SpriteFont>(buttonFontResource);

            SimpleButton button = new SimpleButton(buttonHoverTexture, buttonFont);
            button.Text = text;

            return button;
        }
    }
}
