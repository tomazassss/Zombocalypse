using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRPGLibrary.HUD;
using Microsoft.Xna.Framework;
using XRPGLibrary.Controls.Managers;
using Microsoft.Xna.Framework.Graphics;
using XRPGLibrary.Controls;
using XRPGLibrary.Input;
using Microsoft.Xna.Framework.Input;
using ZombocalypseRevised.GameScreens;
using XRPGLibrary;
using Microsoft.Xna.Framework.Content;
using ZombocalypseRevised.Util;

namespace ZombocalypseRevised.HUD
{
    public class InventoryHUDWindow : AHUDWindow
    {
        private const int EQUIPMENT_INDEX = 0;
        private const int INVENTORY_INDEX = 1;

        private SimpleHUDWindow inventoryWindow;
        private SimpleHUDWindow equipmentWindow;

        private IHUDWindow[] windows;
        private InventoryManager inventoryManager;
        private CommandManager commandManager;
        private GamePlayScreen parent;
        
        public InventoryHUDWindow(GamePlayScreen parent, SpriteFont font, ContentManager content, Texture2D border, Rectangle screenRectangle)
            : base()
        {
            this.windows = new IHUDWindow[2];
            this.inventoryManager = new InventoryManager();
            this.inventoryManager.DisposeItem += OnDisposeItem;

            this.equipmentWindow = CreateEquipmentWindow(font, content, border, screenRectangle, inventoryManager);
            this.inventoryWindow = CreateInventoryWindow(font, content, border, screenRectangle, inventoryManager);

            this.windows[EQUIPMENT_INDEX] = equipmentWindow;
            this.windows[INVENTORY_INDEX] = inventoryWindow;

            this.commandManager = new CommandManager();

            this.enabled = false;
            this.visible = false;

            this.parent = parent;

            //Initialize();
        }

        public void Initialize()
        {
            foreach (IHUDWindow window in windows)
            {
                if (window is SimpleHUDWindow)
                {
                    SimpleHUDWindow tempWindow = (SimpleHUDWindow)window;
                    AInventoryComponent component = tempWindow.Component as AInventoryComponent;
                    if (component != null)
                    {
                        component.InventoryManager = inventoryManager;
                    }
                }
            }
        }

        public void AddItemToInventory(InventoryItem item)
        {
            InventoryWindowComponent inventory = inventoryWindow.Component as InventoryWindowComponent;
            inventory.Inventory.AddItemToFirstFreeSlot(item);
        }

        public void EquipItem(InventoryItem item)
        {
            EquipmentWindowComponent equipment = equipmentWindow.Component as EquipmentWindowComponent;
            equipment.EquipItem(item);
        }

        private SimpleHUDWindow CreateEquipmentWindow(SpriteFont font, ContentManager content, Texture2D border, Rectangle screenRectangle, InventoryManager inventoryManager)
        {
            SimpleHUDWindow equipmentWindow = new SimpleHUDWindow(font);
            //TODO: hardcoded title
            equipmentWindow.Title = "Equipment";
            EquipmentWindowComponent equipmentComponent = new EquipmentWindowComponent(font, content, inventoryManager);
            equipmentComponent.RegisterUnequip(OnUnequip);
            equipmentComponent.RegisterReplace(OnUnequip);
            equipmentWindow.BorderTexture = border;
            //TODO: hardcoded size            
            equipmentWindow.Size = new Vector2(400, 600);

            float positionOffset = (screenRectangle.Width / 2 >= equipmentWindow.Size.X) ?
                                   (screenRectangle.Width / 2 - equipmentWindow.Size.X) / 2 :
                                   0;
            Vector2 position = new Vector2(positionOffset, (screenRectangle.Height - equipmentWindow.Size.Y) / 2);
            equipmentWindow.Position = position;
            equipmentWindow.Component = equipmentComponent;
            equipmentComponent.RepositionHolders();

            return equipmentWindow;
        }

        private SimpleHUDWindow CreateInventoryWindow(SpriteFont font, ContentManager content, Texture2D border, Rectangle screenRectangle, InventoryManager inventoryManager)
        {
            SimpleHUDWindow inventoryWindow = new SimpleHUDWindow(font);
            //TODO: hardcoded title
            inventoryWindow.Title = "Inventory";
            InventoryWindowComponent inventoryComponent = new InventoryWindowComponent(font, content, inventoryManager);
            inventoryComponent.RegisterEquip(OnEquip);
            inventoryWindow.BorderTexture = border;
            //TODO: hardcoded size
            inventoryWindow.Size = new Vector2(400, 600);

            float positionOffset = (screenRectangle.Width / 2 >= inventoryWindow.Size.X) ?
                                   (screenRectangle.Width / 2 - inventoryWindow.Size.X) / 2 :
                                   0;
            Vector2 position = new Vector2(screenRectangle.Width / 2 + positionOffset, (screenRectangle.Height - inventoryWindow.Size.Y) / 2);
            inventoryWindow.Position = position;
            inventoryWindow.Component = inventoryComponent;

            return inventoryWindow;
        }

        public override void Update(GameTime gameTime)
        {
            if (enabled)
            {
                foreach (IHUDWindow window in windows)
                {
                    window.Update(gameTime);
                }
                commandManager.Update(gameTime);
                inventoryManager.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                foreach (IHUDWindow window in windows)
                {
                    window.Draw(spriteBatch);
                }
                inventoryManager.Draw(spriteBatch);
            }
        }

        public void Close()
        {
            HideEquipment();
            HideInventory();
        }

        public override void Hide()
        {
            inventoryManager.Enabled = false;
            inventoryManager.Visible = false;
            enabled = false;
            visible = false;
            parent.IsPaused = false;
            parent.CommandManager.Enabled = true;
            if (inventoryManager.DraggedItem != null)
            {
                inventoryManager.DropDraggedItem();
            }
            InputHandler.Flush();
        }

        public override void Show()
        {
            inventoryManager.Enabled = true;
            inventoryManager.Visible = true;
            enabled = true;
            visible = true;
            parent.IsPaused = true;
            parent.CommandManager.Enabled = false;
            InputHandler.Flush();
        }

        public void ToggleEquipment()
        {
            if (equipmentWindow.Enabled)
            {
                HideEquipment();
            }
            else
            {
                ShowEquipment();
            }
        }

        public void ToggleInventory()
        {
            if (inventoryWindow.Enabled)
            {
                HideInventory();
            }
            else
            {
                ShowInventory();
            }
        }

        public void HideEquipment()
        {
            if (!inventoryWindow.Enabled)
            {
                Hide();
            }
            EquipmentWindowComponent equipment = equipmentWindow.Component as EquipmentWindowComponent;
            equipment.SetEnabled(false);
            equipmentWindow.Hide();
        }

        public void ShowEquipment()
        {
            if (!Enabled)
            {
                Show();
            }
            EquipmentWindowComponent equipment = equipmentWindow.Component as EquipmentWindowComponent;
            equipment.SetEnabled(true);
            equipmentWindow.Show();
        }

        public void HideInventory()
        {
            if (!equipmentWindow.Enabled)
            {
                Hide();
            }
            InventoryWindowComponent inventory = inventoryWindow.Component as InventoryWindowComponent;
            inventory.Inventory.SetEnabled(false);
            inventoryWindow.Hide();
        }

        public void ShowInventory()
        {
            if (!Enabled)
            {
                Show();
            }
            InventoryWindowComponent inventory = inventoryWindow.Component as InventoryWindowComponent;
            inventory.Inventory.SetEnabled(true);
            inventoryWindow.Show();
        }

        public void RegisterCommand(Keys key, Action action)
        {
            RegisterCommand(key, action, ClickState.RELEASED);
        }

        public void RegisterCommand(Keys key, Action action, ClickState clickState)
        {
            commandManager.RegisterCommand(key, action, clickState);
        }

        public void RegisterStatChangeEvent(EventHandler evnt)
        {
            EquipmentWindowComponent equipment = (EquipmentWindowComponent) equipmentWindow.Component;

            equipment.Equipped += evnt;
        }

        public void OnEquip(object sender, EventArgs args)
        {
            if (!(sender is InventoryItem))
            {
                return;
            }
            else
            {
                InventoryItem item = (InventoryItem)sender;
                EquipmentWindowComponent equipment = equipmentWindow.Component as EquipmentWindowComponent;
                equipment.EquipItem(item);
            }
        }

        public void OnUnequip(object sender, EventArgs args)
        {
            if (!(sender is InventoryItem) ||
                inventoryManager.DraggedItem != null)
            {
                return;
            }
            else
            {
                InventoryItem item = (InventoryItem)sender;
                InventoryWindowComponent inventory = inventoryWindow.Component as InventoryWindowComponent;

                if (!inventory.Inventory.AddItemToFirstFreeSlot(item))
                {
                    inventoryManager.DraggedItem = item;
                    item.IsBeingDragged = true;
                    item.IsMouseInside = false;
                }
            }
        }

        public void OnItemPickup(object sender, EventArgs args)
        {
            //Item pickup is treated the same as unequiping an item
            //that is, it is added to inventory's first free slot
            OnUnequip(sender, args);
        }



        public void OnDisposeItem(object sender, EventArgs args)
        {
            //TODO: Kai bus sukurtas alert boxas, paklausti zaidejo, ar tikrai nori ismest daikta
            if (!IsMouseInside() && inventoryManager.DraggedItem != null)
            {
                // Null is passed into PopUpManager.CreateInstance method because it is
                // a singleton class and should be initialized somewhere else
                //TODO: hardcoded PopUp message
                SimpleHUDWindow window = WindowFactory.CreatePopup("Drop item?", OnPopupYes, OnPopupNo);
                this.enabled = false;
                window.Enabled = true;
                window.Visible = true;
                PopUpManager.CreateInstance(null).Push(window);
            }
        }

        private void OnPopupYes(object sender, EventArgs args)
        {
            inventoryManager.DraggedItem = null;
            PopUpManager.CreateInstance(null).Pop();
            this.enabled = true;
        }

        private void OnPopupNo(object sender, EventArgs args)
        {
            PopUpManager.CreateInstance(null).Pop();
            this.enabled = true;
        }

        public override bool IsMouseInside()
        {
            return (inventoryWindow.IsMouseInside() && inventoryWindow.Enabled) || 
                   (equipmentWindow.IsMouseInside() && equipmentWindow.Enabled);
        }
    }
}
