using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.UI.Chat;

namespace ChestBrowser
{
    public static class Config
    {
        private static string ConfigPath = $@"{Main.SavePath}\Mod Configs\ChestBrowser.json";
        private static Preferences config;
        private static int version = 2;
        public static void LoadConfig()
        {
            config = new Preferences(ConfigPath);

            if (config.Load())
            {
                config.Get("version", ref version);
                config.Get("isCheatMode", ref isCheatMode);
                config.Get("isInfinityRange", ref isInfinityRange);
                config.Get("searchRangeX", ref searchRangeX);
                config.Get("searchRangeY", ref searchRangeY);
                if (2 <= version)
                {
                    config.Get("isKillTileProtect", ref isKillTileProtect);
                    config.Get("isKillWallProtect", ref isKillWallProtect);
                }
            }
            else
            {
                SaveValues();
            }
        }

        internal static void SaveValues()
        {
            config.Put("version", version);
            config.Put("isCheatMode", isCheatMode);
            config.Put("isInfinityRange", isInfinityRange);
            config.Put("searchRangeX", searchRangeX);
            config.Put("searchRangeY", searchRangeY);
            config.Save();
        }

        public static bool isCheatMode = false;
        public static bool isInfinityRange = false;
        public static int searchRangeX = Main.screenWidth / ChestBrowserUtils.tileSize;
        public static int searchRangeY = Main.screenHeight / ChestBrowserUtils.tileSize;
        //Version: 2
        public static bool isKillTileProtect = true;
        public static bool isKillWallProtect = true;
    }
}
