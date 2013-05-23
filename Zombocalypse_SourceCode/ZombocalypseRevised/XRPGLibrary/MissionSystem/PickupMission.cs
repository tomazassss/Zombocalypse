using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRpgLibrary.MissionSystem
{
    public class PickupMission : MissionNode
    {
        private uint pickupCount;
        private uint itemId;

        public override event EventHandler Success;
        public override event EventHandler Failure;

        public uint PickupCount
        {
            get { return pickupCount; }
            set { this.pickupCount = value; }
        }

        public uint ItemId
        {
            get { return itemId; }
            set { this.itemId = value; }
        }

        public override void Update(object data)
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(base.ToString());

            builder.Append("PickupCount: " + pickupCount + "\n");
            builder.Append("ItemId: " + itemId + "\n");

            return builder.ToString();
        }

        public override object Clone()
        {
            PickupMission clone = new PickupMission();
            clone.title = title;
            clone.description = description;
            clone.newDialogueOption = newDialogueOption;
            clone.pickupCount = pickupCount;
            clone.itemId = itemId;

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
