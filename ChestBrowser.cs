using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;

namespace ChestBrowser
{
    class ChestBrowser : Mod
    {
        internal static ChestBrowser instance;
        internal ModHotKey HotKey;
        internal ChestBrowserTool chestBrowserTool;

        public bool LoadedFKTModSettings = false;

        int lastSeenScreenWidth;
        int lastSeenScreenHeight;

        public ChestBrowser()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }

        public override void Load()
        {
            // 旧設定ファイルの削除
            var oldConfigPath = Path.Combine(Main.SavePath, "Mod Configs", "ChestBrowser.json");
            if (File.Exists(oldConfigPath))
            {
                File.Delete(oldConfigPath);
            }

            instance = this;
            HotKey = RegisterHotKey("Toggle Chest Browser", "U");
            if (!Main.dedServ)
            {
                chestBrowserTool = new ChestBrowserTool();
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {

            int layerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interface Logic 1"));
            if (layerIndex != -1)
            {
                layers.Insert(layerIndex, new LegacyGameInterfaceLayer(
                    "ChestBrowser: UI",
                    delegate
                    {
                        if (lastSeenScreenWidth != Main.screenWidth || lastSeenScreenHeight != Main.screenHeight)
                        {
                            chestBrowserTool.ScreenResolutionChanged();
                            lastSeenScreenWidth = Main.screenWidth;
                            lastSeenScreenHeight = Main.screenHeight;
                        }
                        chestBrowserTool.UIUpdate();
                        chestBrowserTool.UIDraw();

                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }

            layerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (layerIndex != -1)
            {
                layers.Insert(layerIndex, new LegacyGameInterfaceLayer(
                    "ChestBrowser: Tooltip",
                    delegate
                    {
                        chestBrowserTool.TooltipDraw();
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public static bool isKillTileProtect()
        {
            bool result = true;
            if (!Main.dedServ)
            {
                result = ChestBrowser.instance.chestBrowserTool.visible && ModContent.GetInstance<ChestBrowserConfig>().isKillTileProtect;
            }
            return result;
        }
        public static bool isKillWallProtect()
        {
            bool result = true;
            if (!Main.dedServ)
            {
                result = ChestBrowser.instance.chestBrowserTool.visible && ModContent.GetInstance<ChestBrowserConfig>().isKillWallProtect;
            }
            return result;
        }
    }

    class ChestBrowserGlobalTile : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            return !ChestBrowser.isKillTileProtect();
        }
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (ChestBrowser.isKillTileProtect())
            {
                fail = true;
                effectOnly = true;
                noItem = true;
            }
        }
        public override bool CreateDust(int i, int j, int type, ref int dustType)
        {
            return !ChestBrowser.isKillTileProtect();
        }
        public override bool KillSound(int i, int j, int type)
        {
            return !ChestBrowser.isKillTileProtect();
        }
        public override bool Slope(int i, int j, int type)
        {
            return !ChestBrowser.isKillTileProtect();
        }
        public override bool CanPlace(int i, int j, int type)
        {
            return !ChestBrowser.isKillTileProtect();
        }
    }
    class ChestBrowserGlobalWall : GlobalWall
    {
        public override void KillWall(int i, int j, int type, ref bool fail)
        {
            fail = ChestBrowser.isKillTileProtect();
        }
        public override bool CreateDust(int i, int j, int type, ref int dustType)
        {
            return !ChestBrowser.isKillTileProtect();
        }
        public override bool KillSound(int i, int j, int type)
        {
            return !ChestBrowser.isKillTileProtect();
        }
    }
}
