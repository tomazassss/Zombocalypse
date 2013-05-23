using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XRPGLibrary.SpriteClasses;

namespace ZombocalypseRevised.Components.Actors
{
    public class Player : Actor
    {
        #region Field Region

        private float armor;
        private float health; 
       
        private Game1 gameRef;
        private Camera camera;

        private AnimatedSprite sprite;
        private Color[] textureData;
        private Texture2D spriteSheet;

        private bool isShooting;
        private bool eventCalled;

        public event EventHandler PlayerDead;

        #endregion

        #region Property Region

        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        public override Rectangle BoundingBox
        {
            get { return new Rectangle((int)(sprite.Position.X - camera.Position.X),
                                        (int)(sprite.Position.Y - camera.Position.Y),
                                        sprite.Width,
                                        sprite.Height); }
        }

        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
            set { spriteSheet = value; }
        }

        public override Color[] TextureData
        {
            get {
                Rectangle sourceRectangle = sprite.CurrentAnimationRectangle;
                Texture2D cropTexture = new Texture2D(gameRef.GraphicsDevice, sourceRectangle.Width, sourceRectangle.Height);
                Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
                spriteSheet.GetData(0, sourceRectangle, data, 0, data.Length);
                cropTexture.SetData(data);
                textureData = new Color[cropTexture.Width * cropTexture.Height];
                cropTexture.GetData(textureData);
                return textureData; 
            }
        }

        public AnimatedSprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        public bool IsShooting
        {
            get { return isShooting; }
            set { isShooting = value; }
        }

        public float Health
        {
            get { return health; }
            set { health = value; }
        }

        public float Armor
        {
            get { return armor; }
            set { armor = value; }
        }

        #endregion

        #region Constructor Region

        public Player(Game game)
        {
            this.isShooting = false;
            this.eventCalled = false;

            this.health = 100f;
            this.armor = 0f;

            gameRef = game as Game1;
            camera = new Camera(gameRef.ScreenRectangle);
        }

        #endregion

        #region Method Region

        public override void Update(GameTime gameTime, Player player)
        {
            camera.Update(gameTime);
            if (health < 0)
            {
                Console.WriteLine("Veikejas padvese");
                if (!eventCalled)
                {
                    OnChanged(EventArgs.Empty);
                    eventCalled = true;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public override AnimatedSprite BindAnimations(Texture2D spriteSheet, int frameCount)
        {
            throw new NotImplementedException();
        }

        public override void SetCurrentAnimation(AnimatedSprite sprite, int degrees)
        {
            throw new NotImplementedException();
        }

        public void TakeDamage(float damage)
        {
            if (Armor > 0)
            {
                float temp = Armor - damage;
                if (temp < 0)
                {
                    Health += temp;
                }
            }
            else if (Armor <= 0)
            {
                Health -= damage;
            }
        }

        protected virtual void OnChanged(EventArgs e)
        {
            if (PlayerDead != null)
                PlayerDead(this, e);
        }
     
        #endregion
    }
}
