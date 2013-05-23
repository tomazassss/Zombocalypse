using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRpgLibrary.DialogueSystem;
using Microsoft.Xna.Framework.Content;

namespace XRpgLibrary.MissionSystem
{
    public class MissionInstance : ICloneable
    {
        private MissionNode currentMission;
        private uint missionId;

        public event EventHandler Finished;
        public event EventHandler Changed;

        public uint MissionId
        {
            get { return missionId; }
            set { this.missionId = value; }
        }

        public MissionNode CurrentMission
        {
            get { return currentMission; }
            set
            {
                this.currentMission = value;
                if (currentMission != null)
                {
                    this.currentMission.Success += OnSuccess;
                }
            }
        }

        public void Update(object data)
        {
            currentMission.Update(data);
        }

        private void OnSuccess(object sender, EventArgs args)
        {
            if (sender == null &&
                Finished != null)
            {
                Finished(this, null);
            }

            if (!(sender is MissionNode))
            {
                return;
            }

            if (Changed != null)
            {
                Changed(this, null);
            }

            Console.WriteLine("Mission step {0} finished", currentMission.Title);

            MissionNode newMission = (MissionNode)sender;
            CurrentMission = newMission;
            Console.WriteLine("New mission step: {0}", currentMission.Title);
        }

        public void UnregisterEvents()
        {
            currentMission.Success -= OnSuccess;
        }

        public override string ToString()
        {
            string toString = string.Empty;
            toString += "MissionId: " + missionId + "\n";
            toString += currentMission.ToString();

            return toString;
        }



        public object Clone()
        {
            MissionInstance mission = new MissionInstance();
            mission.currentMission = (MissionNode)currentMission.Clone();
            mission.missionId = missionId;
            if (mission.currentMission != null)
            {
                mission.currentMission.Success += mission.OnSuccess;
            }

            return mission;
        }
    }
}
