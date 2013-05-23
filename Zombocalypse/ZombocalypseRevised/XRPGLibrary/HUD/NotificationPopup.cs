using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ZombocalypseRevised.HUD
{
    public class NotificationPopup
    {
        private static readonly int TIME_TO_SHOW = 1500;
        private static NotificationPopup instance;

        private Vector2 position;
        private Vector2 size;

        private SpriteFont font;
        private Queue<string> textToShow;
        private string text;

        private int timePassed;
        private bool isShowingText;

        public Vector2 Position
        {
            get { return position; }
            set { this.position = value; }
        }

        public Vector2 Size
        {
            get { return size; }
            set { this.size = value; }
        }

        public SpriteFont Font
        {
            get { return font; }
            set { this.font = value; }
        }

        static NotificationPopup()
        {
            instance = new NotificationPopup();
        }

        private NotificationPopup()
        {
            textToShow = new Queue<string>();
            text = "";
        }

        public static NotificationPopup GetInstance()
        {
            return instance;
        }

        public void AddNotification(string text)
        {
            textToShow.Enqueue(text);
            isShowingText = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (font != null && text != "")
            {
                spriteBatch.DrawString(font, text, position, Color.Red);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (isShowingText)
            {
                timePassed += gameTime.ElapsedGameTime.Milliseconds;
                if (text == "" ||
                    timePassed > TIME_TO_SHOW)
                {
                    if (textToShow.Count > 0)
                    {
                        text = textToShow.Dequeue();
                    }
                    else
                    {
                        text = "";
                        isShowingText = false;
                    }
                    timePassed = 0;
                }
            }
        }
    }
}
