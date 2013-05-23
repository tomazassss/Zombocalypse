using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using XRpgLibrary.DialogueSystem;

namespace XRpgLibrary.MissionSystem
{
    public abstract class MissionNode : ICloneable
    {
        protected MissionNode successNode;
        protected MissionNode failureNode;

        protected String title;
        protected String description;

        protected String shortTitle;
        protected bool useShortTitle;

        protected DialogueInfo newDialogueOption;

        public abstract event EventHandler Success;
        public abstract event EventHandler Failure;

        public String Title
        {
            get { return title; }
            set { this.title = value; }
        }

        public String Description
        {
            get { return description; }
            set { this.description = value; }
        }

        [ContentSerializerIgnore]
        public String ShortTitle
        {
            get { return shortTitle; }
            set { this.shortTitle = value; }
        }

        [ContentSerializerIgnore]
        public bool UseShortTitle
        {
            get { return useShortTitle; }
            set { this.useShortTitle = value; }
        }

        [ContentSerializer(Optional=true)]
        public MissionNode SuccessNode
        {
            get { return successNode; }
            set { this.successNode = value; }
        }

        [ContentSerializer(Optional = true)]
        public MissionNode FailureNode
        {
            get { return failureNode; }
            set { this.failureNode = value; }
        }

        [ContentSerializer(Optional=true)]
        public DialogueInfo NewDialogueOption
        {
            get { return newDialogueOption; }
            set { this.newDialogueOption = value; }
        }

        public abstract void Update(object data);

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Title: " + title + "\n");
            builder.Append("Description: " + description + "\n");

            if(successNode != null)
            {
                builder.Append("\nSuccesNode:\n" + successNode + "\n");
            }

            if (failureNode != null)
            {
                builder.Append("\nFailureNode:\n" + failureNode + "\n");
            }

            return builder.ToString();
        }

        public abstract object Clone();
    }
}
