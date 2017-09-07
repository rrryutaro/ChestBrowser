using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.IO;
using ChestBrowser.UIElements;

namespace ChestBrowser
{
	class ChestBrowserUI : UIModState
	{
		static internal ChestBrowserUI instance;

        static internal int menuIconSize = 28;
        static internal int menuMargin = 4;

        internal UIDragablePanel panelMain;
        internal UISplitterPanel panelSplitter;
        internal UIPanel panelFilterChestType;
        internal UIPanel panelFilterItem;
        internal UIPanel panelChest;
        internal UIGrid gridFilterChestType;
        internal UIGrid gridFilterItem;
        internal UIGrid gridChest;
        internal UIHoverImageButton btnClose;
        internal UIImageListButton btnLine;
        internal UIImageListButton btnIconSize;
        internal UIImageListButton btnFilterChestType;
        internal UIImageListButton btnFilterChestTypeReverse;
        internal UIImageListButton btnFilterItem;
        internal UIImageListButton btnFilterChestName;

        internal bool[] chestTypeExist;
        internal bool[] chestTypeView;
        internal bool[] dresserTypeExist;
        internal bool[] dresserTypeView;
        private List<int> filterItemList;

        internal bool updateNeeded;
        internal string caption = $"Chest Browser v{ChestBrowser.instance.Version} Chest:??";

        internal bool isView(Chest chest)
        {
            bool result = isView(chest.isDresser(), chest.getIconIndex());
            result = btnFilterChestTypeReverse.GetValue<bool>()  ? !result : result;

            if (result && 0 < gridFilterItem.Count)
            {
                result = chest.item.Any(x => gridFilterItem._items.Cast<UIFilterItemSlot>().Select(y => y.item.netID).Contains(x.netID));
            }

            if (result && 0 < btnFilterChestName.GetValue<int>())
            {
                result = btnFilterChestName.GetValue<int>() == 1 ? !string.IsNullOrEmpty(chest.name) : string.IsNullOrEmpty(chest.name);
            }

            return result;
        }
        internal bool isView(bool isDresser, int iconIndex)
        {
            bool result = true;
        
            if (isDresser)
                result = dresserTypeView[iconIndex];
            else
                result = chestTypeView[iconIndex];
        
            return result;
        }

        public ChestBrowserUI(UserInterface ui) : base(ui)
		{
			instance = this;
		}

        public override void OnInitialize()
        {
        }

        public void InitializeUI()
        {
            RemoveAllChildren();
            chestTypeExist = new bool[Chest.chestTypeToIcon.Length];
            chestTypeView = Enumerable.Repeat<bool>(true, Chest.chestTypeToIcon.Length).ToArray();
            dresserTypeExist = new bool[Chest.dresserTypeToIcon.Length];
            dresserTypeView = Enumerable.Repeat<bool>(true, Chest.dresserTypeToIcon.Length).ToArray();
            filterItemList = new List<int>();

            //メインパネル
            panelMain = new UIDragablePanel(true, true, true);
            panelMain.caption = caption;
            panelMain.SetPadding(6);
			panelMain.Left.Set(400f, 0f);
			panelMain.Top.Set(400f, 0f);
            panelMain.Width.Set(314f, 0f);
            panelMain.MinWidth.Set(314f, 0f);
            panelMain.MaxWidth.Set(1393f, 0f);
            panelMain.Height.Set(131f, 0f);
            panelMain.MinHeight.Set(131f, 0f);
            panelMain.MaxHeight.Set(1000f, 0f);
			Append(panelMain);

            //フィルターパネル
            panelFilterChestType = new UIPanel();
            panelFilterChestType.SetPadding(6);
            panelFilterChestType.MinWidth.Set(100, 0);
            gridFilterChestType = new UIGrid();
            gridFilterChestType.Width.Set(-20f, 1f);
            gridFilterChestType.Height.Set(0, 1f);
            gridFilterChestType.ListPadding = 2f;
            panelFilterChestType.Append(gridFilterChestType);
            var filterGridScrollbar = new FixedUIScrollbar(userInterface);
            filterGridScrollbar.SetView(100f, 1000f);
            filterGridScrollbar.Height.Set(0, 1f);
            filterGridScrollbar.Left.Set(-20, 1f);
            panelFilterChestType.Append(filterGridScrollbar);
            gridFilterChestType.SetScrollbar(filterGridScrollbar);
            //アイテムフィルターパネル
            panelFilterItem = new UIPanel();
            panelFilterItem.SetPadding(6);
            panelFilterItem.MinWidth.Set(100, 0);
            gridFilterItem = new UIGrid();
            gridFilterItem.Width.Set(-20f, 1f);
            gridFilterItem.Height.Set(0, 1f);
            gridFilterItem.ListPadding = 2f;
            panelFilterItem.Append(gridFilterItem);
            var itemFilterGridScrollbar = new FixedUIScrollbar(userInterface);
            itemFilterGridScrollbar.SetView(100f, 1000f);
            itemFilterGridScrollbar.Height.Set(0, 1f);
            itemFilterGridScrollbar.Left.Set(-20, 1f);
            panelFilterItem.Append(itemFilterGridScrollbar);
            gridFilterItem.SetScrollbar(itemFilterGridScrollbar);
            //チェストパネル
            panelChest = new UIPanel();
            panelChest.SetPadding(6);
            panelChest.MinWidth.Set(100, 0);
            gridChest = new UIGrid();
            gridChest.Width.Set(-20f, 1f);
            gridChest.Height.Set(0, 1f);
            gridChest.ListPadding = 2f;
            panelChest.Append(gridChest);
            var chestGridScrollbar = new FixedUIScrollbar(userInterface);
            chestGridScrollbar.SetView(100f, 1000f);
            chestGridScrollbar.Height.Set(0, 1f);
            chestGridScrollbar.Left.Set(-20, 1f);
            panelChest.Append(chestGridScrollbar);
            gridChest.SetScrollbar(chestGridScrollbar);
            //スプリッターパネル
            panelSplitter = new UISplitterPanel(panelFilterChestType, panelChest);
            panelSplitter.SetPadding(0);
            panelSplitter.Top.Pixels = menuIconSize + menuMargin * 2;
            panelSplitter.Width.Set(0, 1f);
            panelSplitter.Height.Set(-26 - menuIconSize, 1f);
            panelSplitter.Panel1Visible = false;
            panelMain.Append(panelSplitter);

            //閉じるボタン
            Texture2D texture = ChestBrowser.instance.GetTexture("UIElements/closeButton");
            btnClose = new UIHoverImageButton(texture, "Close");
            btnClose.OnClick += (a, b) => ChestBrowser.instance.chestBrowserTool.visible = false;
            btnClose.Left.Set(-20f, 1f);
            btnClose.Top.Set(3f, 0f);
            panelMain.Append(btnClose);

            //線表示ボタン
            btnLine = new UIImageListButton(
                new List<Texture2D>() { Main.inventoryTickOnTexture.Resize(menuIconSize), Main.inventoryTickOffTexture.Resize(menuIconSize) },
                new List<object>() { true, false },
                new List<string>() { "Display line", "Hide line" });
            btnLine.OnClick += (a, b) => btnLine.NextIamge();
            btnLine.Left.Set(btnClose.Left.Pixels - menuMargin - menuIconSize, 1f);
            btnLine.Top.Set(3f, 0f);
            panelMain.Append(btnLine);

            //アイコンサイズボタン
            btnIconSize = new UIImageListButton(
                new List<Texture2D>() {
                    Main.itemTexture[ItemID.Chest].Resize(menuIconSize),
                    Main.itemTexture[ItemID.Chest].Resize((int)(menuIconSize * 0.8f)),
                    Main.itemTexture[ItemID.Chest].Resize((int)(menuIconSize * 0.6f))},
                new List<object>() { 1.0f, 0.8f, 0.6f },
                new List<string>() { "Icon size large", "Icon size medium", "Icon size small" });
            btnIconSize.OnClick += (a, b) =>
            {
                btnIconSize.NextIamge();
                UIItemSlot.scale = btnIconSize.GetValue<float>();
            };
            btnIconSize.Left.Set(btnLine.Left.Pixels - menuMargin - menuIconSize, 1f);
            btnIconSize.Top.Set(3f, 0f);
            panelMain.Append(btnIconSize);

            //フィルターボタン
            btnFilterChestType = new UIImageListButton(
                new List<Texture2D>() { Main.itemTexture[ItemID.Chest].Resize(menuIconSize), Main.itemTexture[ItemID.GoldChest].Resize(menuIconSize) },
                new List<object>() { false, true },
                new List<string>() { "Hide filter", "Display filter" });
            btnFilterChestType.OnClick += (a, b) =>
            {
                btnFilterChestType.NextIamge();
                if (btnFilterChestType.GetValue<bool>())
                {
                    btnFilterItem.Index = 0;
                }
                ChangeSpliterPanel();
            };
            btnFilterChestType.Left.Set(menuMargin, 0f);
            btnFilterChestType.Top.Set(3f, 0f);
            panelMain.Append(btnFilterChestType);

            //リバースボタン
            btnFilterChestTypeReverse = new UIImageListButton(
                new List<Texture2D>() {
                    ChestBrowser.instance.GetTexture("UIElements/reverseButton2").Resize(menuIconSize),
                    ChestBrowser.instance.GetTexture("UIElements/reverseButton1").Resize(menuIconSize) },
                new List<object>() { false, true },
                new List<string>() { "Not filter reverse", "Filter reverse" });
            btnFilterChestTypeReverse.OnClick += (a, b) =>
            {
                btnFilterChestTypeReverse.NextIamge();
                updateNeeded = true;
            };
            btnFilterChestTypeReverse.Left.Set(btnFilterChestType.Left.Pixels + menuIconSize + menuMargin, 0f);
            btnFilterChestTypeReverse.Top.Set(3f, 0f);
            panelMain.Append(btnFilterChestTypeReverse);

            //アイテムフィルターボタン
            btnFilterItem = new UIImageListButton(
                new List<Texture2D>() { Main.itemTexture[ItemID.Chest].Resize(menuIconSize), Main.itemTexture[ItemID.GoldChest].Resize(menuIconSize) },
                new List<object>() { false, true },
                new List<string>() { "Hide item filter", "Display item filter"});
            btnFilterItem.OnClick += (a, b) =>
            {
                btnFilterItem.NextIamge();
                if (btnFilterItem.GetValue<bool>())
                {
                    btnFilterChestType.Index = 0;
                }
                ChangeSpliterPanel();
            };
            btnFilterItem.Left.Set(btnFilterChestTypeReverse.Left.Pixels + (menuIconSize + menuMargin) * 2, 0f);
            btnFilterItem.Top.Set(3f, 0f);
            panelMain.Append(btnFilterItem);

            //名前フィルターボタン
            btnFilterChestName = new UIImageListButton(
                new List<Texture2D>() {
                    Main.itemTexture[ItemID.AlphabetStatueA].Resize(menuIconSize),
                    Main.itemTexture[ItemID.AlphabetStatueN].Resize(menuIconSize),
                    Main.itemTexture[ItemID.AlphabetStatueU].Resize(menuIconSize) },
                new List<object>() { 0, 1, 2 },
                new List<string>() { "Named chest filter: All", "Named chest filter: Named", "Named chest filter: No names" });
            btnFilterChestName.OnClick += (a, b) =>
            {
                btnFilterChestName.NextIamge();
                updateNeeded = true;
            };
            btnFilterChestName.Left.Set(btnFilterItem.Left.Pixels + (menuIconSize + menuMargin) * 2, 0f);
            btnFilterChestName.Top.Set(3f, 0f);
            panelMain.Append(btnFilterChestName);
        }

        private void ChangeSpliterPanel()
        {
            if (btnFilterChestType.GetValue<bool>())
            {
                panelSplitter.Panel1Visible = true;
                panelSplitter.SetPanel1(panelFilterChestType);
            }
            else if (btnFilterItem.GetValue<bool>())
            {
                panelSplitter.Panel1Visible = true;
                panelSplitter.SetPanel1(panelFilterItem);
            }
            else
            {
                panelSplitter.Panel1Visible = false;
            }
            updateNeeded = true;
        }

        public void AddFilterItem(int netID)
        {
            if (!filterItemList.Contains(netID))
            {
                filterItemList.Add(netID);
                updateNeeded = true;
            }
        }
        public void RemoveFilterItem(int netID)
        {
            if (filterItemList.Remove(netID))
            {
                updateNeeded = true;
            }
        }

        private void Clear()
        {
            gridFilterItem.Clear();
            gridFilterChestType.Clear();
            gridChest.Clear();
            panelMain.DragTargetClear();

            chestTypeExist = new bool[Chest.chestTypeToIcon.Length];
            dresserTypeExist = new bool[Chest.dresserTypeToIcon.Length];

            panelSplitter.Recalculate();
        }

        internal void UpdateGrid()
		{
			if (!updateNeeded) { return; }
			updateNeeded = false;

            Clear();

            foreach (var netID in filterItemList)
            {
                var item = new Item();
                item.SetDefaults(netID);

                var box = new UIFilterItemSlot(item);
                gridFilterItem._items.Add(box);
                gridFilterItem._innerList.Append(box);
            }
            gridFilterItem.UpdateOrder();
            gridFilterItem._innerList.Recalculate();

            var rect = ChestBrowserUtils.GetSearchRangeRectangle();
            foreach (var chest in Main.chest.Where(x => x != null && (Config.isInfinityRange ? true : rect.Contains(x.x, x.y))))
            {
                if (panelSplitter.Panel1Visible && btnFilterChestType.GetValue<bool>())
                {
                    int itemType = -1;
                    bool isDresser = chest.isDresser();
                    int iconIndex = chest.getIconIndex();
                    if (isDresser && !dresserTypeExist[iconIndex])
                    {
                        dresserTypeExist[iconIndex] = true;
                        itemType = Chest.dresserTypeToIcon[iconIndex];
                    }
                    else if (!isDresser && !chestTypeExist[iconIndex])
                    {
                        chestTypeExist[iconIndex] = true;
                        itemType = Chest.chestTypeToIcon[iconIndex];
                    }
                    if (0 <= itemType)
                    {
                        var box = new UIFilterChestSlot(isDresser, iconIndex, chest.ToItem());
                        gridFilterChestType._items.Add(box);
                        gridFilterChestType._innerList.Append(box);
                        panelMain.AddDragTarget(box);
                    }
                }

                if (isView(chest))
                {
                    var box = new UIChestSlot(chest);
                    gridChest._items.Add(box);
                    gridChest._innerList.Append(box);
                    panelMain.AddDragTarget(box);
                }
            }
            gridFilterChestType.UpdateOrder();
            gridFilterChestType._innerList.Recalculate();
            gridChest.UpdateOrder();
            gridChest._innerList.Recalculate();

            panelMain.caption = caption.Replace("??", $"{gridChest.Count}");
        }

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			UpdateGrid();
		}

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Player player = Main.LocalPlayer;
            if (btnLine.GetValue<bool>() && 0 <= player.chest)
            {
                Utils.DrawLine(spriteBatch, player.Center, Main.chest[player.chest].getCenter(), Color.Red, Color.Red, 1);
            }
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            if (btnFilterItem.GetValue<bool>() && evt.Target == gridFilterItem._innerList &&(Main.mouseItem != null))
            {
                AddFilterItem(Main.mouseItem.netID);
            }

            if (!Config.isCheatMode && !this.ContainsPoint(evt.MousePosition))
            {
                ChestBrowser.instance.chestBrowserTool.visible = false;
            }
            base.MouseDown(evt);
        }

        public override void RightMouseDown(UIMouseEvent evt)
        {
            if (!Config.isCheatMode && !this.ContainsPoint(evt.MousePosition))
            {
                ChestBrowser.instance.chestBrowserTool.visible = false;
            }
            base.RightMouseDown(evt);
        }

        public override TagCompound Save()
        {
            TagCompound result = base.Save();

            if (panelMain != null)
            {
                result.Add("position", panelMain.SavePositionJsonString());
                result.Add("SplitterBarLeft", panelSplitter.GetSplitterBarLeft());
                result.Add("chestTypeView", chestTypeView.ToList());
                result.Add("dresserTypeView", dresserTypeView.ToList());
                result.Add("filterItemList", filterItemList);

                result.Add("btnLine", btnLine.Index);
                result.Add("btnIconSize", btnIconSize.Index);
                result.Add("btnFilterChestType", btnFilterChestType.Index);
                result.Add("btnFilterChestTypeReverse", btnFilterChestTypeReverse.Index);
                result.Add("btnFilterItem", btnFilterItem.Index);
                result.Add("btnFilterChestName", btnFilterChestName.Index);
            }

            return result;
        }

        public override void Load(TagCompound tag)
        {
            base.Load(tag);
            if (tag.ContainsKey("position"))
            {
                panelMain.LoadPositionJsonString(tag.GetString("position"));
            }
            if (tag.ContainsKey("SplitterBarLeft"))
            {
                panelSplitter.SetSplitterBarLeft(tag.GetFloat("SplitterBarLeft"));
            }
            if (tag.ContainsKey("chestTypeView"))
            {
                chestTypeView = tag.GetList<bool>("chestTypeView").ToArray();
            }
            if (tag.ContainsKey("dresserTypeView"))
            {
                dresserTypeView = tag.GetList<bool>("dresserTypeView").ToArray();
            }
            if (tag.ContainsKey("filterItemList"))
            {
                tag.GetList<int>("filterItemList").ToList().ForEach(x => AddFilterItem(x));
            }
            if (tag.ContainsKey("btnLine"))
            {
                btnLine.Index = tag.GetInt("btnLine");
            }
            if (tag.ContainsKey("btnIconSize"))
            {
                btnIconSize.Index = tag.GetInt("btnIconSize");
                UIItemSlot.scale = btnIconSize.GetValue<float>();
            }
            if (tag.ContainsKey("btnFilterChestType"))
            {
                btnFilterChestType.Index = tag.GetInt("btnFilterChestType");
            }
            if (tag.ContainsKey("btnFilterChestTypeReverse"))
            {
                btnFilterChestTypeReverse.Index = tag.GetInt("btnFilterChestTypeReverse");
            }
            if (tag.ContainsKey("btnFilterItem"))
            {
                btnFilterItem.Index = tag.GetInt("btnFilterItem");
            }
            if (tag.ContainsKey("btnFilterChestName"))
            {
                btnFilterChestName.Index = tag.GetInt("btnFilterChestName");
            }
            ChangeSpliterPanel();
        }
    }
}
