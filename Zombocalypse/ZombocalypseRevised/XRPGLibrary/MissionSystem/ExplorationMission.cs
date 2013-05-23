using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XRpgLibrary.MissionSystem
{
    public class ExplorationMission : MissionNode
    {
        private Point targetArea;
        private Point areaWidthHeight;
        private uint mapId;

        public override event EventHandler Success;
        public override event EventHandler Failure;

        public Point TargetArea
        {
            get { return targetArea; }
            set { this.targetArea = value; }
        }

        public Point AreaWidthHeight
        {
            get { return areaWidthHeight; }
            set { this.areaWidthHeight = value; }
        }
        
        public uint MapId
        {
            get { return mapId; }
            set { this.mapId = value; }
        }

        public override void Update(object data)
        {
            if (!(data is Point))
            {
                return;
            }

            Point playerPosition = (Point)data;

            if (Success != null &&
                playerPosition.X >= targetArea.X &&
                playerPosition.Y >= targetArea.Y &&
                playerPosition.X <= targetArea.X + areaWidthHeight.X &&
                playerPosition.Y <= targetArea.Y + areaWidthHeight.Y)
            {
                Success(SuccessNode, null);
            }

        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(base.ToString());

            builder.Append("TargetArea: " + targetArea + "\n");
            builder.Append("AreaWidthHeight: " + areaWidthHeight + "\n");
            builder.Append("MapId: " + mapId + "\n");

            return builder.ToString();
        }

        public override object Clone()
        {
            ExplorationMission clone = new ExplorationMission();
            clone.title = title;
            clone.description = description;
            clone.newDialogueOption = newDialogueOption;
            clone.targetArea = targetArea;
            clone.areaWidthHeight = areaWidthHeight;
            clone.mapId = mapId;

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
