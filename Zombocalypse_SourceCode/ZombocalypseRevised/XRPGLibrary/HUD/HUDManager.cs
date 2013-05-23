using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.HUD;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XRPGLibrary.Controls.Managers
{
    public enum Window
    {
        PAUSE,
        INVENTORY,
        EQUIPMENT,
        MISSION_LOG,
        DIALOGUE
    }

    public class HUDManager
    {
        private Dictionary<Window, IHUDWindow> windows;

        public HUDManager()
        {
            windows = new Dictionary<Window, IHUDWindow>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Window key in windows.Keys)
            {
                windows[key].Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Window key in windows.Keys)
            {
                windows[key].Update(gameTime);
            }
        }

        public void Put(Window windowType, IHUDWindow window)
        {
            if (!windows.ContainsKey(windowType))
            {
                windows.Add(windowType, window);
            }
            else
            {
                windows[windowType] = window;
            }
        }

        public void Remove(Window windowType)
        {
            if (windows.ContainsKey(windowType))
            {
                windows.Remove(windowType);
            }
        }

        public IHUDWindow GetWindow(Window windowType)
        {
            if (windows.ContainsKey(windowType))
            {
                return windows[windowType];
            }
            else
            {
                throw new Exception("This HUDManager doesn't contain window of type " + windowType);
            }
        }
    }
}
