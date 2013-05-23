using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XRPGLibrary.Util
{
    public sealed class DrawingUtils
    {
        private static Rectangle currentRectangle;

        public static void DrawBorder(Texture2D borderTexture, SpriteBatch spriteBatch, Vector2 position, Vector2 size)
        {
            DrawBorder(borderTexture, spriteBatch, position, size, 1, 1);
        }

        public static void DrawBorder(Texture2D borderTexture, SpriteBatch spriteBatch, Vector2 position, Vector2 size, int xEdgeWidth, int yEdgeHeight)
        {
            DrawBackground(borderTexture, spriteBatch, position, size);
            DrawBorderCorners(borderTexture, spriteBatch, position, size, xEdgeWidth, yEdgeHeight);
            DrawBorderEdges(borderTexture, spriteBatch, position, size, xEdgeWidth, yEdgeHeight);
        }

        private static void DrawBorderCorners(Texture2D borderTexture, SpriteBatch spriteBatch, Vector2 position, Vector2 size, int xEdgeWidth, int yEdgeHeight)
        {
            //Draw top left corner
            Vector2 currentPosition = position - new Vector2(borderTexture.Width - xEdgeWidth, borderTexture.Height - yEdgeHeight);
            Rectangle destinationRectangle = new Rectangle(
                (int)currentPosition.X,
                (int)currentPosition.Y,
                borderTexture.Width - xEdgeWidth,
                borderTexture.Height - yEdgeHeight);
            Rectangle sourceRectangle = new Rectangle(
                0,
                0, 
                borderTexture.Width - xEdgeWidth,
                borderTexture.Height - yEdgeHeight);

            spriteBatch.Draw(
                borderTexture,
                destinationRectangle,
                sourceRectangle,
                Color.White);

            //Draw top right corner
            currentPosition = new Vector2(position.X + size.X, position.Y - borderTexture.Height + yEdgeHeight);
            destinationRectangle = new Rectangle(
                (int)currentPosition.X,
                (int)currentPosition.Y,
                borderTexture.Width - xEdgeWidth,
                borderTexture.Height - yEdgeHeight);

            spriteBatch.Draw(
                borderTexture,
                destinationRectangle,
                sourceRectangle,
                Color.White,
                0f,                             //Rotation
                Vector2.Zero,                   //Origin
                SpriteEffects.FlipHorizontally, //Effects
                0f);                            //Layer Depth

            //Draw bottom left corner
            currentPosition = new Vector2(position.X - borderTexture.Width + xEdgeWidth, position.Y + size.Y);
            destinationRectangle = new Rectangle(
                (int)currentPosition.X,
                (int)currentPosition.Y,
                borderTexture.Width - xEdgeWidth,
                borderTexture.Height - yEdgeHeight);

            spriteBatch.Draw(
                borderTexture,
                destinationRectangle,
                sourceRectangle,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.FlipVertically,
                0f);

            //Draw bottom right corner
            //Origin is needed for rotation
            Vector2 origin = new Vector2(borderTexture.Width - xEdgeWidth, borderTexture.Height - yEdgeHeight);
            currentPosition = new Vector2(position.X + size.X, position.Y + size.Y);
            destinationRectangle = new Rectangle(
                (int)currentPosition.X,
                (int)currentPosition.Y,
                borderTexture.Width - xEdgeWidth,
                borderTexture.Height - yEdgeHeight);

            spriteBatch.Draw(
                borderTexture,
                destinationRectangle,
                sourceRectangle,
                Color.White,
                MathHelper.Pi,
                origin,
                SpriteEffects.None,
                0f);
        }

        private static void DrawBorderEdges(Texture2D borderTexture, SpriteBatch spriteBatch, Vector2 position, Vector2 size, int xEdgeWidth, int yEdgeHeight)
        {
            //Draw top edge
            Vector2 currentPosition = position - new Vector2(0, borderTexture.Height - yEdgeHeight);
            Rectangle destinationRectangle = new Rectangle(
                (int)currentPosition.X,
                (int)currentPosition.Y,
                (int)size.X,
                borderTexture.Height - yEdgeHeight);
            Rectangle sourceRectangle = new Rectangle(
                borderTexture.Width - xEdgeWidth,
                0,
                xEdgeWidth,
                borderTexture.Height - yEdgeHeight);

            spriteBatch.Draw(
                borderTexture,
                destinationRectangle,
                sourceRectangle,
                Color.White);

            //Draw bottom edge
            currentPosition = new Vector2(position.X, position.Y + size.Y);
            destinationRectangle = new Rectangle(
                (int)currentPosition.X,
                (int)currentPosition.Y,
                (int)size.X,
                borderTexture.Height - yEdgeHeight);

            spriteBatch.Draw(
                borderTexture,
                destinationRectangle,
                sourceRectangle,
                Color.White,
                0f,                             //Rotation
                Vector2.Zero,                   //Origin
                SpriteEffects.FlipVertically,   //Effects
                0f);                            //Layer Depth

            //Draw left edge
            currentPosition = position - new Vector2(borderTexture.Width - xEdgeWidth, 0);
            destinationRectangle = new Rectangle(
                (int)currentPosition.X,
                (int)currentPosition.Y,
                borderTexture.Width - xEdgeWidth,
                (int)size.Y);
            sourceRectangle = new Rectangle(
                0,
                borderTexture.Height - yEdgeHeight,
                borderTexture.Width - xEdgeWidth,
                yEdgeHeight);

            spriteBatch.Draw(
                borderTexture,
                destinationRectangle,
                sourceRectangle,
                Color.White);
            
            //Draw right edge
            currentPosition = new Vector2(position.X + size.X, position.Y);
            destinationRectangle = new Rectangle(
                (int)currentPosition.X,
                (int)currentPosition.Y,
                borderTexture.Width - xEdgeWidth,
                (int)size.Y);

            spriteBatch.Draw(
                borderTexture,
                destinationRectangle,
                sourceRectangle,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.FlipHorizontally,
                0f);
        }

        private static void DrawBackground(Texture2D backgroundTexture, SpriteBatch spriteBatch, Vector2 position, Vector2 size)
        {
            Rectangle sourceRectangle = new Rectangle(backgroundTexture.Width - 1, backgroundTexture.Height - 1, 1, 1);
            Rectangle destinationRectangle = new Rectangle(
                        (int)position.X,
                        (int)position.Y,
                        (int)size.X,
                        (int)size.Y);
            spriteBatch.Draw(backgroundTexture, destinationRectangle, sourceRectangle, Color.White);
        }

        /// <summary>
        /// Begins a SpriteBatch with preffered parameters
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void SpriteBatchBegin(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                Matrix.Identity);
        }

        /// <summary>
        /// A method just to keep things consistent
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void SpriteBatchEnd(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
        }

        /// <summary>
        /// Begins a SpriteBatch with a clipping rectangle
        /// If started with this method, the SpriteBatch must be ended with SpriteBatchEndClipped method
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="clippingRectangle">Rectangle of the area to draw in</param>
        public static void SpriteBatchBeginClipped(SpriteBatch spriteBatch, Rectangle clippingRectangle)
        {
            RasterizerState state = new RasterizerState() { ScissorTestEnable = true };

            spriteBatch.Begin(
               SpriteSortMode.Immediate,
               BlendState.AlphaBlend,
               null,
               null,
               state);

            currentRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;

            spriteBatch.GraphicsDevice.ScissorRectangle = clippingRectangle;
        }

        /// <summary>
        /// End a SpriteBatch with a clipping rectangle, restoring previous clipping rectangle
        /// SpriteBatchBeginClipped must be called prior to using this method
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void SpriteBatchEndClipped(SpriteBatch spriteBatch)
        {
            if (currentRectangle == null)
            {
                throw new Exception("SpriteBatch end called without calling SpriteBatchBeginClipped");
            }

            spriteBatch.GraphicsDevice.ScissorRectangle = currentRectangle;
            spriteBatch.End();
        }
    }
}
