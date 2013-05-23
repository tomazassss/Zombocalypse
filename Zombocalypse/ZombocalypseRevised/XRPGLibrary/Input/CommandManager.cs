using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace XRPGLibrary.Input
{
    public class CommandManager : List<Command>
    {
        private bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { this.enabled = value; }
        }

        public CommandManager()
            : base()
        {
            this.enabled = true;
        }

        public CommandManager(int capacity)
            : base(capacity)
        {
            this.enabled = true;
        }

        public CommandManager(IEnumerable<Command> collection)
            : base(collection)
        {
            this.enabled = true;
        }

        public void RegisterCommand(Keys key, Action action)
        {
            RegisterCommand(key, action, ClickState.RELEASED);
        }

        public void RegisterCommand(Keys key, Action action, ClickState clickState)
        {
            if (Contains(key))
            {
                return;
            }

            Command command = new Command();
            command.RegisterAction(key, action, clickState);
            this.Add(command);
        }

        public void UnregisterCommand(Keys key)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Key == key)
                {
                    this.RemoveAt(i);
                    break;
                }
            }
        }

        private bool Contains(Keys key)
        {
            foreach (Command command in this)
            {
                if (command.Key == key)
                {
                    return true;
                }
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            if (enabled)
            {
                foreach (Command command in this)
                {
                    switch (command.ClickState)
                    {
                        case ClickState.DOWN:
                            if (InputHandler.KeyDown(command.Key))
                            {
                                command.RegisteredAction();
                            }
                            break;

                        case ClickState.PRESSED:
                            if (InputHandler.KeyPressed(command.Key))
                            {
                                command.RegisteredAction();
                            }
                            break;

                        case ClickState.RELEASED:
                            if (InputHandler.KeyReleased(command.Key))
                            {
                                command.RegisteredAction();
                            }
                            break;

                        default:
                            break;
                    }
                }
                //InputHandler.Flush();
            }
        }
    }
}
