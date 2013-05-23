using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XRpgLibrary.DialogueSystem;
using XRPGLibrary.Util;
using ZombocalypseRevised.HUD;

namespace XRpgLibrary.MissionSystem
{
    public class MissionManager
    {
        private static MissionManager instance;

        private List<MissionInstance> currentMissions;
        private Queue<Point> explorationMissionData;
        private Queue<DialogueInfo> dialogueMissionData;
        private Queue<uint> missionsToRemove;
        private bool hasChanged;

        public List<MissionInstance> CurrentMissions
        {
            get { return currentMissions; }
        }

        public bool HasChanged
        {
            get { return hasChanged; }
            set { this.hasChanged = value; }
        }

        private MissionManager()
        {
            currentMissions = new List<MissionInstance>();
            explorationMissionData = new Queue<Point>();
            dialogueMissionData = new Queue<DialogueInfo>();
            missionsToRemove = new Queue<uint>();
        }

        public static MissionManager GetInstance()
        {
            if (instance == null)
            {
                instance = new MissionManager();
            }
            return instance;
        }

        public void AddMission(MissionInstance mission)
        {
            currentMissions.Add(mission);
            mission.Finished += OnMissionFinished;
            mission.Changed += OnMissionChange;
            hasChanged = true;
        }

        public void RemoveMission(uint missionId)
        {
            for (int i = 0; i < currentMissions.Count; i++)
            {
                if (currentMissions[i].MissionId == missionId)
                {
                    currentMissions.RemoveAt(i);
                    hasChanged = true;
                    break;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            //TODO: sugalvoti, kaip atskirti kokiai misijai siusti informacija
            while (explorationMissionData.Count > 0)
            {
                Point data = explorationMissionData.Dequeue();
                foreach (MissionInstance mission in currentMissions)
                {
                    if (mission.CurrentMission is ExplorationMission)
                    {
                        mission.Update(data);
                    }
                }
            }

            while (dialogueMissionData.Count > 0)
            {
                DialogueInfo data = dialogueMissionData.Dequeue();
                foreach (MissionInstance mission in currentMissions)
                {
                    if (mission.CurrentMission is DialogueMission)
                    {
                        mission.Update(data);
                    }
                }
            }

            while (missionsToRemove.Count > 0)
            {
                uint missionId = missionsToRemove.Dequeue();
                RemoveMission(missionId);
            }
        }

        public void OnMissionFinished(object sender, EventArgs args)
        {
            if (!(sender is MissionInstance))
            {
                return;
            }

            MissionInstance finishedMission = (MissionInstance)sender;

            Console.WriteLine("Mission {0} finished", finishedMission.CurrentMission.Title);
            NotificationPopup.GetInstance().AddNotification(string.Format("Mission {0} finished", finishedMission.CurrentMission.Title));

            //TODO: Pazymeti misija kaip pabaigta, kad negaletu vel pridet
            missionsToRemove.Enqueue(finishedMission.MissionId);
        }

        public void OnUpdateEvent(object sender, EventArgs args)
        {
            if (sender != null)
            {
                if (sender is Point)
                {
                    Point position = (Point)sender;
                    explorationMissionData.Enqueue(position);
                }
                else if (sender is DialogueInfo)
                {
                    DialogueInfo info = (DialogueInfo)sender;
                    dialogueMissionData.Enqueue(info);
                }
            }
        }

        public void OnMissionChange(object sender, EventArgs args)
        {
            if (!(sender is MissionInstance))
            {
                return;
            }

            MissionInstance mission = (MissionInstance)sender;
            DialogueInfo dialogueInfo = mission.CurrentMission.NewDialogueOption;
            if (dialogueInfo != null)
            {
                IChattable npc = EntityService.GetInstance().GetNpcById(dialogueInfo.NpcId);
                DialogueManager.GetInstance().AssignNewDialogue(dialogueInfo);
            }

            NotificationPopup.GetInstance().AddNotification(string.Format("Mission log updated"));
            hasChanged = true;
        }

        public void Clear()
        {
            foreach (MissionInstance mission in currentMissions)
            {
                mission.UnregisterEvents();
            }
            currentMissions.Clear();
            explorationMissionData.Clear();
            hasChanged = true;
        }
    }
}
