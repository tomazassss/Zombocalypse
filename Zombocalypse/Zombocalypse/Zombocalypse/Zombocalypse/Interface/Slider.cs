using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Zombocalypse
{
    public class Slider
    {
        Point currentResolution;
        List<Point> resolutions;
        Button leftArrow;
        Button rightArrow;
        Texture2D texture;
        Rectangle rectangle;
        Vector2 position;
        Vector2 size;

        SpriteFont spriteFont;

        int currentPosition;

        public Slider(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"Interface/Buttons/Base");
            spriteFont = content.Load<SpriteFont>(@"Fonts/Font");
            leftArrow = new Button(content, "LeftArrow");
            rightArrow = new Button(content, "RightArrow");
            currentPosition = 0;
        }

        public Vector2 Position
        {
            set
            {
                leftArrow.Position = value;
                this.position = value + new Vector2(leftArrow.Size.X, 0);
                rightArrow.Position = value + new Vector2(leftArrow.Size.X + this.size.X, 0);
            }
            get
            {
                return leftArrow.Position;
            }
        }

        public void SetSize(GraphicsDevice graphics)
        {
            this.size = new Vector2(graphics.Viewport.Width / 5, graphics.Viewport.Height / 18);
            leftArrow.SetSize(graphics, 32, 18);
            rightArrow.SetSize(graphics, 32, 18);
        }

        public Vector2 Size
        {
            get
            {
                return leftArrow.Size + size + rightArrow.Size;
            }
        }

        public List<Point> Resolutions
        {
            get
            {
                if (resolutions == null)
                {
                    resolutions = new List<Point>();
                }

                return resolutions;
            }
            set
            {
                this.resolutions = value;
            }
        }

        public Point CurrentResolution
        {
            get
            {
                return this.currentResolution;
            }
            set
            {

            }
        }

        public Point Selected
        {
            get
            {
                return resolutions[currentPosition];
            }
        }

        public void Update(GameTime gameTime, int mouseX, int mouseY, bool pressed, bool prevPressed)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y,
                                      (int)size.X, (int)size.Y);

            if (leftArrow.IsJustReleased && currentPosition > 0)
            {
                currentPosition -= 1;
                /*
                if (currentPosition < 0)
                {
                    currentPosition = resolutions.Count - 1;
                }
                */
            }

            if (rightArrow.IsJustReleased && currentPosition < resolutions.Count - 1)
            {
                currentPosition += 1;
                /*
                if (currentPosition >= resolutions.Count)
                {
                    currentPosition = 0;
                }
                */
            }

            leftArrow.Update(gameTime, mouseX, mouseY, pressed, prevPressed);
            rightArrow.Update(gameTime, mouseX, mouseY, pressed, prevPressed);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Point resolution = resolutions[currentPosition];
            string resolutionString = string.Format("{0}x{1}", resolution.X, resolution.Y);

            spriteBatch.Draw(texture, rectangle, Color.White);
            spriteBatch.DrawString(spriteFont, resolutionString, position, Color.White);
            leftArrow.Draw(spriteBatch);
            rightArrow.Draw(spriteBatch);
        }
    }
}
