using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRPGLibrary.Controls.Items
{
    public sealed class ItemKind
    {
        /// <summary>
        /// Content loader throws an exception if you want to add these in the
        /// constructor (itemKinds.Add(this)). So they are added in the static constructor 
        /// </summary>
        public static readonly ItemKind OFF_HAND; //= new ItemKind("OFF_HAND");
        public static readonly ItemKind MAIN_HAND;// = new ItemKind("MAIN_HAND");
        public static readonly ItemKind HEAD_GEAR;// = new ItemKind("HEAD_GEAR");
        public static readonly ItemKind TORSO;// = new ItemKind("TORSO");
        public static readonly ItemKind LEGGINGS;// = new ItemKind("LEGGINGS");
        public static readonly ItemKind BOOTS;// = new ItemKind("BOOTS");\
        public static readonly ItemKind AMMO;
        public static readonly ItemKind ANY;

        private static List<ItemKind> itemKinds = new List<ItemKind>();

        private string value;
        private string niceValue;

        static ItemKind()
        {
            OFF_HAND = new ItemKind("OFF_HAND", "Off hand");
            MAIN_HAND = new ItemKind("MAIN_HAND", "Main hand");
            HEAD_GEAR = new ItemKind("HEAD_GEAR", "Head gear");
            TORSO = new ItemKind("TORSO", "Torso");
            LEGGINGS = new ItemKind("LEGGINGS", "Leggings");
            BOOTS = new ItemKind("BOOTS", "Boots");
            AMMO = new ItemKind("AMMO", "Ammo");
            ANY = new ItemKind("ANY", "Any");
            itemKinds.Add(OFF_HAND);
            itemKinds.Add(MAIN_HAND);
            itemKinds.Add(HEAD_GEAR);
            itemKinds.Add(TORSO);
            itemKinds.Add(LEGGINGS);
            itemKinds.Add(BOOTS);
            itemKinds.Add(AMMO);
            itemKinds.Add(ANY);
        }

        public ItemKind()
        {

        }

        private ItemKind(string value, string niceValue)
        {
            this.value = value;
            this.niceValue = niceValue;
            //itemKinds.Add(this);
        }

        public string Value
        {
            get { return this.value; }
        }

        public string NiceValue
        {
            get { return this.niceValue; }
        }

        public static ItemKind FromString(string kindString)
        {
            foreach (ItemKind itemKind in itemKinds)
            {
                if (itemKind.Value == kindString)
                {
                    return itemKind;
                }
            }
            throw new NoSuchItemKindException("Item kind " + kindString + " does not exist");
        }
    }

    [Serializable]
    public sealed class NoSuchItemKindException : Exception
    {
        public NoSuchItemKindException()
            : base()
        {
        }

        public NoSuchItemKindException(string message)
            : base(message)
        {
        }
    }
}
