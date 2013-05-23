using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace XRPGLibrary
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract partial class GameState : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Field Region

        private List<GameComponent> childComponents;
        private GameState tag;
       
        protected GameStateManager stateManager;

        #endregion

        #region Property Region

        public List<GameComponent> ChildComponents 
        {
            get { return this.childComponents; }
        }

        public GameState Tag
        {
            get { return this.tag; } 
        }

        #endregion

        #region Constructor Region

        public GameState(Game game, GameStateManager stateManager)
            : base(game)
        {
            this.stateManager = stateManager;
            childComponents = new List<GameComponent>();
            tag = this;
        }

        #endregion

        #region XNA Method Region

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent component in childComponents)
            {
                if (component.Enabled)
                {
                    component.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameComponent component in childComponents)
            {
                if (component is DrawableGameComponent)
                {
                    DrawableGameComponent drawableComponent = component as DrawableGameComponent;

                    if (drawableComponent.Visible)
                    {
                        drawableComponent.Draw(gameTime);
                    }
                }
            }

            base.Draw(gameTime);
        }

        #endregion

        #region Method Region

        internal protected virtual void StateChange(object sender, EventArgs e)
        {
            if (stateManager.CurrentGameState == Tag)
                Show();
            else
                Hide();
        }

        protected virtual void Show()
        {
            Visible = true;
            Enabled = true;

            foreach (GameComponent component in childComponents)
            {
                component.Enabled = true;

                if (component is DrawableGameComponent)
                {
                    ((DrawableGameComponent)component).Visible = true;
                }
            }
        }

        protected virtual void Hide()
        {
            Visible = false;
            Enabled = false;

            foreach (GameComponent component in childComponents)
            {
                component.Enabled = false;

                if (component is DrawableGameComponent)
                {
                    ((DrawableGameComponent)component).Visible = false;
                }
            }
        }

        public void AddChildComponent(GameComponent component)
        {
            childComponents.Add(component);
        }

        public void RemoveChildComponent(GameComponent component)
        {
            childComponents.Remove(component);
        }

        #endregion
    }
}
