using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRpgLibrary.DialogueSystem;

namespace XRpgLibrary.DialogueSystem
{
    public class NewMissionData : DialogueData
    {
        private uint missionId;

        public uint MissionId
        {
            get { return missionId; }
            set { this.missionId = value; }
        }
    }
}
