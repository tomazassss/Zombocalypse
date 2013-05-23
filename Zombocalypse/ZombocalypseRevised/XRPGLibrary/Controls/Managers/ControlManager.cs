using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XRPGLibrary.Controls
{
    public class ControlManager : List<IControl>
    {
        #region Field Region

        private int selectedControl = 0;

        private static SpriteFont spriteFont;

        #endregion

        #region Property Region

        public static SpriteFont SpriteFont
        {
            get { return spriteFont; }
        }

        #endregion

        #region Event Region

        public event EventHandler FocusChanged;

        #endregion

        #region Constructor Region

        public ControlManager(SpriteFont spriteFont)
            : base()
        {
            ControlManager.spriteFont = spriteFont;
        }

        public ControlManager(SpriteFont spriteFont, int capacity)
            : base(capacity)
        {
            ControlManager.spriteFont = spriteFont;
        }

        public ControlManager(SpriteFont spriteFont, IEnumerable<IControl> collection)
            : base(collection)
        {
            ControlManager.spriteFont = spriteFont;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            if (base.Count == 0)
            {
                return;
            }

            foreach (IControl control in this)
            {
                /*
                if (control is Button)
                {
                    Button button = control as Button;
                    if (button.IsMouseInside())
                    {
                        button.HasFocus = true;
                    }
                    else
                    {
                        button.HasFocus = false;
                    }
                }
                */

                if (control.Enabled)
                {
                    control.Update(gameTime);
                }

                if (control.HasFocus)
                {
                    control.HandleInput();
                }

                if (control is InventoryItem)
                {
                    InventoryItem item = control as InventoryItem;
                    if (item.IsBeingDragged)
                    {
                        Vector2 newPosition = new Vector2(InputHandler.MouseState.X - item.Size.X / 2,
                                                          InputHandler.MouseState.Y - item.Size.Y / 2);
                        item.SetPosition(newPosition);
                    }
                }

                /*
                if (control.TrackMouse && (control is InventoryItem))
                {
                    InventoryItem item = control as InventoryItem;
                    item.UpdateMouse();

                    if (item.IsMouseInside &&
                        InputHandler.LeftButtonPressed() &&
                        !item.IsBeingDragged)
                    {
                        if (Drag != null)
                        {
                            Drag(item, null);
                        }
                        item.IsBeingDragged = true;
                    }
                    else if (InputHandler.LeftButtonPressed() &&
                             item.IsBeingDragged)
                    {
                        if (Drop != null)
                        {
                            Drop(item, null);
                        }
                        item.IsBeingDragged = false;
                    }

                    if (item.IsBeingDragged)
                    {
                        Vector2 newPosition = new Vector2(InputHandler.MouseState.X - item.Size.X / 2,
                                                          InputHandler.MouseState.Y - item.Size.Y / 2);
                        item.SetPosition(newPosition);
                    }
                }
                */
            }

            if (InputHandler.KeyPressed(Keys.Up) ||
                InputHandler.KeyPressed(Keys.W))
            {
                PreviousControl();
            }

            if (InputHandler.KeyPressed(Keys.Down) ||
                InputHandler.KeyPressed(Keys.S))
            {
                NextControl();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IControl control in this)
            {
                if (control.Visible)
                {
                    control.Draw(spriteBatch);
                }
            }
        }

        public void NextControl()
        {
            if (base.Count == 0)
            {
                return;
            }

            int currentControl = selectedControl;

            this[selectedControl].HasFocus = false;

            do
            {
                selectedControl++;

                if (selectedControl == base.Count)
                {
                    selectedControl = 0;
                }

                if (this[selectedControl].TabStop && this[selectedControl].Enabled)
                {
                    if (FocusChanged != null)
                    {
                        FocusChanged(this[selectedControl], null);
                    }

                    break;
                }

            } while (currentControl != selectedControl);

            this[selectedControl].HasFocus = true;
        }

        public void PreviousControl()
        {
            if (base.Count == 0)
            {
                return;
            }

            int currentControl = selectedControl;

            this[selectedControl].HasFocus = false;

            do
            {
                selectedControl--;

                if (selectedControl < 0)
                {
                    selectedControl = base.Count - 1;
                }

                if (this[selectedControl].TabStop && this[selectedControl].Enabled)
                {
                    if (FocusChanged != null)
                    {
                        FocusChanged(this[selectedControl], null);
                    }

                    break;
                }

            } while (currentControl != selectedControl);

            this[selectedControl].HasFocus = true;
        }

        #endregion
    }
}
