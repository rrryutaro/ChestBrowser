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
	class UIFilterChestSlot : UIItemSlot
	{
		public static Texture2D selectedBackgroundTexture = Main.inventoryBack15Texture;
		public static Texture2D recentlyDiscoveredBackgroundTexture = Main.inventoryBack10Texture;

        private bool isDresser;
        private int iconIndex;

        public UIFilterChestSlot(bool isDresser, int iconIndex, Item item, float scale = 0.75f) : base(item, scale)
		{
            this.isDresser = isDresser;
            this.iconIndex = iconIndex;
            disable = !FilterItemTypeUI.isView(isDresser, iconIndex);
        }

		public override void Click(UIMouseEvent evt)
		{
		}

        public override void RightClick(UIMouseEvent evt)
        {
            if (isDresser)
            {
                FilterItemTypeUI.dresserTypeView[iconIndex] = !FilterItemTypeUI.dresserTypeView[iconIndex];
                disable = !FilterItemTypeUI.dresserTypeView[iconIndex];
            }
            else
            {
                FilterItemTypeUI.chestTypeView[iconIndex] = !FilterItemTypeUI.chestTypeView[iconIndex];
                disable = !FilterItemTypeUI.chestTypeView[iconIndex];
            }
            ChestBrowserUI.instance.updateNeeded = true;
        }

        public override void DoubleClick(UIMouseEvent evt)
		{
		}

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (IsMouseHovering)
            {
                //ChestBrowser.instance.chestBrowserTool.tooltip = string.IsNullOrEmpty(chest.name) ? $"{item.Name}" : $"{chest.name}";
            }
        }

    }
}
