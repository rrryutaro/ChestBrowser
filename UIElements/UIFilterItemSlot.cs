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
	class UIFilterItemSlot : UIItemSlot
	{
		public static Texture2D selectedBackgroundTexture = Main.inventoryBack15Texture;
		public static Texture2D recentlyDiscoveredBackgroundTexture = Main.inventoryBack10Texture;

        public UIFilterItemSlot(Item item) : base(item)
		{
            item.stack = 1;
        }

        public override int CompareTo(object obj)
        {
            int result = item.netID < (obj as UIFilterItemSlot).item.netID ? -1 : 1;
            return result;
        }

        public override void RightClick(UIMouseEvent evt)
        {
            if (evt.Target == this)
            {
                ChestBrowserUI.instance.RemoveFilterItem(item.netID);
            }
        }
    }
}
