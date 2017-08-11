using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace ChestBrowser.UIElements
{
	class UIChestSlot : UIItemSlot
	{
		public static Texture2D selectedBackgroundTexture = Main.inventoryBack15Texture;
		public static Texture2D recentlyDiscoveredBackgroundTexture = Main.inventoryBack10Texture;
        public Chest chest;

        public UIChestSlot(Chest chest, float scale = 0.75f) : base(chest.ToItem(), scale)
		{
            this.chest = chest;
        }

		public override void Click(UIMouseEvent evt)
		{
		}

        public override void RightClick(UIMouseEvent evt)
        {
            ChestBrowserUtils.OpenChest(chest);
        }

        public override void DoubleClick(UIMouseEvent evt)
		{
            ChestBrowserUtils.Teleport(chest.x, chest.y);
		}

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (IsMouseHovering)
            {
                ChestBrowser.instance.chestBrowserTool.tooltip = string.IsNullOrEmpty(chest.name) ? item.Name : chest.name;
            }
        }

    }
}
