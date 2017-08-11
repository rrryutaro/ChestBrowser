using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using ChestBrowser.UIElements;

namespace ChestBrowser
{
	class ChestBrowserUI : UIModState
	{
		static internal ChestBrowserUI instance;

		internal UIDragablePanel mainPanel;
		internal UIPanel inlaidPanel;
		internal UIGrid chestGrid;
        internal UIHoverImageButton closeButton;

        internal bool updateNeeded;
        internal string caption = "Chest Browser v0.0.0.1   Chest:??";


        public ChestBrowserUI(UserInterface ui) : base(ui)
		{
			instance = this;
		}

		public override void OnInitialize()
		{
			mainPanel = new UIDragablePanel(true, true, true);
            mainPanel.caption = caption;
            mainPanel.SetPadding(6);
			mainPanel.Left.Set(400f, 0f);
			mainPanel.Top.Set(400f, 0f);
            //mainPanel.Width.Set(415f, 0f);
            mainPanel.Width.Set(314f, 0f);
            //mainPanel.MinWidth.Set(415f, 0f);
            mainPanel.MinWidth.Set(314f, 0f);
            //mainPanel.MaxWidth.Set(784f, 0f);
            mainPanel.MaxWidth.Set(1393f, 0f);
            //mainPanel.Height.Set(350, 0f);
            mainPanel.Height.Set(116, 0f);
            //mainPanel.MinHeight.Set(243, 0f);
            mainPanel.MinHeight.Set(116, 0f);
            mainPanel.MaxHeight.Set(1000, 0f);
			Append(mainPanel);

            Texture2D texture = ChestBrowser.instance.GetTexture("UIElements/closeButton");
            closeButton = new UIHoverImageButton(texture, "Close");
            closeButton.OnClick += CloseButtonClicked;
            closeButton.Left.Set(-20f, 1f);
            //closeButton.Top.Set(6f, 0f);
            closeButton.Top.Set(3f, 0f);
            mainPanel.Append(closeButton);

            inlaidPanel = new UIPanel();
			inlaidPanel.SetPadding(6);
            //inlaidPanel.Top.Pixels = 60;
            inlaidPanel.Top.Pixels = 20;
            inlaidPanel.Width.Set(0, 1f);
            //inlaidPanel.Height.Set(-60 - 121, 1f);
            inlaidPanel.Height.Set(0 - 40, 1f);
            mainPanel.Append(inlaidPanel);

            chestGrid = new UIGrid();
			chestGrid.Width.Set(-20f, 1f); 
			chestGrid.Height.Set(0, 1f);
			chestGrid.ListPadding = 2f;
			inlaidPanel.Append(chestGrid);

			var lootItemsScrollbar = new FixedUIScrollbar(userInterface);
			lootItemsScrollbar.SetView(100f, 1000f);
			lootItemsScrollbar.Height.Set(0, 1f);
			lootItemsScrollbar.Left.Set(-20, 1f);
			inlaidPanel.Append(lootItemsScrollbar);
			chestGrid.SetScrollbar(lootItemsScrollbar);
        }

        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
            ChestBrowser.instance.chestBrowserTool.visible = !ChestBrowser.instance.chestBrowserTool.visible;
        }

		internal void UpdateGrid()
		{
			if (!updateNeeded) { return; }
			updateNeeded = false;

            chestGrid.Clear();
            mainPanel.DragTargetClear();
            mainPanel.AddDragTarget(inlaidPanel);
            mainPanel.AddDragTarget(chestGrid);

            foreach (var chest in Main.chest.Where(x => x != null))
            {
                var box = new UIChestSlot(chest, 1f);
                chestGrid._items.Add(box);
                chestGrid._innerList.Append(box);
                mainPanel.AddDragTarget(box);
            }
            chestGrid.UpdateOrder();
			chestGrid._innerList.Recalculate();

            mainPanel.caption = caption.Replace("??", $"{chestGrid.Count}");
        }

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			UpdateGrid();
		}

        public override string SaveJsonString()
        {
            string result = mainPanel.SavePositionJsonString();
            return result;
        }
        public override void LoadJsonString(string jsonString)
        {
            mainPanel.LoadPositionJsonString(jsonString);
        }
    }
}
