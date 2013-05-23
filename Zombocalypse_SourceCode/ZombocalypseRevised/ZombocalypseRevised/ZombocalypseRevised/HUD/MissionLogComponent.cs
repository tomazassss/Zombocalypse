using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.Controls;
using Microsoft.Xna.Framework.Content;
using ZombocalypseRevised.HUD.Controls;
using XRpgLibrary.MissionSystem;

namespace ZombocalypseRevised.HUD
{
    public class MissionLogComponent : AHUDComponent
    {
        private TextArea textArea;
        private ItemList itemList;

        public override Vector2 Size
        {
            get { return size; }
            set
            {
                this.size = value;
                itemList.Size = new Vector2(value.X / 3, value.Y);
                textArea.Size = new Vector2(value.X * (2 / 3f), value.Y);
            }
        }

        public override Vector2 Position
        {
            get { return position; }
            set
            {
                this.position = value;
                itemList.Position = value;
                textArea.Position = new Vector2(value.X + itemList.Size.X, value.Y);
            }
        }

        public override bool Enabled
        {
            set
            {
                enabled = value;
                if (!enabled)
                {
                    Hide();
                }
            }
        }

        public MissionLogComponent(SpriteFont font, ContentManager content)
            : base(font, content)
        {
            itemList = new ItemList(font);
            itemList.SelectionChange += OnListSelectionChange;

            //TODO: hardcoded missions
            /*
            MissionNodeTemp mission = new MissionNodeTemp();
            mission.Title = "Find the skunk and kill it (don't get sprayed)";
            mission.Description = "The title says it all";

            MissionListItem listItem = new MissionListItem(mission);
            itemList.AddItem(listItem);

            mission = new MissionNodeTemp();
            mission.Title = "Restrain the cookie monster (he's nuts)";
            mission.Description = "Cookie monster has gone wild. You have to catch him before all the cookies are consumed." +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n" +
                "test test test test test test test test test test test test test test\n";
            listItem = new MissionListItem(mission);
            itemList.AddItem(listItem);
            */

            textArea = new TextArea(font, content);
        }

        public override void LoadContent()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            textArea.Draw(spriteBatch);
            itemList.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            textArea.Update(gameTime);
            itemList.Update(gameTime);
            commandManager.Update(gameTime);
            if (MissionManager.GetInstance().HasChanged)
            {
                UpdateMissions();
            }
        }

        private void UpdateMissions()
        {
            itemList.Clear();
            foreach (MissionInstance missionInstance in MissionManager.GetInstance().CurrentMissions)
            {
                MissionListItem item = new MissionListItem(missionInstance);
                itemList.AddItem(item);
            }
            MissionManager.GetInstance().HasChanged = false;
        }

        public void Hide()
        {
            itemList.Reset();
            textArea.Text = "";
        }

        public void Show()
        {

        }

        public void AddMission(MissionListItem mission)
        {
            itemList.AddItem(mission);
        }

        /// <summary>
        /// Action to be performed when a new list item has been selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnListSelectionChange(object sender, EventArgs args)
        {
            if (sender is MissionListItem)
            {
                MissionListItem mission = (MissionListItem)sender;
                //TODO: pricrapintas title atitraukimas nuo krasto
                textArea.Text = "       " + mission.Mission.CurrentMission.Title +  "\n" + mission.Mission.CurrentMission.Description;
            }
        }
    }
}
