using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.Controls;
using ZombocalypseRevised.Components.Actors;
using Microsoft.Xna.Framework.Content;

namespace ZombocalypseRevised.HUD
{
    public class PlayerInfoComponent : AHUDComponent
    {
        private Player player;
        private Label healthLabel;
        private Label armorLabel;


        public override Vector2 Size
        {
            get { return size; }
            set
            {
                this.size = value;
                LayoutComponents();
            }
        }

        public override Vector2 Position
        {
            get { return position; }
            set 
            { 
                this.position = value;
                LayoutComponents();
            }
        }

        public PlayerInfoComponent(SpriteFont font, ContentManager content, Player player)
            : base(font, content)
        {
            this.player = player;

            this.healthLabel = new Label();
            this.armorLabel = new Label();

            healthLabel.Text = "Health: " + Math.Max(player.Health, 0);
            armorLabel.Text = "Armor: " + Math.Max(player.Armor, 0);

            this.controlManager.Add(healthLabel);
            this.controlManager.Add(armorLabel);
        }

        public override void LoadContent()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            controlManager.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            healthLabel.Text = "Health: " + Math.Max(player.Health, 0);
            armorLabel.Text = "Armor: " + Math.Max(player.Armor, 0);
            controlManager.Update(gameTime);
            commandManager.Update(gameTime);
        }

        private void LayoutComponents()
        {
            healthLabel.Position = position;
            armorLabel.Position = new Vector2(position.X, position.Y + healthLabel.Size.Y);
        }
    }
}
