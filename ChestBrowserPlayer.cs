using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

namespace ChestBrowser
{
	public class ChestBrowserPlayer : ModPlayer
	{
		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (ChestBrowser.instance.HotKey.JustPressed)
			{
                ChestBrowser.instance.chestBrowserTool.visible = !ChestBrowser.instance.chestBrowserTool.visible;
                if (ChestBrowser.instance.chestBrowserTool.visible)
                    ChestBrowserUI.instance.updateNeeded = true;
            }
        }

		public override TagCompound Save()
		{
            return new TagCompound
            {
                ["ChestBrowserUI"] = ChestBrowser.instance.chestBrowserTool.uistate.SaveJsonString(),
            };
        }

		public override void Load(TagCompound tag)
		{
            if (tag.ContainsKey("ChestBrowserUI"))
            {
                ChestBrowser.instance.chestBrowserTool.uistate.LoadJsonString(tag.GetString("ChestBrowserUI"));
            }
		}

        /// <summary>
        /// チェストブラウザーを表示中はタイルレンジを無制限にする
        /// </summary>
        /// <remarks>
        /// HEROsMod の InfiniteReach.cs の public override void ResetEffects() 部分を流用
        /// </remarks>
        public override void ResetEffects()
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (ChestBrowser.instance.chestBrowserTool.visible)
                {
                    Player.tileRangeX = int.MaxValue / 32 - 20;
                    Player.tileRangeY = int.MaxValue / 32 - 20;
                }
            }
        }

    }
}
