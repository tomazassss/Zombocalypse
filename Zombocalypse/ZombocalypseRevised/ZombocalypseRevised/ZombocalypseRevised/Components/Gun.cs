using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using XRPGLibrary.Util;
using XRPGLibrary.SpriteClasses;
using ZombocalypseRevised.Components.Actors;

namespace ZombocalypseRevised.Components
{
    public class Gun
    {
        #region Field region

        private List<Bullet> magazine;
        private Texture2D bulletTexture;
        private Texture2D bulletTrail;
        private int range;
        private float accuracy;
        private float damage;

        #endregion

        #region Property Region

        public List<Bullet> Magazine
        {
            get { return magazine; }
            set { magazine = value; }
        }

        public int Range
        {
            get { return range; }
            set { range = value; }
        }

        public float Accuracy
        {
            get { return accuracy; }
            set { accuracy = value; }
        }

        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        #endregion

        #region Constructor Region

        public Gun(Texture2D texture, Texture2D trail)
        {
            this.magazine = new List<Bullet>();
            this.bulletTexture = texture;
            this.bulletTrail = trail;
            this.range = 7;
            this.accuracy = 0.0f;
            this.damage = 10f;
        }

        #endregion

        #region Method Region

        public void Update(List<Enemy> enemies)
        {
            int x = magazine.Count;
            bool beenHit = false;
            for (int i = 0; i < x; i++)
            {
                if (magazine[i] != null)
                {
                    Bullet b = magazine[i];
                    b.Update();
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        if (enemies[j] != null)
                        {
                            if (b.BoundingBox.Intersects(enemies[j].BoundingBox))
                            {
                                if (IntersectsPixel(b.BoundingBox, b.TextureData, enemies[j].BoundingBox, enemies[j].TextureData) == true)
                                {
                                    enemies[j].Health -= Damage;
                                    enemies[j].BeenHit = true;
                                    beenHit = true;
                                    continue;
                                }
                                Vector2 delta = b.Position + (b.Direction * b.Speed);
                                if (Collides(b.Position.X, b.Position.Y, delta.X, delta.Y, enemies[j].BoundingBox))
                                {
                                    enemies[j].Health -= Damage;
                                    enemies[j].BeenHit = true;
                                    beenHit = true;
                                }
                            }
                            
                        }
                    }
                    if (b.FiringTimer > range || beenHit == true)
                    {
                        Remove(b);
                        x--;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in magazine)
            {
                b.Draw(spriteBatch);
            }
        }

        public void Add(Vector2 motion, Vector2 spriteOrigin, int speed, int rotationAngle)
        {
            Bullet b = new Bullet(bulletTexture, bulletTrail, speed);
            magazine.Add(b);
            b.Position = spriteOrigin;
            b.StartingPoint = spriteOrigin;
            float angle = (float)(rotationAngle * (Math.PI / 180));
            b.RotationAngle = angle;
            Vector2 direction = motion - spriteOrigin;
            direction.Normalize();
            direction.X += (float)(RandomUtils.RANDOM.NextDouble() - 0.5) * accuracy;
            direction.Y += (float)(RandomUtils.RANDOM.NextDouble() - 0.5) * accuracy;
            direction.Normalize();
            b.Direction = direction;
        }

        private void Remove(Bullet bullet)
        {
            magazine.Remove(bullet);
        }

        private bool IntersectsPixel(Rectangle rect1, Color[] data1,
                                     Rectangle rect2, Color[] data2)
        {
            int top = Math.Max(rect1.Top, rect2.Top);
            int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
            int left = Math.Max(rect1.Left, rect2.Left);
            int right = Math.Min(rect1.Right, rect2.Right);

            for (int y = top; y < bottom; y++)
                for (int x = left; x < right; x++)
                {
                    Color color1 = data1[(x - rect1.Left) +
                                         (y - rect1.Top) * rect1.Width];
                    Color color2 = data2[(x - rect2.Left) +
                                         (y - rect2.Top) * rect2.Width];

                    if (color1.A != 0 && color2.A != 0)
                    {
                        return true;
                    }
                }

            return false;
        }

        private bool Collides(float x, float y, float vx, float vy, Rectangle rect)
        {
            //x; y Vektoriaus (kulkos dabartine pozicija)
            //vx ir vy Vektoriaus busima pozicija
            // left, right, top, bottom - sprite'o ribojancio kvadrato krastines
            float[] p = new float[4] {-vx,vx,-vy, vy};
            float[] q = new float[4] { x - rect.Left, rect.Right - x, y - rect.Top, rect.Bottom - y };
            double u1 = double.NegativeInfinity;
            double u2 = double.PositiveInfinity;

            for (int i = 0; i < 4; i++)
            {
                if (p[i] == 0)
                {
                    if (q[i] < 0)
                    {
                        return false;
                    }
                }
                else
                {
                    float t = q[i] / p[i];
                    if (p[i] < 0 && u1 < t)
                    {
                        u1 = t;
                    }
                    else if(p[i] > 0 && u2 > t)
                    {
                        u2 = t;
                    }
                }
            }

            if (u1 > u2 || u1 > 1 || u1 < 0)
            {
                return false;
            }

            return true;
        }

        #endregion

    }
}
