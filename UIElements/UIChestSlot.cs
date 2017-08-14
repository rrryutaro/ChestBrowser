using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace ChestBrowser.UIElements
{
	class UIChestSlot : UIItemSlot
	{
		public static Texture2D selectedBackgroundTexture = Main.inventoryBack15Texture;
		public static Texture2D recentlyDiscoveredBackgroundTexture = Main.inventoryBack10Texture;
        public Chest chest;
        private float distance;

        public UIChestSlot(Chest chest, float scale = 0.75f) : base(chest.ToItem(), scale)
		{
            this.chest = chest;
            SetDistance();
        }

		public override void Click(UIMouseEvent evt)
		{
		}

        public override void RightClick(UIMouseEvent evt)
        {
            if (Chest.isLocked(chest.x, chest.y) && Config.isCheatMode)
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
            if (Config.isCheatMode)
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
                ChestBrowser.instance.chestBrowserTool.tooltip = string.IsNullOrEmpty(chest.name) ? $"{item.Name}" : $"{chest.name}";
            }
        }

    }
}
