using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRpgLibrary.DialogueSystem
{
    public class MockNpc : IChattable
    {
        private List<DialogueOption> dialogues;
        private string name;

        public List<DialogueOption> Dialogues
        {
            get
            {
                return dialogues;
            }
            set
            {
                this.dialogues = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }

        public MockNpc(string name)
        {
            this.dialogues = new List<DialogueOption>();
            this.name = name;
        }

        public void AddDialogue(DialogueOption option)
        {
            dialogues.Add(option);
            option.AttachChattable(this);
        }

        public void RemoveDialogue(DialogueOption option)
        {
            dialogues.Remove(option);
        }


        public int Id
        {
            get { return 0; }
            set { }
        }
    }
}
