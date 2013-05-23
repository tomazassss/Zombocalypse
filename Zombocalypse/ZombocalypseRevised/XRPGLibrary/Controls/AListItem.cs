using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XRPGLibrary.Controls
{
    public abstract class AListItem
    {
        protected Vector2 size;
        protected Vector2 position;
        protected SpriteFont font;
        protected bool isSelected;

        protected Color normalColor;
        protected Color hoverColor;
        protected Color renderColor;

        public abstract event EventHandler Selected; 

        public virtual Vector2 Size
        {
            get { return size; }
            set { this.size = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { this.position = value; }
        }

        public SpriteFont Font
        {
            get { return font; }
            set { this.font = value; }
        }

        public virtual bool IsSelected
        {
            get { return isSelected; }
            set { this.isSelected = value; }
        }

        public AListItem()
        {
            this.normalColor = Color.White;
            this.hoverColor = new Color(251, 177, 23);
            this.renderColor = this.normalColor;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);

        public bool IsMouseInside()
        {
            MouseState state = InputHandler.MouseState;
            Rectangle mouseRectangle = new Rectangle(state.X, state.Y, 1, 1);

            Rectangle itemRectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)size.X,
                (int)size.Y);

            return mouseRectangle.Intersects(itemRectangle);
        }
    }
}
