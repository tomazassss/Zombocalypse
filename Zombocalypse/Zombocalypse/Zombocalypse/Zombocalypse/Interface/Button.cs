using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Zombocalypse
{
    public class Button
    {
        #region Attributes

        private Texture2D textureOnUp;
        private Texture2D textureOnHover;
        private Texture2D textureOnDown;
        private Texture2D textureToDraw;
        private Vector2 size;
        private Vector2 position;
        private Rectangle rectangle;
        private Color color;
        private State state;
        private double buttonTimer;

        #endregion

        #region Constructors

        public Button(ContentManager content, string buttonName)
        {
            textureOnUp = content.Load<Texture2D>(@"Interface\Buttons\Up\" + buttonName);
            textureOnHover = content.Load<Texture2D>(@"Interface\Buttons\Hover\" + buttonName + "OnHover");
            textureOnDown = content.Load<Texture2D>(@"Interface\Buttons\Down\" + buttonName + "OnDown");
            textureToDraw = textureOnUp;

            state = State.UP;
            this.color = Color.White;
        }

        #endregion

        #region Properties

        public Vector2 Position
        {
            set
            {
                this.position = value;
            }
            get
            {
                return this.position;
            }
        }

        public Vector2 Size
        {
            get
            {
                return this.size;
            }
        }

        public bool IsJustReleased
        {
            get
            {
                return this.state == State.JUST_RELEASED;
            }
        }

        #endregion

        #region Methods

        public void SetSize(GraphicsDevice graphicsDevice, float divisorWidth, float divisorHeight)
        {
            float width = graphicsDevice.Viewport.Width / divisorWidth;
            float height = graphicsDevice.Viewport.Height / divisorHeight;

            this.size = new Vector2(width, height);
        }

        private bool isImageAlphaHit(int mouseX, int mouseY)
        {
            return isImageAlphaHit(0, 0,
                textureOnUp.Width * (mouseX - rectangle.X) / rectangle.Width,
                textureOnUp.Height * (mouseY - rectangle.Y) / rectangle.Height);
        }

        private bool isImageAlphaHit(float textureX, float textureY, int mouseX, int mouseY)
        {
            if (isImageHit(textureX, textureY, mouseX, mouseY))
            {
                uint[] data = new uint[textureOnUp.Width * textureOnUp.Height];

                textureOnUp.GetData<uint>(data);

                if ((mouseX - (int)textureX) + (mouseY - (int)textureY) *
                    textureOnUp.Width < textureOnUp.Width * textureOnUp.Height)
                {
                    return ((data[
                        (mouseX - (int)textureX) + (mouseY - (int)textureY) * textureOnUp.Width
                        ] & 0xFF000000) >> 24) > 20;
                }
            }
            return false;
        }

        private bool isImageHit(float textureX, float textureY, int mouseX, int mouseY)
        {
            return (mouseX >= textureX &&
                mouseX <= textureX + textureOnUp.Width &&
                mouseY >= textureY &&
                mouseY <= textureY + textureOnUp.Height);
        }

        public void Update(GameTime gameTime, int mouseX, int mouseY, bool pressed, bool prevPressed)
        {
            double frameTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0;

            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            if (isImageAlphaHit(mouseX, mouseY))
            {
                buttonTimer = 0.0;
                if (pressed)
                {
                    state = State.DOWN;
                    textureToDraw = textureOnDown;
                }
                else if (!pressed && prevPressed)
                {
                    if (state == State.DOWN)
                    {
                        state = State.JUST_RELEASED;
                    }
                }
                else
                {
                    state = State.HOVER;
                    textureToDraw = textureOnHover;
                }
            }
            else
            {
                state = State.UP;

                if (buttonTimer > 0)
                {
                    buttonTimer -= frameTime;
                }
                else
                {
                    textureToDraw = textureOnUp;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureToDraw, rectangle, color);
        }

        #endregion

        private enum State
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }
    }
}
