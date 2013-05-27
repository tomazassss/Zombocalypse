using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRpgLibrary.MissionSystem
{
    public class EliminationMission : MissionNode
    {
        private uint eliminationCount;
        private uint currentlyEliminated;

        private int targetId;

        public override event EventHandler Success;
        public override event EventHandler Failure;

        public uint EliminationCount
        {
            get { return eliminationCount; }
            set { this.eliminationCount = value; }
        }
        
        public int TargetId
        {
            get { return targetId; }
            set { this.targetId = value; }
        }

        public EliminationMission()
        {
            this.currentlyEliminated = 0;
        }

        public override void Update(object data)
        {
            if (!(data is int))
            {
                return;
            }

            int eliminatedId = (int)data;

            if (eliminatedId == targetId)
            {
                currentlyEliminated++;

                if (Success != null && 
                    currentlyEliminated >= eliminationCount)
                {
                    Success(successNode, null);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(base.ToString());

            builder.Append("EliminationCount: " + eliminationCount + "\n");
            builder.Append("TargetId: " + targetId + "\n");

            return builder.ToString();
        }

        public override object Clone()
        {
            EliminationMission clone = new EliminationMission();
            clone.description = description;
            clone.title = title;
            clone.newDialogueOption = newDialogueOption;
            clone.eliminationCount = eliminationCount;
            clone.targetId = targetId;

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
