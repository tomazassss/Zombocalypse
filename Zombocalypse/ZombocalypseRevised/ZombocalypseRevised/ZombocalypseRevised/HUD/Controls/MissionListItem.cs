using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.Controls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XRpgLibrary.MissionSystem;
using XRPGLibrary.Util;
using Microsoft.Xna.Framework.Input;
using XRPGLibrary;

namespace ZombocalypseRevised.HUD.Controls
{
    public class MissionListItem : AListItem
    {
        private MissionNodeTemp missionInfo;
        private MissionInstance mission;

        private Vector2 animatedPosition;

        private float textOverflow;
        private float currentPositionOffset;
        private bool isAnimatingLeft;
        private bool isClickedInside;

        public override event EventHandler Selected;

        public override Vector2 Size
        {
            get { return size; }
            set
            {
                /*
                Vector2 textSize = font.MeasureString(missionInfo.Title);
                this.size = new Vector2(value.X, textSize.Y);
                this.textOverflow = textSize.X - this.size.X; 
                if (textSize.X > this.size.X)
                {
                    missionInfo.ShortTitle = StringUtils.ShortenText(missionInfo.Title, font, this.size.X);
                    missionInfo.UseShortTitle = true;
                }
                else if(missionInfo.UseShortTitle)
                {
                    missionInfo.UseShortTitle = false;
                }
                */

                Vector2 textSize = font.MeasureString(mission.CurrentMission.Title);
                this.size = new Vector2(value.X, textSize.Y);
                this.textOverflow = textSize.X - this.size.X;
                if (textSize.X > this.size.X)
                {
                    mission.CurrentMission.ShortTitle = StringUtils.ShortenText(
                        mission.CurrentMission.Title, font, this.size.X);

                    mission.CurrentMission.UseShortTitle = true;
                }
                else if (mission.CurrentMission.UseShortTitle)
                {
                    mission.CurrentMission.UseShortTitle = false;
                }
            }
        }

        public override bool IsSelected
        {
            set
            {
                isSelected = value;
                this.animatedPosition = position;
                this.isAnimatingLeft = true;
                this.currentPositionOffset = 0;
            }
        }

        public MissionNodeTemp MissionInfo
        {
            get { return missionInfo; }
        }

        public MissionInstance Mission
        {
            get { return mission; }
        }

        public MissionListItem(MissionInstance mission)
        {
            //this.missionInfo = missionInfo;
            this.mission = mission;
            this.isAnimatingLeft = true;
            this.isClickedInside = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            if (!isSelected)
            {
                if (mission.CurrentMission.UseShortTitle)
                {
                    spriteBatch.DrawString(font, mission.CurrentMission.ShortTitle, position, renderColor);
                }
                else
                {
                    spriteBatch.DrawString(font, mission.CurrentMission.Title, position, renderColor);
                }
            }
            else
            {
                DrawClipping(spriteBatch);
            }
        }

        private void DrawClipping(SpriteBatch spriteBatch)
        {

            Rectangle clipRectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)size.X,
                (int)size.Y);

            DrawingUtils.SpriteBatchEnd(spriteBatch);
            DrawingUtils.SpriteBatchBeginClipped(spriteBatch, clipRectangle);

            spriteBatch.GraphicsDevice.ScissorRectangle = clipRectangle;

            spriteBatch.DrawString(font, mission.CurrentMission.Title, animatedPosition, renderColor);

            DrawingUtils.SpriteBatchEndClipped(spriteBatch);
            DrawingUtils.SpriteBatchBegin(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsMouseInside())
            {
                renderColor = hoverColor;
            }
            else
            {
                renderColor = normalColor;
            }

            if (IsMouseInside() && InputHandler.LeftButtonPressed())
            {
                isClickedInside = true;
            }

            if (IsMouseInside() &&
                InputHandler.LeftButtonReleased() &&
                isClickedInside)
            {
                if (!isSelected && Selected != null)
                {
                    Selected(this, null);
                }
                isClickedInside = false;
            }

            if (isSelected)
            {
                AnimateTitle();
            }
        }

        private void AnimateTitle()
        {
            //TODO: hardcoded scrolling speed
            if (textOverflow > 0)
            {
                //animatedPosition = position;
                if (isAnimatingLeft)
                {
                    currentPositionOffset -= 0.5f;
                    animatedPosition.X -= 0.5f;
                    if (Math.Abs(currentPositionOffset) >= textOverflow)
                    {
                        isAnimatingLeft = false;
                    }
                }
                else
                {
                    currentPositionOffset += 0.5f;
                    animatedPosition.X += 0.5f;
                    if (Math.Abs(currentPositionOffset) == 0)
                    {
                        isAnimatingLeft = true;
                    }
                }
            }
        }
    }
}
