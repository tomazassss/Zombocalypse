using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XRPGLibrary.TileEngine;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary;
using XRPGLibrary.Util;
using XRPGLibrary.SpriteClasses;
using ZombocalypseRevised.Components.Actors;

namespace ZombocalypseRevised.Components
{
    public class Bullet
    {
        #region Field Region

        private Vector2 position;
        private Texture2D sprite;
        private Texture2D bulletTrail;
        private Vector2 speed;
        private Vector2 direction;
        private Vector2 startingPoint;
        private int firingTimer;
        private int separatorX;
        private int separatorY;
        private float rotationAngle;
        private Color[] textureData;

        #endregion

        #region Property Region

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 StartingPoint
        {
            get { return startingPoint; }
            set { startingPoint = value; }
        }

        public int FiringTimer
        {
            get { return firingTimer; }
            set { firingTimer = value; }
        }

        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X,
                                      (int)position.Y,
                                      sprite.Width,
                                      sprite.Height);
            }
        }

        public Color[] TextureData
        {
            get { return textureData; }
        }

        public float RotationAngle
        {
            get { return rotationAngle; }
            set { rotationAngle = value; }
        }

        #endregion

        #region Constructor Region

        public Bullet(Texture2D sprite, Texture2D trail, int speed)
        {
            this.position = new Vector2(0, 0);
            this.startingPoint = new Vector2(0,0);
            this.separatorX = RandomUtils.RANDOM.Next(0, 0);
            this.separatorY = RandomUtils.RANDOM.Next(0, 0);
            this.speed = new Vector2(speed + separatorX, speed + separatorY);
            this.sprite = sprite;
            this.bulletTrail = trail;
            this.firingTimer = 0;
            textureData = new Color[this.sprite.Width * this.sprite.Height];
            this.sprite.GetData(textureData);
        }

        #endregion

        #region Method Region

        public void Update()
        {
            this.position += direction * speed;
            this.firingTimer++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, rotationAngle, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            spriteBatch.Draw(bulletTrail, new Vector2(startingPoint.X + 1,startingPoint.Y + 1), null, Color.White, rotationAngle, Vector2.Zero, new Vector2(Vector2.Distance(startingPoint, position) / bulletTrail.Width, 1), SpriteEffects.None, 0f);
        }

        #endregion

    }
}
