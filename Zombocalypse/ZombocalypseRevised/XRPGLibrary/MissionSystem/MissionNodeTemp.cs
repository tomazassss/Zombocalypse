using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRpgLibrary.MissionSystem
{
    //TODO: Laikina/bandomoji struktūra. Reiks arba ją pildyt, arba ją ištrint sukurs naują/pilną
    public struct MissionNodeTemp
    {
        private string title;
        private string shortTitle;
        private string description;

        private bool useShortTitle;

        public string Title
        {
            get { return title; }
            set { this.title = value; }
        }

        public string Description
        {
            get { return description; }
            set { this.description = value; }
        }

        public string ShortTitle
        {
            get { return shortTitle; }
            set { this.shortTitle = value; }
        }

        public bool UseShortTitle
        {
            get { return useShortTitle; }
            set { this.useShortTitle = value; }
        }
    }
}
