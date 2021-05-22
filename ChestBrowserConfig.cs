using System.ComponentModel;
using Terraria.ModLoader.Config;
using Terraria;

namespace ChestBrowser
{
    [Label("Config")]
    public class ChestBrowserConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public ChestBrowserConfig()
        {
            searchRange = new SearchRange();
        }

        [Label("Enable Cheat Mode")]
        [DefaultValue(false)]
        public bool isCheatMode;

        [Label("Enable Infinity Range")]
        [DefaultValue(false)]
        public bool isInfinityRange;

        [Label("Search Range")]
        public SearchRange searchRange;

        [Label("Enable Kill Tile Protect")]
        [DefaultValue(true)]
        public bool isKillTileProtect;

        [Label("Enable Kill Wall Protect")]
        [DefaultValue(true)]
        public bool isKillWallProtect;

        public class SearchRange
        {
            [Range(1, 500)]
            public int X;
            [Range(1, 500)]
            public int Y;

            public SearchRange()
            {
                X = Main.screenWidth / ChestBrowserUtils.tileSize;
                Y = Main.screenHeight / ChestBrowserUtils.tileSize;
            }
            public override bool Equals(object obj)
            {
                if (obj is SearchRange other)
                    return X == other.X && Y == other.Y;
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return new { X, Y }.GetHashCode();
            }
        }
    }
}
