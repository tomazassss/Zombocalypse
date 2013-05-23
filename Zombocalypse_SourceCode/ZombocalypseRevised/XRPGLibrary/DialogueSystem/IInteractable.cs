using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombocalypseRevised.DialogueSystem
{
    public interface IInteractable
    {
        event EventHandler Interacted;

        bool canInteract();
    }
}
