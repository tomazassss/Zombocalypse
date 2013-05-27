using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombocalypseRevised.Components.Actors;

namespace ZombocalypseRevised.HUD
{
    public class GameInterface
    {
        private Player player;
        private Label healthLabel;
        private ControlManager controlManager;
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set 
            { 
                position = value;
                healthLabel.Position = value;
            }
        }

        public GameInterface(Player player, SpriteFont font)
        {
            this.player = player;
            controlManager = new ControlManager(font);

            healthLabel = new Label();
            healthLabel.Color = Color.Cyan;
            healthLabel.Text = "Health: " + player.Health.ToString();

            controlManager.Add(healthLabel);
        }

        public void Update(GameTime gameTime)
        {
            healthLabel.Text = "Health: " + player.Health.ToString();
            controlManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            controlManager.Draw(spriteBatch);
        }
    }
}
