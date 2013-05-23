using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZombocalypseRevised;
using XRPGLibrary.HUD;
using XRPGLibrary.Util;

namespace ZombocalypseRevised.GameScreens
{
    public class PopUpManager : DrawableGameComponent
    {
        private static PopUpManager instance;
        private Stack<AHUDWindow> components;
        private Game1 gameRef;
        private int elementsToPop;
        private Queue<AHUDWindow> elementsToPush;

        private PopUpManager(Game game)
            : base(game)
        {
            components = new Stack<AHUDWindow>();
            gameRef = (Game1)game;
            elementsToPop = 0;
            elementsToPush = new Queue<AHUDWindow>();
        }

        public static PopUpManager CreateInstance(Game game)
        {
            if (instance == null)
            {
                instance = new PopUpManager(game);
            }

            return instance;
        }

        public void Push(AHUDWindow popup)
        {
            elementsToPush.Enqueue(popup);
        }

        public void Pop()
        {
            elementsToPop++;
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (AHUDWindow component in components)
            {
                //TODO: piesia langa po apacia, nes idetas i Game1 klases Components list, kuri update'ina pries visus kitus langus
                component.Draw(gameRef.SpriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            elementsToPop = 0;
            foreach (AHUDWindow component in components)
            {
                component.Update(gameTime);
            }

            for (int i = 0; i < elementsToPop; i++)
            {
                components.Pop();
            }

            for (int i = 0; i < elementsToPush.Count; i++)
            {
                components.Push(elementsToPush.Dequeue());
            }
        }
    }
}
