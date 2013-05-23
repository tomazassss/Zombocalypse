using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRPGLibrary.Controls.Items
{
    public enum StatType
    {
        MIN_DAMAGE,
        MAX_DAMAGE,
        ATTACK_SPEED,
        RELOAD_SPEED,
        CLIP_SIZE,
        ARMOR,
        DAMAGE_BLOCK,
        IS_STACKABLE,
        MAX_STACK
    }

    public class ItemStat
    {
        private StatType statType;
        private float val;

        public StatType StatType
        {
            get { return statType; }
            set { this.statType = value; }
        }

        public float Val
        {
            get { return val; }
            set { this.val = value; }
        }

        public override string ToString()
        {
            String toString = String.Format("{0}: {1}", statType, val);
            return toString;
        }
    }
}
