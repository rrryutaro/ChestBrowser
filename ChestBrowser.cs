using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using FKTModSettings;

namespace ChestBrowser
{
	class ChestBrowser : Mod
	{
        internal static ChestBrowser instance;
        internal ModHotKey HotKey;
        internal ChestBrowserTool chestBrowserTool;
        internal FilterItemTypeTool filterItemTypeTool;

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
            instance = this;
            HotKey = RegisterHotKey("Toggle Chest Browser", "U");
            if (!Main.dedServ)
            {
                chestBrowserTool = new ChestBrowserTool();
                filterItemTypeTool = new FilterItemTypeTool();

                Config.LoadConfig();
                LoadedFKTModSettings = ModLoader.GetMod("FKTModSettings") != null;
                try
                {
                    if (LoadedFKTModSettings)
                    {
                        LoadModSettings();
                    }
                }
                catch { }


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
                        filterItemTypeTool.UIUpdate();
                        chestBrowserTool.UIDraw();
                        filterItemTypeTool.UIDraw();

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
                        filterItemTypeTool.TooltipDraw();
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void PreSaveAndQuit()
        {
            Config.SaveValues();
        }

        public override void PostUpdateInput()
        {
            try
            {
                if (LoadedFKTModSettings && !Main.gameMenu)
                {
                    UpdateModSettings();
                }
            }
            catch { }
        }

        private void LoadModSettings()
        {
            ModSetting setting = ModSettingsAPI.CreateModSettingConfig(this);
            setting.AddBool("isCheatMod", "Enable Cheat Mode", false);
            setting.AddBool("isInfinityRange", "Enable Infinity Range", false);
            setting.AddInt("searchRangeX", "Search Range X", 4, ChestBrowserUtils.maxTilesX, false);
            setting.AddInt("searchRangeY", "Search Range Y", 4, ChestBrowserUtils.maxTilesY, false);
        }

        private void UpdateModSettings()
        {
            ModSetting setting;
            if (ModSettingsAPI.TryGetModSetting(this, out setting))
            {
                setting.Get("isCheatMod", ref Config.isCheatMode);
                setting.Get("isInfinityRange", ref Config.isInfinityRange);
                setting.Get("searchRangeX", ref Config.searchRangeX);
                setting.Get("searchRangeY", ref Config.searchRangeY);
            }
        }

    }
}
