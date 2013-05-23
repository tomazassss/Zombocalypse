using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace XRPGLibrary.Input
{
    public enum ClickState
    {
        DOWN,
        PRESSED,
        RELEASED
    }

    public struct Command
    {
        private ClickState clickState;
        private Action registeredAction;
        private Keys key;

        public ClickState ClickState
        {
            get { return clickState; }
        }

        public Action RegisteredAction
        {
            get { return registeredAction; }
        }

        public Keys Key
        {
            get { return key; }
        }

        public void RegisterAction(Keys key, Action action, ClickState clickState)
        {
            this.key = key;
            this.registeredAction = action;
            this.clickState = clickState;
        }

        public override string ToString()
        {
            return string.Format("Key: {0}, Action: {1}, ClickState: {2}", key, registeredAction, clickState);
        }
    }
}
