using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRpgLibrary.DialogueSystem
{
    public class AllDialogues
    {
        private HashMap<DialogueInfo, DialogueOption> dialogues;

        public HashMap<DialogueInfo, DialogueOption> Dialogues
        {
            get { return dialogues; }
            set { this.dialogues = value; }
        }
    }
}
