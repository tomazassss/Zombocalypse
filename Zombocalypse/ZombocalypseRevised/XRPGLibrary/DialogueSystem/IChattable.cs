using System;
using System.Collections.Generic;
using System.Text;

namespace XRpgLibrary.DialogueSystem
{
	public interface IChattable
	{
		List<DialogueOption> Dialogues
		{
			get;
			set;
		}

		string Name
		{
			get;
			set;
		}

        int Id
        {
            get;
            set;
        }
		
		void AddDialogue(DialogueOption option);
		void RemoveDialogue(DialogueOption option);
	}
}
