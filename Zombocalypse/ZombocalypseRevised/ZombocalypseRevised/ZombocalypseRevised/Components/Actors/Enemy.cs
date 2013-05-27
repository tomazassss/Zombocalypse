using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.SpriteClasses;
using XRPGLibrary.TileEngine;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using XRPGLibrary.Collisions;
using XRPGLibrary.Animations;

namespace ZombocalypseRevised.Components.Actors
{

    public abstract class Enemy : Actor
    {
        #region Field Region

        private float health;
        private float damage;
        private float visionRange;
        private bool beenHit;
        private bool attacking;
        private bool eventCalled;

        private AnimatedSprite moveSprite;
        private AnimatedSprite deathSprite;
        private AnimatedSprite hitSprite;
        private AnimatedSprite attackSprite;

        private Texture2D spriteSheet;
        private Texture2D deathSpriteSheet;
        private Texture2D hitSpriteSheet;
        private Texture2D attackSpriteSheet;

        private Camera camera;
        private Game1 gameRef;
        private Color[] textureData;

        private AudioEngine enemyAttack;
        private SoundBank enemyAttackSoundBank;
        private WaveBank enemyAttackWaveBank;

        private String attackCue;
        private String hitCue;
        private String deathCue;

        private PathNode[,] grid;
        private PathFinder path;

        private int id;

        public event EventHandler Dead;

        private SpriteFont damageTakenFont;
        private FloatTextAnimation floatDamage;
        private float floatDuration;

        private int pathDelayCounter;
        private int attackDelayCounter;
        private bool hasAttacked;

        private bool removeThisElement;
        private float timeOfDeath;

        #endregion

        #region Property Region

        public Vector2 Position
        {
            get { return moveSprite.Position; }
            set { moveSprite.Position = value; }
        }

        public float Health
        {
            get { return health; }
            set { health = value; }
        }

        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public float VisionRange
        {
            get { return visionRange; }
            set { visionRange = value; }
        }

        public bool BeenHit
        {
            get { return beenHit; }
            set { beenHit = value; }
        }

        public bool Attacking
        {
            get { return attacking; }
            set { attacking = value; }
        }

        public AnimatedSprite Sprite
        {
            get { return moveSprite; }
            set { moveSprite = value; }
        }

        public Texture2D DeathSpriteSheet
        {
            get { return deathSpriteSheet; }
            set { deathSpriteSheet = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public PathNode[,] Grid
        {
            get { return grid; }
            set { grid = value; }
        }

        public override Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)(Sprite.Position.X - camera.Position.X),
                                      (int)(Sprite.Position.Y - camera.Position.Y),
                                      moveSprite.Width,
                                      moveSprite.Height);
            }
        }

        public override Color[] TextureData
        {
            get
            {
                Rectangle sourceRectangle = moveSprite.CurrentAnimationRectangle;
                Texture2D cropTexture = new Texture2D(gameRef.GraphicsDevice, sourceRectangle.Width, sourceRectangle.Height);
                Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
                spriteSheet.GetData(0, sourceRectangle, data, 0, data.Length);
                cropTexture.SetData(data);

                textureData = new Color[cropTexture.Width * cropTexture.Height];
                cropTexture.GetData(textureData);
                return textureData;
            }
        }

        public AudioEngine EnemyAttack
        {
            get { return enemyAttack; }
            set { enemyAttack = value; }
        }

        public SoundBank EnemyAttackSoundBank
        {
            get { return enemyAttackSoundBank; }
            set { enemyAttackSoundBank = value; }
        }

        public WaveBank EnemyAttackWaveBank
        {
            get { return enemyAttackWaveBank; }
            set { enemyAttackWaveBank = value; }
        }

        public String AttackCue
        {
            get { return attackCue; }
            set { attackCue = value; }
        }

        public String DeathCue
        {
            get { return deathCue; }
            set { deathCue = value; }
        }

        public String HitCue
        {
            get { return hitCue; }
            set { hitCue = value; }
        }

        public bool RemoveThisElement
        {
            get { return removeThisElement; }
            set { removeThisElement = value; }
        }

        #endregion

        #region Constructor Region

        public Enemy(SpriteSheet moveTexture, SpriteSheet deathTexture, SpriteSheet hitTexture, SpriteSheet attackTexture,
            Vector2 position, Game game, Camera cam)
        {
            this.spriteSheet = moveTexture.SpriteSheet1;
            this.moveSprite = BindAnimations(spriteSheet,moveTexture.AnimationFrames);
            this.moveSprite.Position = position;

            this.deathSpriteSheet = deathTexture.SpriteSheet1;
            this.deathSprite = BindAnimations(deathSpriteSheet,deathTexture.AnimationFrames);
            this.deathSprite.Position = position;

            this.hitSpriteSheet = hitTexture.SpriteSheet1;
            this.hitSprite = BindAnimations(hitSpriteSheet,hitTexture.AnimationFrames);
            this.hitSprite.Position = position;

            this.attackSpriteSheet = attackTexture.SpriteSheet1;
            this.attackSprite = BindAnimations(attackSpriteSheet,attackTexture.AnimationFrames);
            this.attackSprite.Position = position;

            this.gameRef = game as Game1;
            this.camera = cam;
            this.health = 100f;
            this.damage = 10f;
            this.beenHit = false;
            this.attacking = false;
            this.eventCalled = false;
            this.visionRange = 300f;

            this.damageTakenFont = gameRef.DamageTakenFont;
            this.floatDamage = new FloatTextAnimation();
            floatDamage.TextFont = damageTakenFont;
            floatDamage.Color = Color.WhiteSmoke;
            this.floatDuration = 2500f;

            this.pathDelayCounter = 30;
            this.attackDelayCounter = attackTexture.AnimationFrames / 2;
            this.hasAttacked = false;

            this.removeThisElement = false;
            this.timeOfDeath = 0.0f;
        }

        #endregion

        #region Method Region

        public override AnimatedSprite BindAnimations(Texture2D spriteSheet, int frameCount)
        {
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(frameCount, 48, 48, 0, 0);
            animations.Add(AnimationKey.Right, animation);

            animation = new Animation(frameCount, 48, 48, 0, 48);
            animations.Add(AnimationKey.Up, animation);

            animation = new Animation(frameCount, 48, 48, 0, 96);
            animations.Add(AnimationKey.UpRight, animation);

            animation = new Animation(frameCount, 48, 48, 0, 144);
            animations.Add(AnimationKey.UpLeft, animation);

            animation = new Animation(frameCount, 48, 48, 0, 192);
            animations.Add(AnimationKey.Down, animation);

            animation = new Animation(frameCount, 48, 48, 0, 240);
            animations.Add(AnimationKey.DownRight, animation);

            animation = new Animation(frameCount, 48, 48, 0, 288);
            animations.Add(AnimationKey.DownLeft, animation);

            animation = new Animation(frameCount, 48, 48, 0, 336);
            animations.Add(AnimationKey.Left, animation);

            return new AnimatedSprite(spriteSheet, animations);
        }

        public override void Update(GameTime gameTime, Player player)
        {
            if (health > 0)
            {
                if (this.BoundingBox.Intersects(player.BoundingBox) && Vector2.Distance(moveSprite.Position, player.Sprite.Position) < 10)
                {
                    attacking = true;                    
                }
                if (!beenHit && !attacking)
                {
                    
                    deathSprite.IsAnimating = false;
                    hitSprite.IsAnimating = false;

                    //---------------------------------------------
                    //Meginimas padaryti ai
                    Vector2 enemyPosition = moveSprite.Position;
                    enemyPosition.X += 24;
                    enemyPosition.Y += 40;

                    Vector2 playerPosition = player.Sprite.Position;
                    playerPosition.X += 24;
                    playerPosition.Y += 40;

                    int degrees = GetDegrees(enemyPosition, playerPosition);

                    if (Vector2.Distance(enemyPosition, playerPosition) < visionRange)
                    {
                        path.EnemyPosition = Engine.VectorToCellIso(enemyPosition);
                        path.PlayerPosition = Engine.VectorToCellIso(playerPosition);
                        if (Engine.VectorToCellIso(enemyPosition) == Engine.VectorToCellIso(playerPosition))
                        {
                            Vector2 direction = playerPosition - enemyPosition;
                            direction.Normalize();

                            moveSprite.IsAnimating = true;
                            moveSprite.Position += direction * moveSprite.Speed;
                        }
                        else
                        {
                            pathDelayCounter--;
                            if (pathDelayCounter <= 0)
                            {
                                path.FindPath();
                                pathDelayCounter = 30;
                            }

                            Vector2 lastNodeCenter = GetNodeCenterPosition(path.LastPosition);
                            Vector2 prevLastNodeCenter;
                            Vector2 goToPosition;
                            if (path.PreviousLastPosition.X != -1)
                            {
                                prevLastNodeCenter = GetNodeCenterPosition(path.PreviousLastPosition);
                                goToPosition.X = (prevLastNodeCenter.X + lastNodeCenter.X) / 2;
                                goToPosition.Y = (prevLastNodeCenter.Y + lastNodeCenter.Y) / 2;
                            }
                            else
                            {
                                goToPosition = lastNodeCenter;
                            }

                            Vector2 direction = goToPosition - enemyPosition;
                            direction.Normalize();

                            moveSprite.IsAnimating = true;
                            moveSprite.Position += direction * moveSprite.Speed;
                        }
                    }
                    else
                    {
                        moveSprite.IsAnimating = false;
                    }    
                    //---------------------------------------
                    hitSprite.Position = moveSprite.Position;
                    attackSprite.Position = moveSprite.Position;
                    deathSprite.Position = moveSprite.Position;

                    moveSprite.LockToMap();
                    SetCurrentAnimation(moveSprite, degrees);
                    moveSprite.Update(gameTime);
                }
                else if(beenHit)
                {
                    if (enemyAttackSoundBank.IsInUse == false)
                    {
                        enemyAttackSoundBank.PlayCue(hitCue);
                    }

                    hitSprite.AnimateOnceAndResume = true;

                    deathSprite.IsAnimating = false;
                    moveSprite.IsAnimating = false;
                    hitSprite.IsAnimating = true;

                    int degrees = GetDegrees(moveSprite.Position, player.Sprite.Position);
                    
                    hitSprite.LockToMap();
                    SetCurrentAnimation(hitSprite, degrees);
                    hitSprite.Update(gameTime);
                    if (!hitSprite.AnimateOnceAndResume)
                    {
                        beenHit = false; 
                    }
                }
                else if (attacking)
                {
                    if (enemyAttackSoundBank.IsInUse == false)
                    {
                        enemyAttackSoundBank.PlayCue(attackCue);
                    }
                    attackSprite.AnimateOnceAndResume = true;

                    deathSprite.IsAnimating = false;
                    moveSprite.IsAnimating = false;
                    hitSprite.IsAnimating = false;
                    attackSprite.IsAnimating = true;

                    int degrees = GetDegrees(moveSprite.Position, player.Sprite.Position);

                    if (attackSprite.CurrentAnimationFrame > attackDelayCounter && hasAttacked == false)
                    {
                        hasAttacked = true;
                        if (this.BoundingBox.Intersects(player.BoundingBox))
                        {
                            if (IntersectsPixel(this.BoundingBox, this.TextureData, player.BoundingBox, player.TextureData))
                            {
                                player.TakeDamage(damage);
                            }
                        }
                    }

                    attackSprite.LockToMap();
                    SetCurrentAnimation(attackSprite, degrees);
                    attackSprite.Update(gameTime);
                    if (!attackSprite.AnimateOnceAndResume)
                    {
                        attacking = false;
                        hasAttacked = false;
                    }
                }
            }
            else
            {
                if(!eventCalled)
                {
                    OnChanged(EventArgs.Empty);
                    enemyAttackSoundBank.PlayCue(deathCue);
                    eventCalled = true;
                    timeOfDeath = gameTime.TotalGameTime.Seconds;
                }
                if ((gameTime.TotalGameTime.Seconds - timeOfDeath) > 10)
                {
                    removeThisElement = true;
                }
                deathSprite.AnimateOnce = true;

                moveSprite.IsAnimating = false;
                hitSprite.IsAnimating = false;
                deathSprite.IsAnimating = true;



                deathSprite.LockToMap();
                deathSprite.Update(gameTime);
            }
            enemyAttack.Update();
            floatDamage.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (health > 0)
            {
                if (!beenHit && !attacking)
                {
                    moveSprite.Draw(gameTime, spriteBatch, camera);
                }
                else if (beenHit)
                {
                   hitSprite.Draw(gameTime, spriteBatch, camera);
                }
                else if (attacking)
                {
                    attackSprite.Draw(gameTime, spriteBatch, camera);
                }
                floatDamage.Draw(spriteBatch, camera.Position);
            }
            else
            {
                deathSprite.Draw(gameTime, spriteBatch, camera);
            }
        }

        public override void SetCurrentAnimation(AnimatedSprite sprite, int degrees)
        {
            if (degrees > 338 || degrees <= 23)
            {
                sprite.CurrentAnimation = AnimationKey.Right;
            }
            else if (degrees > 23 && degrees <= 68)
            {
                sprite.CurrentAnimation = AnimationKey.DownRight;
            }
            else if (degrees > 68 && degrees <= 113)
            {
                sprite.CurrentAnimation = AnimationKey.Down;
            }
            else if (degrees > 113 && degrees <= 158)
            {
                sprite.CurrentAnimation = AnimationKey.DownLeft;
            }
            else if (degrees > 158 && degrees <= 203)
            {
                sprite.CurrentAnimation = AnimationKey.Left;
            }
            else if (degrees > 203 && degrees <= 248)
            {
                sprite.CurrentAnimation = AnimationKey.UpLeft;
            }
            else if (degrees > 248 && degrees <= 293)
            {
                sprite.CurrentAnimation = AnimationKey.Up;
            }
            else if (degrees > 293 && degrees <= 338)
            {
                sprite.CurrentAnimation = AnimationKey.UpRight;
            }
        }

        public int GetDegrees(Vector2 source,Vector2 destination)
        {
            int degrees = 0;

            float dx = (destination.X - this.camera.Position.X) - (source.X - this.camera.Position.X);
            float dy = (destination.Y - this.camera.Position.Y) - (source.Y - this.camera.Position.Y);

            degrees = Convert.ToInt32(Math.Atan2(dy, dx) * (180 / Math.PI));
            if (degrees < 0)
            {
                degrees += 360;
            }
            return degrees;
        }

        public void CreatePathFinder(PathNode[,] grid)
        {
            this.grid = grid;
            path = new PathFinder(grid);
        }

        private Vector2 GetNodeCenterPosition(Point node)
        {
            Vector2 position = new Vector2();
            if (node.Y % 2 == 1)
            {
                position.X = 64 * node.X;
            }
            else
            {
                position.X = 64 * (node.X - 1) + 32;
            }
            position.Y = (node.Y - 1 ) * 16;
            
            return position;
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

        public void TakeDamage(float damage)
        {
            floatDamage.StartDrawing(true,
                                    damage.ToString(),
                                    moveSprite.Position,
                                    new Vector2(moveSprite.Position.X, moveSprite.Position.Y - (float)moveSprite.Height),
                                    floatDuration);
        }

        protected virtual void OnChanged(EventArgs e)
        {
            if (Dead != null)
                Dead(id, e);
        }

        #endregion
    }
}
