using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XRPGLibrary;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.Controls;
using XRPGLibrary.Util;

namespace ZombocalypseRevised.GameScreens
{
    public class TitleScreen : BaseGameState
    {
        #region Field Region

        private Texture2D backgroundImage;
        private LinkLabel startLabel;

        #endregion

        #region Constructor Region

        public TitleScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
        }

        #endregion

        #region XNA Method Region

        protected override void LoadContent()
        {
            ContentManager content = gameRef.Content;

            backgroundImage = content.Load<Texture2D>(@"Backgrounds\tomo_titlescreen");

            base.LoadContent();

            startLabel = new LinkLabel();
            startLabel.Position = new Vector2(350, 600);
            startLabel.Text = "Press ENTER to begin";
            startLabel.Color = Color.White;
            startLabel.TabStop = true;
            startLabel.HasFocus = true;
            startLabel.Selected += startLabelOnClick;

            controlManager.Add(startLabel);

            controlManager.NextControl();
        }

        public override void Update(GameTime gameTime)
        {
            controlManager.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawingUtils.SpriteBatchBegin(gameRef.SpriteBatch);
            
            base.Draw(gameTime);

            gameRef.SpriteBatch.Draw(
                backgroundImage,
                gameRef.ScreenRectangle,
                Color.White);

            controlManager.Draw(gameRef.SpriteBatch);

            DrawingUtils.SpriteBatchEnd(gameRef.SpriteBatch);
        }

        #endregion

        #region Title Screen Method Region

        private void startLabelOnClick(object sender, EventArgs e)
        {
            stateManager.PushState(gameRef.StartMenuScreen);
        }

        #endregion

    }
}
