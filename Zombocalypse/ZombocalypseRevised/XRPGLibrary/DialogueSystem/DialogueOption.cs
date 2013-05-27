using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XRPGLibrary.Util;
using Microsoft.Xna.Framework.Content;

namespace XRpgLibrary.DialogueSystem
{
	public class DialogueOption
	{
		private string chatOption;
		private string response;
		private List<DialogueInfo> successorInfo;
		private DialogueInfo dialogueInfo;
		private IChattable chattable;
        private bool isRepeatable;
        private DialogueData additionalData;

		public event EventHandler Selected;
        public event EventHandler ProccessAdditionalData;

		public string ChatOption
		{
			get{ return chatOption; }
			set{ this.chatOption = value; }
		}

		public string Response
		{
			get{ return response; }
			set{ this.response = value; }
		}

		public DialogueInfo DialogueInfo
		{
			get{ return dialogueInfo; }
			set{ this.dialogueInfo = value; }
		}
		
		public List<DialogueInfo> SuccessorInfo
		{
			get { return successorInfo; }
			set { this.successorInfo = value; }
		}

        public bool IsRepeatable
        {
            get { return isRepeatable; }
            set { this.isRepeatable = value; }
        }

        [ContentSerializer(Optional=true)]
        public DialogueData AdditionalData
        {
            get { return additionalData; }
            set { this.additionalData = value; }
        }
		
		public void AttachChattable(IChattable chattable)
		{
			this.chattable = chattable;
		}

		public void Update(GameTime gameTime)
		{
			if(chattable != null)
			{
				chattable.RemoveDialogue(this);
				
				foreach(DialogueInfo info in successorInfo)
				{
					DialogueOption option = DialogueService.GetInstance().GetDialogue(info);
                    IChattable npc = EntityService.GetInstance().GetNpcById(info.NpcId);
					npc.AddDialogue(option);
				}
			}
			
			if(Selected != null)
			{
				Selected(dialogueInfo, null);
			}

            if (ProccessAdditionalData != null && additionalData != null)
            {
                ProccessAdditionalData(additionalData, null);
            }
		}

        public override bool Equals(object obj)
        {
            if (!(obj is DialogueOption))
            {
                return false;
            }

            DialogueOption option = (DialogueOption)obj;

            return option.dialogueInfo.Equals(dialogueInfo);
        }

        public override int GetHashCode()
        {
            return dialogueInfo.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Option: {0}\nResponse: {1}", chatOption, response);
        }
	}
}
