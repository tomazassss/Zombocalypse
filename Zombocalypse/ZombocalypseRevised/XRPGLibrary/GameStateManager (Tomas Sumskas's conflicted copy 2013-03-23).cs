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
    public class GameStateManager : Microsoft.Xna.Framework.GameComponent
    {
        #region Event Region

        public event EventHandler OnStateChange;

        #endregion

        #region Field Region

        private Stack<GameState> gameStates;

        private const int START_DRAW_ORDER = 5000;
        private const int DRAW_ORDER_INC = 100;

        private int drawOrder;

        #endregion

        #region Property Region

        public GameState CurrentGameState 
        {
            get { return gameStates.Peek(); }
        }

        #endregion

        #region Constructor Region

        public GameStateManager(Game game)
            : base(game)
        {
            gameStates = new Stack<GameState>();
            drawOrder = START_DRAW_ORDER;
        }

        #endregion

        #region XNA Method Region

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        #endregion

        #region Method Region

        public void PopState()
        {
            if (gameStates.Count > 0)
            {
                RemoveState();
                drawOrder -= DRAW_ORDER_INC;

                if (OnStateChange != null)
                {
                    OnStateChange(this, null);
                }
            }
        }

        private void RemoveState()
        {
            GameState state = gameStates.Peek();
            OnStateChange -= state.StateChange;
            Game.Components.Remove(state);
            gameStates.Pop();
        }

        public void PushState(GameState newState)
        {
            drawOrder += DRAW_ORDER_INC;
            newState.DrawOrder = drawOrder;

            AddState(newState);

            if (OnStateChange != null)
            {
                OnStateChange(this, null);
            }
        }

        private void AddState(GameState newState)
        {
            gameStates.Push(newState);
            Game.Components.Add(newState);
            OnStateChange += newState.StateChange;
        }

        public void ChangeState(GameState newState)
        {
            while (gameStates.Count > 0)
            {
                RemoveState();
            }

            newState.DrawOrder = START_DRAW_ORDER;
            drawOrder = START_DRAW_ORDER;

            AddState(newState);

            if (OnStateChange != null)
            {
                OnStateChange(this, null);
            }
        }

        #endregion

    }
}
