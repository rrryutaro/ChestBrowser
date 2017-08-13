using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.Achievements;

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
            if (Chest.isLocked(chest.x, chest.y))
            {
                int num = (int)(Main.tile[chest.x, chest.y].frameX / 36);

                if (Unlock(chest.x, chest.y))
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
        private static bool Unlock(int X, int Y)
        {
            int num = (int)(Main.tile[X, Y].frameX / 36);
            int num2 = num;
            short num3;
            int type;
            switch (num2)
            {
                case 2:
                    {
                        num3 = 36;
                        type = 11;
                        AchievementsHelper.NotifyProgressionEvent(19);
                        goto IL_B7;
                    }
                case 3:
                    break;
                case 4:
                    {
                        num3 = 36;
                        type = 11;
                        goto IL_B7;
                    }
                default:
                    switch (num2)
                    {
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                        case 27:
                            {
                                num3 = 180;
                                type = 11;
                                AchievementsHelper.NotifyProgressionEvent(20);
                                goto IL_B7;
                            }
                        default:
                            switch (num2)
                            {
                                case 36:
                                case 38:
                                case 40:
                                    {
                                        num3 = 36;
                                        type = 11;
                                        goto IL_B7;
                                    }
                            }
                            break;
                    }
                    break;
            }
            return false;
            IL_B7:
            Main.PlaySound(22);
            for (int i = X; i <= X + 1; i++)
            {
                for (int j = Y; j <= Y + 1; j++)
                {
                    Tile expr_E8 = Main.tile[i, j];
                    expr_E8.frameX -= num3;
                    for (int k = 0; k < 4; k++)
                    {
                        Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, type, 0f, 0f, 0, default(Color), 1f);
                    }
                }
            }
            return true;
        }


        public override void DoubleClick(UIMouseEvent evt)
		{
            ChestBrowserUtils.Teleport(chest.x, chest.y);
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
