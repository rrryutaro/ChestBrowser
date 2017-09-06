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

        public UIFilterChestSlot(bool isDresser, int iconIndex, Item item) : base(item)
		{
            this.isDresser = isDresser;
            this.iconIndex = iconIndex;
            disable = !ChestBrowserUI.instance.isView(isDresser, iconIndex);
        }

		public override void Click(UIMouseEvent evt)
		{
		}

        public override void RightClick(UIMouseEvent evt)
        {
            if (isDresser)
            {
                ChestBrowserUI.instance.dresserTypeView[iconIndex] = !ChestBrowserUI.instance.dresserTypeView[iconIndex];
                disable = !ChestBrowserUI.instance.dresserTypeView[iconIndex];
            }
            else
            {
                ChestBrowserUI.instance.chestTypeView[iconIndex] = !ChestBrowserUI.instance.chestTypeView[iconIndex];
                disable = !ChestBrowserUI.instance.chestTypeView[iconIndex];
            }
            ChestBrowserUI.instance.updateNeeded = true;
        }

        public override void DoubleClick(UIMouseEvent evt)
		{
		}

        public override int CompareTo(object obj)
        {
            int result = iconIndex < (obj as UIFilterChestSlot).iconIndex ? -1 : 1;
            return result;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
        }
    }
}
