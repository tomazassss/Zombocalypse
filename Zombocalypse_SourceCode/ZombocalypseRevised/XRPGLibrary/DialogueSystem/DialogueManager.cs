using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XRPGLibrary.Util;

namespace XRpgLibrary.DialogueSystem
{
	public class DialogueManager
	{
        private static DialogueManager instance;

		private Queue<DialogueInfo> selectedDialogues;
        private bool hasChanged;

        public event EventHandler DialogueSelected;

        public bool HasChanged
        {
            get { return hasChanged; }
            set { this.hasChanged = value; }
        }

        static DialogueManager()
        {
            instance = new DialogueManager();
        }

        private DialogueManager() 
        {
            selectedDialogues = new Queue<DialogueInfo>();
        }

        public static DialogueManager GetInstance()
        {
            return instance;
        }

		public void OnSelected(object sender, EventArgs args)
		{
			if(!(sender is DialogueInfo))
			{
				return;
			}
			
			DialogueInfo dialogueInfo = (DialogueInfo)sender;
			selectedDialogues.Enqueue(dialogueInfo);
		}

		public void Update(GameTime gameTime)
		{
			while(selectedDialogues.Count > 0)
			{
				DialogueInfo info = selectedDialogues.Dequeue();
                DialogueOption dialogue = DialogueService.GetInstance().GetDialogue(info);
                dialogue.Update(gameTime);
                hasChanged = true;

                if (DialogueSelected != null)
                {
                    DialogueSelected(info, null);
                }
			}
		}

        public void AssignNewDialogue(DialogueInfo info)
        {
            DialogueOption dialogue = DialogueService.GetInstance().GetDialogue(info);
            IChattable npc = EntityService.GetInstance().GetNpcById(info.NpcId);

            npc.AddDialogue(dialogue);
            hasChanged = true;
        }
	}
}
