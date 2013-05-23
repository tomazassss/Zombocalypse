using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombocalypseRevised.Components
{
    public class Stats
    {
        private float damage;
        private float armor;

        public float Damage
        {
            get { return damage; }
            set { this.damage = value; }
        }

        public float Armor
        {
            get { return armor; }
            set { this.armor = value; }
        }

        public Stats()
        {
            this.damage = 0;
            this.armor = 0;
        }

        public Stats(float damage, float armor)
        {
            this.damage = damage;
            this.armor = armor;
        }

        public override string ToString()
        {
            return string.Format("Damage: {0}, armor: {1}", damage, armor);
        }
    }
}
