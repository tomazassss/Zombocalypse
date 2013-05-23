using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using XRPGLibrary.Controls;
using XRPGLibrary.Util;

namespace ZombocalypseRevised.HUD
{
    public class PopupComponent : AHUDComponent
    {
        private const float SPACE_BETWEEN_COMPONENTS = 5f;

        private SimpleButton yesButton;
        private SimpleButton noButton;
        private Label textLabel;

        public PopupComponent(SpriteFont font, ContentManager content, string text)
            : base(font, content)
        {
            this.yesButton = ComponentFactory.CreateSimpleButton("Yes");
            this.noButton = ComponentFactory.CreateSimpleButton("No");
            this.textLabel = new Label();
            this.textLabel.Text = text;
            this.textLabel.Size = font.MeasureString(this.textLabel.Text);

            controlManager.Add(this.textLabel);
            controlManager.Add(yesButton);
            controlManager.Add(noButton);
            controlManager.NextControl();
        }

        public override Vector2 Size
        {
            get
            {
                return size;
            }
            set
            {
                this.size = value;
                PositionComponents();
            }
        }

        public override Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                this.position = value;
                PositionComponents();
            }
        }

        public override void LoadContent()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                controlManager.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (enabled)
            {
                controlManager.Update(gameTime);
            }
        }

        private void PositionComponents()
        {
            textLabel.Position = new Vector2(position.X + (size.X - textLabel.Size.X) / 2, position.Y);

            Vector2 buttonPosition = new Vector2(position.X + (size.X - yesButton.Size.X) / 2, position.Y + textLabel.Size.Y);
            yesButton.Position = buttonPosition;
            yesButton.Position = new Vector2(position.X + (size.X - yesButton.Size.X) / 2, textLabel.Position.Y + textLabel.Size.Y + SPACE_BETWEEN_COMPONENTS);

            buttonPosition.X = position.X + (size.X - noButton.Size.X) / 2;
            buttonPosition.Y += yesButton.Size.Y + SPACE_BETWEEN_COMPONENTS;
            noButton.Position = buttonPosition;
        }

        public void RegisterYesEvent(EventHandler evnt)
        {
            yesButton.Selected += evnt;
        }

        public void RegisterNoEvent(EventHandler evnt)
        {
            noButton.Selected += evnt;
        }
    }
}
