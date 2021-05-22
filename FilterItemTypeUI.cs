using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using ChestBrowser.UIElements;
using Terraria.ModLoader;

namespace ChestBrowser
{
    class FilterItemTypeUI : UIModState
    {
        static internal FilterItemTypeUI instance;

        internal UIDragablePanel mainPanel;
        internal UIPanel inlaidPanel;
        internal UIGrid chestGrid;
        internal UIHoverImageButton closeButton;

        internal bool updateNeeded;
        internal string caption = "Filter Setting";
        internal bool isDrawLine = true;

        internal static bool[] chestTypeExist = new bool[Chest.chestTypeToIcon.Length];
        internal static bool[] chestTypeView = Enumerable.Repeat<bool>(true, Chest.chestTypeToIcon.Length).ToArray();
        internal static bool[] dresserTypeExist = new bool[Chest.dresserTypeToIcon.Length];
        internal static bool[] dresserTypeView = Enumerable.Repeat<bool>(true, Chest.dresserTypeToIcon.Length).ToArray();
        internal static void Clear()
        {
            chestTypeExist = new bool[Chest.chestTypeToIcon.Length];
            dresserTypeExist = new bool[Chest.dresserTypeToIcon.Length];
        }
        internal static bool isView(Chest chest)
        {
            bool result = isView(chest.isDresser(), chest.getIconIndex());
            return result;
        }
        internal static bool isView(bool isDresser, int iconIndex)
        {
            bool result = true;

            if (isDresser)
                result = dresserTypeView[iconIndex];
            else
                result = chestTypeView[iconIndex];

            return result;
        }

        public FilterItemTypeUI(UserInterface ui) : base(ui)
        {
            instance = this;
        }

        public override void OnInitialize()
        {
            mainPanel = new UIDragablePanel(true, true, true);
            mainPanel.caption = caption;
            mainPanel.SetPadding(6);
            mainPanel.Left.Set(714f, 0f);
            mainPanel.Top.Set(400f, 0f);
            mainPanel.Width.Set(314f, 0f);
            mainPanel.MinWidth.Set(314f, 0f);
            mainPanel.MaxWidth.Set(1393f, 0f);
            mainPanel.Height.Set(116, 0f);
            mainPanel.MinHeight.Set(116, 0f);
            mainPanel.MaxHeight.Set(1000, 0f);
            Append(mainPanel);

            Texture2D texture = ChestBrowser.instance.GetTexture("UIElements/closeButton");
            closeButton = new UIHoverImageButton(texture, "Close");
            closeButton.OnClick += CloseButtonClicked;
            closeButton.Left.Set(-20f, 1f);
            closeButton.Top.Set(3f, 0f);
            mainPanel.Append(closeButton);

            inlaidPanel = new UIPanel();
            inlaidPanel.SetPadding(6);
            inlaidPanel.Top.Pixels = 20;
            inlaidPanel.Width.Set(0, 1f);
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
        }

        internal void UpdateGrid()
        {
            if (!updateNeeded) { return; }
            updateNeeded = false;

            chestGrid.Clear();
            mainPanel.DragTargetClear();
            mainPanel.AddDragTarget(inlaidPanel);
            mainPanel.AddDragTarget(chestGrid);

            Clear();

            var rect = ChestBrowserUtils.GetSearchRangeRectangle();
            foreach (var chest in Main.chest.Where(x => x != null && (ModContent.GetInstance<ChestBrowserConfig>().isInfinityRange ? true : rect.Contains(x.x, x.y))))
            {
                int itemType = -1;
                bool isDresser = chest.isDresser();
                int iconIndex = chest.getIconIndex();
                if (isDresser && !FilterItemTypeUI.dresserTypeExist[iconIndex])
                {
                    FilterItemTypeUI.dresserTypeExist[iconIndex] = true;
                    itemType = Chest.dresserTypeToIcon[iconIndex];
                }
                else if (!isDresser && !FilterItemTypeUI.chestTypeExist[iconIndex])
                {
                    FilterItemTypeUI.chestTypeExist[iconIndex] = true;
                    itemType = Chest.chestTypeToIcon[iconIndex];
                }
                if (0 <= itemType)
                {
                    var box = new UIFilterChestSlot(isDresser, iconIndex, chest.ToItem());
                    chestGrid._items.Add(box);
                    chestGrid._innerList.Append(box);
                    mainPanel.AddDragTarget(box);
                }
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

        private class SaveInfo
        {
            public string position;
            public string chestTypeView;
            public string dresserTypeView;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Player player = Main.LocalPlayer;
        }
    }
}
