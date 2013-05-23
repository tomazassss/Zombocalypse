using System;
using System.Collections.Generic;
using System.Text;

namespace XRpgLibrary.DialogueSystem
{
	public class DialogueInfo : ICloneable
	{
		private int dialogueId;
		private int npcId;

		public int DialogueId
		{
			get{ return dialogueId; }
			set{ this.dialogueId = value; }
		}

		public int NpcId
		{
			get{ return npcId; }
			set{ this.npcId = value; }
		}

        public override bool Equals(object obj)
        {
            if (!(obj is DialogueInfo))
            {
                return false;
            }

            DialogueInfo info = (DialogueInfo)obj;

            return info.dialogueId == dialogueId &&
                   info.npcId == npcId;
        }

        public override int GetHashCode()
        {
            return dialogueId + npcId;
        }

        public override string ToString()
        {
            return string.Format("NPC: {0} ID: {1}", npcId, dialogueId);
        }

        public object Clone()
        {
            DialogueInfo clone = new DialogueInfo();
            clone.npcId = npcId;
            clone.dialogueId = dialogueId;

            return clone;
        }
    }
}
