using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XRPGLibrary.Util
{
    public class StringUtils
    {
        private const string THREE_DOTS = "...";

        public static string WrapText(string text, SpriteFont font, float width)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string line in text.Split('\n'))
            {
                float currentWidth = 0;

                foreach (string word in line.Split(' '))
                {
                    Vector2 wordSize = font.MeasureString(" " + word);

                    if ((currentWidth + wordSize.X) >= width)
                    {
                        builder.Append("\n");
                        currentWidth = 0;
                    }

                    currentWidth += wordSize.X;
                    builder.Append(" " + word);
                }

                builder.Append("\n");
            }

            return builder.ToString();
        }

        public static string ShortenText(string text, SpriteFont font, float width)
        {
            if (width <= 0)
            {
                return "";
            }

            StringBuilder shortText = new StringBuilder();
            Vector2 textSize = Vector2.Zero;

            foreach (char character in text.ToCharArray())
            {
                if (textSize.X >= width)
                {
                    break;
                }

                shortText.Append(character);
                textSize = font.MeasureString(shortText);
            }

            shortText.Remove(shortText.Length - THREE_DOTS.Length, THREE_DOTS.Length);
            shortText.Append(THREE_DOTS);

            return shortText.ToString();
        }
    }

}
