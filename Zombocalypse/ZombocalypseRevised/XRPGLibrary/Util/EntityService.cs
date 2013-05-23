using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRpgLibrary.DialogueSystem;

namespace XRPGLibrary.Util
{
    public class EntityService
    {
        private static EntityService instance;

        private List<IChattable> npcs;

        public List<IChattable> Npcs
        {
            get { return npcs; }
            set { this.npcs = value; }
        }

        static EntityService()
        {
            instance = new EntityService();
        }

        private EntityService() { }

        public static EntityService GetInstance()
        {
            return instance;
        }

        public IChattable GetNpcById(int id)
        {
            if (npcs != null)
            {
                foreach (IChattable npc in npcs)
                {
                    if (npc.Id == id)
                    {
                        return npc;
                    }
                }
            }
            return null;
        }
    }
}
