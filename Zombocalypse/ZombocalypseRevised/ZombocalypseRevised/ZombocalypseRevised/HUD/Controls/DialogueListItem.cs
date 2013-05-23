using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.Controls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XRpgLibrary.DialogueSystem;
using XRPGLibrary.Util;
using Microsoft.Xna.Framework.Input;
using XRPGLibrary;

namespace ZombocalypseRevised.HUD.Controls
{
    public class DialogueListItem : AListItem
    {
        private DialogueOption dialogueOption;
        private string wrappedText;

        private bool isClickedInside;

        public override event EventHandler Selected;

        public override Vector2 Size
        {
            set
            {
                this.wrappedText = StringUtils.WrapText(dialogueOption.ChatOption, font, value.X);

                Vector2 textSize = font.MeasureString(this.wrappedText);

                this.size = new Vector2(value.X, textSize.Y);
            }
        }

        public DialogueOption DialogueOption
        {
            get { return dialogueOption; }
        }

        public DialogueListItem(DialogueOption dialogueOption)
        {
            this.dialogueOption = dialogueOption;
            this.isClickedInside = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, wrappedText, position, renderColor);
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
        }
    }
}
