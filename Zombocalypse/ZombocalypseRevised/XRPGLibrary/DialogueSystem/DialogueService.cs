using System;
using System.Collections.Generic;
using System.Text;

namespace XRpgLibrary.DialogueSystem
{
	public class DialogueService
	{
        private static DialogueService instance;

		private HashMap<DialogueInfo, DialogueOption> allDialogues;

		public HashMap<DialogueInfo, DialogueOption> AllDialogues
		{
			get{ return allDialogues;}
			set{ this.allDialogues = value;}
		}

        static DialogueService()
        {
            instance = new DialogueService();
        }

        private DialogueService() { }

        public static DialogueService GetInstance()
        {
            return instance;
        }

		public DialogueOption GetDialogue(DialogueInfo dialogueInfo)
		{
			foreach (KeyValuePair<DialogueInfo, DialogueOption> item in allDialogues)
			{
				if (dialogueInfo.Equals(item.Key))
				{
					return item.Value;
				}
			}
            return null;
		}
	}
}
