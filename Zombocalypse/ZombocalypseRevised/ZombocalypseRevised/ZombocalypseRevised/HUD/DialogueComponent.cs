using System;
using System.Collections.Generic;
using System.Text;
using XRPGLibrary.Controls;
using XRPGLibrary.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ZombocalypseRevised.HUD.Controls;

namespace XRpgLibrary.DialogueSystem
{
	public class DialogueComponent : AHUDComponent
	{
		private IChattable chatSupplier;
        private ItemList dialogueOptions;
        private TextArea responseArea;

        public event EventHandler DialogueSelected;

        public override Vector2 Size
        {
            get { return size; }
            set
            {
                this.size = value;
                dialogueOptions.Size = new Vector2(value.X / 3, value.Y);
                responseArea.Size = new Vector2(value.X * (2 / 3f), value.Y);
            }
        }

        public override Vector2 Position
        {
            get { return position; }
            set
            {
                this.position = value;  
                dialogueOptions.Position = value;
                responseArea.Position = new Vector2(value.X + dialogueOptions.Size.X, value.Y);
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

        public DialogueComponent(SpriteFont font, ContentManager content, IChattable chatSupplier)
            : base(font, content)
        {
            dialogueOptions = new ItemList(font);

            this.chatSupplier = chatSupplier;
            dialogueOptions.SelectionChange += OnListSelectionChange;
            PopulateOptions();

            responseArea = new TextArea(font, content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            dialogueOptions.Draw(spriteBatch);
            responseArea.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            commandManager.Update(gameTime);
            dialogueOptions.Update(gameTime);
            responseArea.Update(gameTime);
            if (DialogueManager.GetInstance().HasChanged)
            {
                PopulateOptions();
            }
        }

        private void PopulateOptions()
        {
            dialogueOptions.Clear();
            foreach (DialogueOption option in chatSupplier.Dialogues)
            {
                DialogueListItem item = new DialogueListItem(option);
                dialogueOptions.AddItem(item);
            }
            DialogueManager.GetInstance().HasChanged = false;
        }

        public void Hide()
        {
            dialogueOptions.Reset();
            responseArea.Text = "";
        }

        public void OnListSelectionChange(object sender, EventArgs args)
        {
            if (sender is DialogueListItem)
            {
                DialogueListItem dialogue = (DialogueListItem)sender;
                responseArea.Text = dialogue.DialogueOption.Response;
                if (!dialogue.DialogueOption.IsRepeatable)
                {
                    chatSupplier.RemoveDialogue(dialogue.DialogueOption);
                }

                if (DialogueSelected != null)
                {
                    DialogueSelected(dialogue.DialogueOption.DialogueInfo, null);
                }
            }
        }

        public override void LoadContent()
        {
        }
    }
}
