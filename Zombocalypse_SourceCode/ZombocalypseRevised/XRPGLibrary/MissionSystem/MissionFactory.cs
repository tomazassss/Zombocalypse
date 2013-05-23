using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRpgLibrary.MissionSystem
{
    public class MissionFactory
    {
        private List<MissionInstance> allMissions;

        public List<MissionInstance> AllMissions
        {
            get { return allMissions; }
            set { this.allMissions = value; }
        }

        public MissionInstance GetMissionById(uint missionId)
        {
            foreach (MissionInstance mission in allMissions)
            {
                if (mission.MissionId == missionId)
                {
                    return (MissionInstance)mission.Clone();
                }
            }
            return null;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (MissionInstance mission in allMissions)
            {
                builder.Append(mission);
            }

            return builder.ToString();
        }
    }
}
