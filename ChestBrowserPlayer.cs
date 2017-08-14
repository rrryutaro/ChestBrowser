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
                {
                    ChestBrowserUI.instance.updateNeeded = true;

                    ChestBrowser.instance.filterItemTypeTool.visible = true;
                    FilterItemTypeUI.instance.updateNeeded = true;
                }
                else
                    ChestBrowser.instance.filterItemTypeTool.visible = false;
            }
        }

		public override TagCompound Save()
		{
            return new TagCompound
            {
                ["ChestBrowserUI"] = ChestBrowser.instance.chestBrowserTool.uistate.SaveJsonString(),
                ["FilterItemTypeUI"] = ChestBrowser.instance.filterItemTypeTool.uistate.SaveJsonString(),
            };
        }

		public override void Load(TagCompound tag)
		{
            if (tag.ContainsKey("ChestBrowserUI"))
            {
                ChestBrowser.instance.chestBrowserTool.uistate.LoadJsonString(tag.GetString("ChestBrowserUI"));
            }
            if (tag.ContainsKey("FilterItemTypeUI"))
            {
                ChestBrowser.instance.filterItemTypeTool.uistate.LoadJsonString(tag.GetString("FilterItemTypeUI"));
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
                    Player.tileRangeX = Config.isInfinityRange ? ChestBrowserUtils.InfinityRange : Config.searchRangeX / 2;
                    Player.tileRangeY = Config.isInfinityRange ? ChestBrowserUtils.InfinityRange : Config.searchRangeY / 2;
                }
            }
        }

    }
}
