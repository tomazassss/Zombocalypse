using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using XRPGLibrary.HUD;
using Microsoft.Xna.Framework.Graphics;
using ZombocalypseRevised.HUD;
using Microsoft.Xna.Framework;

namespace ZombocalypseRevised.Util
{
    public class WindowFactory
    {
        private static ContentManager content;
        private static string windowFontResource;
        private static string borderResource;

        public static ContentManager Content
        {
            set { content = value; }
        }

        public static string WindowFontResource
        {
            set { windowFontResource = value; }
        }

        public static string BorderResource
        {
            set { borderResource = value; }
        }

        public static SimpleHUDWindow CreatePopup(string text, EventHandler onYesPressed, EventHandler onNoPressed)
        {
            SpriteFont font = content.Load<SpriteFont>(windowFontResource);

            SimpleHUDWindow popupWindow = new SimpleHUDWindow(font);
            PopupComponent popupComponent = new PopupComponent(font, content, text);
            popupComponent.RegisterYesEvent(onYesPressed);
            popupComponent.RegisterNoEvent(onNoPressed);
            //TODO: hardcoded popup size
            popupWindow.Size = new Vector2(330, 150);
            popupWindow.Position = new Vector2(
                (GameSettings.ScreenRectangle.Width - popupWindow.Size.X) / 2,
                (GameSettings.ScreenRectangle.Height - popupWindow.Size.Y) / 2);
            popupWindow.Component = popupComponent;
            popupWindow.BorderTexture = content.Load<Texture2D>(borderResource);

            return popupWindow;
        }
    }
}
