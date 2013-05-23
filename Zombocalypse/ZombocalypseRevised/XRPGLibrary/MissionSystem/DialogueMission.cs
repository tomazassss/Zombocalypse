using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRpgLibrary.DialogueSystem;

namespace XRpgLibrary.MissionSystem
{
    public class DialogueMission : MissionNode
    {
        private DialogueInfo dialogueInfo;

        public override event EventHandler Success;
        public override event EventHandler Failure;

        public DialogueInfo DialogueInfo
        {
            get { return dialogueInfo; }
            set { this.dialogueInfo = value; }

        }

        public override void Update(object data)
        {
            if (!(data is DialogueInfo))
            {
                return;
            }

            DialogueInfo info = (DialogueInfo)data;

            if (Success != null &&
                info.Equals(dialogueInfo))
            {
                Success(successNode, null);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(base.ToString());

            builder.Append("DialogueInfo: " + dialogueInfo);

            return builder.ToString();
        }


        public override object Clone()
        {
            DialogueMission clone = new DialogueMission();
            clone.title = title;
            clone.description = description;
            clone.newDialogueOption = newDialogueOption;
            clone.dialogueInfo = (DialogueInfo)dialogueInfo.Clone();

            if (successNode != null)
            {
                clone.successNode = (MissionNode)successNode.Clone();
            }

            if (failureNode != null)
            {
                clone.failureNode = (MissionNode)failureNode.Clone();
            }

            return clone;
        }
    }
}
