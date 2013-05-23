using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.Controls;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using XRPGLibrary.Input;
using XRPGLibrary.Util;

//TODO: Padaryti visus mygtukus ir jų išdėstymą
//TODO: ištrinti nereikalingus/senus mygtukus
namespace ZombocalypseRevised.HUD
{
    public class PauseGameComponent : AHUDComponent
    {
        private Vector2 buttonOffset;
        private List<AnimatedButton> buttons;
        private AnimatedButton exitGameButton;
        private AnimatedButton resumeGameButton;

        //TODO: palikti arba simple button arba animated button, ne abu
        private List<SimpleButton> simpleButtons;
        private SimpleButton simpleResumeGameButton;
        private SimpleButton simpleExitGameButton;
        private SimpleButton simpleSaveGameButton;
        private SimpleButton simpleLoadGameButton;

        private const int SPACE_BETWEEN_BUTTONS = 5;

        public override Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                this.position = value;
                //exitGameButton.Position = buttonOffset + value;
                //simpleExitGameButton.Position = buttonOffset + value;
                LayOutButtons();
            }
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
                //buttonOffset = new Vector2((this.size.X - exitGameButton.Size.X) / 2, buttonOffset.Y);
                buttonOffset = new Vector2((this.size.X - simpleExitGameButton.Size.X) / 2, buttonOffset.Y);
            }
        }

        public Vector2 ButtonOffset
        {
            get { return buttonOffset; }
            set { this.buttonOffset = value; }
        }

        public PauseGameComponent(SpriteFont font, ContentManager content)
            : base(font, content)
        {
        }

        public override void LoadContent()
        {
            //buttons = new List<AnimatedButton>();
            simpleButtons = new List<SimpleButton>();

            /*
            Texture2D buttonTexture = content.Load<Texture2D>(@"GUI\Buttons\Exit");
            Texture2D buttonSelectedTexture = content.Load<Texture2D>(@"GUI\Buttons\SelectedButton");
            exitGameButton = new AnimatedButton(buttonTexture, buttonSelectedTexture);
            controlManager.Add(exitGameButton);

            buttonTexture = content.Load<Texture2D>(@"GUI\Buttons\Start");
            resumeGameButton = new AnimatedButton(buttonTexture, buttonSelectedTexture);
            controlManager.Add(resumeGameButton);

            buttons.Add(resumeGameButton);
            buttons.Add(exitGameButton);
            */

            simpleResumeGameButton = ComponentFactory.CreateSimpleButton("Resume");
            controlManager.Add(simpleResumeGameButton);

            simpleLoadGameButton = ComponentFactory.CreateSimpleButton("Load Game");
            //controlManager.Add(simpleLoadGameButton);

            simpleSaveGameButton = ComponentFactory.CreateSimpleButton("Save Game");
            //controlManager.Add(simpleSaveGameButton);

            simpleExitGameButton = ComponentFactory.CreateSimpleButton("Exit");
            controlManager.Add(simpleExitGameButton);


            simpleButtons.Add(simpleResumeGameButton);
            //simpleButtons.Add(simpleLoadGameButton);
            //simpleButtons.Add(simpleSaveGameButton);
            simpleButtons.Add(simpleExitGameButton);

            controlManager.NextControl();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (backgroundTexture != null)
            {
                Rectangle destinationRectangle = new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    (int)size.X,
                    (int)size.Y);

                spriteBatch.Draw(
                    backgroundTexture,
                    destinationRectangle,
                    Color.White);
            }

            controlManager.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            controlManager.Update(gameTime);
            commandManager.Update(gameTime);
        }

        public void RegisterExitButtonEvent(EventHandler evnt)
        {
            //exitGameButton.Selected += evnt;
            simpleExitGameButton.Selected += evnt;
        }

        public void RegisterSaveGameButtonEvent(EventHandler evnt)
        {
            simpleSaveGameButton.Selected += evnt;
        }

        public void RegisterLoadGameButtonEvent(EventHandler evnt)
        {
            simpleLoadGameButton.Selected += evnt;
        }

        public void RegisterResumeButtonEvent(EventHandler evnt)
        {
            //resumeGameButton.Selected += evnt;
            simpleResumeGameButton.Selected += evnt;
        }

        private void LayOutButtons()
        {
            Vector2 buttonPosition = this.position + buttonOffset;

            //TODO: paliktitik vieną
            /*foreach (AnimatedButton button in buttons)
            {
                button.Position = buttonPosition;
                buttonPosition.Y += button.Size.Y + SPACE_BETWEEN_BUTTONS;
            }*/

            foreach (SimpleButton button in simpleButtons)
            {
                button.Position = buttonPosition;
                buttonPosition.Y += button.Size.Y + SPACE_BETWEEN_BUTTONS;
            }
        }
    }
}
