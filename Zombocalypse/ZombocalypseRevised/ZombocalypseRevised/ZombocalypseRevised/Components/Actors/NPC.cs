using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.TileEngine;
using XRPGLibrary.SpriteClasses;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XRpgLibrary.DialogueSystem;
using ZombocalypseRevised.DialogueSystem;

namespace ZombocalypseRevised.Components.Actors
{
    class NPC : Actor, IChattable, IInteractable
    {
        #region Field Region

        private bool talking;

        private AnimatedSprite readingSprite;
        private AnimatedSprite talkingSprite;

        private Texture2D readingSpriteSheet;
        private Texture2D talkingSpriteSheet;

        private Camera camera;
        private Game1 gameRef;
        private Color[] textureData;

        #endregion

        #region Property Region

        public Vector2 Position
        {
            get { return readingSprite.Position; }
            set { readingSprite.Position = value; }
        }

        public AnimatedSprite Sprite
        {
            get { return readingSprite; }
            set { readingSprite = value; }
        }

        public override Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)(Sprite.Position.X - camera.Position.X),
                                    (int)(Sprite.Position.Y - camera.Position.Y),
                                    readingSprite.Width,
                                    readingSprite.Height);
            }
        }

        public override Color[] TextureData
        {
            get
            {
                Rectangle sourceRectangle = readingSprite.CurrentAnimationRectangle;
                Texture2D cropTexture = new Texture2D(gameRef.GraphicsDevice, sourceRectangle.Width, sourceRectangle.Height);
                Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
                readingSpriteSheet.GetData(0, sourceRectangle, data, 0, data.Length);
                cropTexture.SetData(data);

                textureData = new Color[cropTexture.Width * cropTexture.Height];
                cropTexture.GetData(textureData);
                return textureData;
            }
        }

        #endregion

        #region Constructor Region

        public NPC(SpriteSheet readTexture, SpriteSheet talkTexture, Vector2 position, Game game, Camera cam)
        {
            this.readingSpriteSheet = readTexture.SpriteSheet1;
            this.readingSprite = BindAnimations(readingSpriteSheet, readTexture.AnimationFrames);
            this.readingSprite.Position = position;

            this.talkingSpriteSheet = talkTexture.SpriteSheet1;
            this.talkingSprite = BindAnimations(talkingSpriteSheet,talkTexture.AnimationFrames);
            this.talkingSprite.Position = position;

            this.gameRef = game as Game1;
            this.camera = cam;
            this.talking = false;

            this.dialogues = new List<DialogueOption>();
        }

        #endregion

        public override void Update(GameTime gameTime, Player player)
        {
            if (this.BoundingBox.Intersects(player.BoundingBox))
            {
                talking = true;                       
            }
            if (!talking)
            {
                readingSprite.IsAnimating = true;
                talkingSprite.IsAnimating = false;

                readingSprite.LockToMap();
                SetCurrentAnimation(readingSprite, 70);
                readingSprite.Update(gameTime);
            }
            else
            {
                readingSprite.IsAnimating = false;
                talkingSprite.IsAnimating = true;

                int degrees = GetDegrees(talkingSprite.Position, player.Sprite.Position);

                talkingSprite.LockToMap();
                SetCurrentAnimation(talkingSprite, degrees);
                talkingSprite.Update(gameTime);
                if (!this.BoundingBox.Intersects(player.BoundingBox))
                {
                    talking = false;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!talking)
            {
                readingSprite.Draw(gameTime, spriteBatch, camera);
            }
            else
            {
                talkingSprite.Draw(gameTime, spriteBatch, camera);
            }
        }

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

        public int GetDegrees(Vector2 source, Vector2 destination)
        {
            int degrees = 0;

            float dx = (destination.X + this.camera.Position.X) - source.X;
            float dy = (destination.Y + this.camera.Position.Y) - source.Y;

            degrees = Convert.ToInt32(Math.Atan2(dy, dx) * (180 / Math.PI));
            if (degrees < 0)
            {
                degrees += 360;
            }
            return degrees;
        }

        //Interfaces needed for dialogue system
        #region IChattable interface

        private List<DialogueOption> dialogues;
        private String name;
        private int id;

        public List<DialogueOption> Dialogues
        {
            get { return dialogues; }
            set { this.dialogues = value; }
        }

        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }

        public int Id
        {
            get { return id; }
            set { this.id = value; }
        }

        public void AddDialogue(DialogueOption option)
        {
            dialogues.Add(option);
            option.AttachChattable(this);
        }

        public void RemoveDialogue(DialogueOption option)
        {
            dialogues.Remove(option);
        }

        #endregion

        #region IInteractable interface

        public event EventHandler Interacted;

        public bool canInteract()
        {
            return talking;
        }
        
        #endregion
    }
}
