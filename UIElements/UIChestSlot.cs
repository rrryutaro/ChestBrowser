using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;

namespace ChestBrowser.UIElements
{
    class UIChestSlot : UIItemSlot
    {
        public static Texture2D selectedBackgroundTexture = Main.inventoryBack15Texture;
        public static Texture2D recentlyDiscoveredBackgroundTexture = Main.inventoryBack10Texture;
        public Chest chest;
        private float distance;

        public UIChestSlot(Chest chest) : base(chest.ToItem())
        {
            this.chest = chest;
            SetDistance();
        }

        public override void Click(UIMouseEvent evt)
        {
        }

        public override void RightClick(UIMouseEvent evt)
        {
            if (Chest.isLocked(chest.x, chest.y) && ModContent.GetInstance<ChestBrowserConfig>().isCheatMode)
            {
                int num = (int)(Main.tile[chest.x, chest.y].frameX / 36);

                if (ChestBrowserUtils.ChestUnlock(chest.x, chest.y))
                {
                    Item item = chest.ToItem();
                    base.item = item;
                    base.itemType = item.type;
                }
            }
            else
            {
                ChestBrowserUtils.OpenChest(chest);
            }
        }

        public override void DoubleClick(UIMouseEvent evt)
        {
            if (ModContent.GetInstance<ChestBrowserConfig>().isCheatMode)
            {
                ChestBrowserUtils.Teleport(chest.x, chest.y);
            }
        }

        public void SetDistance()
        {
            distance = Vector2.Distance(Main.LocalPlayer.Center, chest.getCenter());
        }
        public override int CompareTo(object obj)
        {
            int result = distance < (obj as UIChestSlot).distance ? -1 : 1;
            return result;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (IsMouseHovering)
            {
                Tool.tooltip = string.IsNullOrEmpty(chest.name) ? $"{item.Name}" : $"{chest.name}";
            }
        }

    }
}
