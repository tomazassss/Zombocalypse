using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XRPGLibrary.Controls
{
    public class PictureBox : Control
    {
        #region Field Region

        protected Texture2D image;
        protected Rectangle sourceRectangle;
        protected Rectangle destinationRectangle;

        #endregion

        #region Property Region

        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        public Rectangle SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }

        public Rectangle DestinationRectangle
        {
            get { return destinationRectangle; }
            set { destinationRectangle = value; }
        }

        #endregion

        #region Constructor Region

        public PictureBox(Texture2D image, Rectangle destination)
        {
            this.image = image;
            this.destinationRectangle = destination;
            this.sourceRectangle = new Rectangle(0, 0, image.Width, image.Height);
            this.size = new Vector2(image.Width, image.Height);
            this.color = Color.White;
        }

        public PictureBox(Texture2D image, Rectangle destination, Rectangle source)
        {
            this.image = image;
            this.destinationRectangle = destination;
            this.sourceRectangle = source;
            this.color = Color.White;
        }

        #endregion

        #region Abstract Method Region

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                image,
                destinationRectangle,
                sourceRectangle,
                color);
        }

        public override void HandleInput()
        {
        }

        #endregion

        #region Picture Box Method Region

        public void SetPosition(Vector2 newPosition)
        {
            destinationRectangle = new Rectangle(
                (int)newPosition.X,
                (int)newPosition.Y,
                sourceRectangle.Width,
                sourceRectangle.Height);
        }

        #endregion
    }
}
