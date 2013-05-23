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
    public class InputHandler : Microsoft.Xna.Framework.GameComponent
    {
        #region Field Region

        private static KeyboardState keyboardState;
        private static KeyboardState lastKeyboardState;

        private static ButtonState leftMouseButtonState;
        private static ButtonState lastLeftMouseButtonState;
        private static ButtonState rightMouseButtonState;
        private static ButtonState lastRightMouseButtonState;

        private static MouseState mouseState;
        private static MouseState lastMouseState;

        private static int mouseX;
        private static int mouseY;

        #endregion

        #region Property Region

        public static KeyboardState KeyboardState
        {
            get { return keyboardState; }
        }

        public static KeyboardState LastKeyboardState
        {
            get { return lastKeyboardState; }
        }

        public static ButtonState LeftMouseButtonState
        {
            get { return leftMouseButtonState; }
        }

        public static ButtonState LastLeftMouseButtonState
        {
            get { return lastLeftMouseButtonState; }
        }

        public static MouseState MouseState
        {
            get { return mouseState; }
        }

        public static MouseState LastMouseState
        {
            get { return lastMouseState; }
        }

        #endregion

        #region Constructor Region

        public InputHandler(Game game)
            : base(game)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            leftMouseButtonState = mouseState.LeftButton;
            rightMouseButtonState = mouseState.RightButton;
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
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            lastMouseState = mouseState;
            mouseState = Mouse.GetState();

            lastLeftMouseButtonState = leftMouseButtonState;
            leftMouseButtonState = mouseState.LeftButton;

            lastRightMouseButtonState = rightMouseButtonState;
            rightMouseButtonState = mouseState.RightButton;

            base.Update(gameTime);
        }

        #endregion

        #region General Method Region

        public static void Flush()
        {
            lastKeyboardState = keyboardState;
        }

        #endregion

        #region Keyboard Region

        public static bool KeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) &&
                   lastKeyboardState.IsKeyDown(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) &&
                   lastKeyboardState.IsKeyUp(key);
        }

        public static bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        #endregion

        #region Mouse Region
        
        public static int GetDegrees(Vector2 position, Vector2 camera)
        {
            int degrees = 0;
            MouseState currentMouse = Mouse.GetState();

            mouseX = currentMouse.X + (int)camera.X;
            mouseY = currentMouse.Y + (int)camera.Y;

            float dx = mouseX - position.X;
            float dy = mouseY - position.Y;

            degrees = Convert.ToInt32(Math.Atan2(dy, dx) * (180 / Math.PI));
            if (degrees < 0) 
            {
                degrees += 360; 
            }
            return degrees;
        }

        public static bool LeftButtonPressed()
        {
            return leftMouseButtonState == ButtonState.Pressed &&
                   lastLeftMouseButtonState == ButtonState.Released;
        }

        public static bool LeftButtonReleased()
        {
            return leftMouseButtonState == ButtonState.Released &&
                   lastLeftMouseButtonState == ButtonState.Pressed;
        }

        public static bool LeftButtonDown()
        {
            return leftMouseButtonState == ButtonState.Pressed;
        }

        public static bool RightButtonPressed()
        {
            return rightMouseButtonState == ButtonState.Pressed &&
                   lastRightMouseButtonState == ButtonState.Released;
        }

        public static bool RightButtonReleased()
        {
            return rightMouseButtonState == ButtonState.Released &&
                   lastRightMouseButtonState == ButtonState.Pressed;
        }

        public static bool RightButtonDown()
        {
            return rightMouseButtonState == ButtonState.Pressed;
        }

        #endregion
    }
}
